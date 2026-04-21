using System.Collections.Generic;
using EventDispatcher;
using Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace ObjectPool
{
    public class SpawnManager : NetworkBehaviour
    {
        public NetworkObject m_pool = null;

#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.InlineEditor(Sirenix.OdinInspector.InlineEditorObjectFieldModes.Foldout)]
#endif
        [SerializeField] private PoolConfigGroup poolConfigGroup = null;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        [SerializeField] private List<PoolConfig> configs => poolConfigGroup.Configs;

        private Dictionary<string, Queue<PooledObject>> poolDict = new();
        [SerializeField] UnityEngine.InputSystem.PlayerInput testInput;

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;

            foreach (var config in configs)
            {
                var queue = new Queue<PooledObject>();

                for (int i = 0; i < config.initialSize; i++)
                {
                    var obj = CreateNew(config);
                    obj.gameObject.SetActive(false);
                    queue.Enqueue(obj);
                }

                poolDict[config.Key] = queue;
            }
        }

        private PooledObject CreateNew(PoolConfig config)
        {
            GameObject go = Instantiate(config.prefab);
            if (go.TryGetComponent<PooledObject>(out var pooled))
                pooled.SetPool(config.Key);
            if (go.TryGetComponent<NetworkObject>(out var netObj))
            {
                netObj.Spawn(true); // Netcode生成
                netObj.TrySetParent(m_pool);
            }

            return pooled;
        }

        // Server 呼叫
        public PooledObject Spawn(string key, ESide side, System.Action callback = null)
        {
            Vector3 pos = GetSideRendomPos(side);
            Quaternion rot = GetSideRot(side);
            return Spawn(key, pos, rot, callback);
        }
        public PooledObject Spawn(string key, Vector3 pos, Quaternion rot, System.Action callback = null)
        {
            if (!IsServer) return null;

            var pool = poolDict[key];

            PoolConfig config = GetConfig(key);
            PooledObject obj = pool.Count > 0
                ? pool.Dequeue()
                : CreateNew(config);

            if (config.recycleTime > 0)
                obj.StartCoroutine(obj.LifeTimer(config.recycleTime));

            // 同步 Transform
            if (obj.TryGetComponent<NetworkObject>(out var netObj))
                netObj.TrySetParent(m_pool);
            obj.OnSpawnFromPoolClientRpc(pos, rot);
            callback?.Invoke();

            return obj;
        }
        
        public void ReturnToPool(PooledObject obj)
        {
            if (!IsServer) return;

            obj.OnReturnToPoolClientRpc();

            poolDict[obj.Key].Enqueue(obj);
        }

        private PoolConfig GetConfig(string id)
        {
            return configs.Find(c => c.Key == id);
        }

        public Vector3 GetSideRendomPos(ESide side)
        {
            switch (side)
            {
                case ESide.Red:
                    return new Vector3(Random.Range(-14, -10), 0, Random.Range(-4, 4));
                case ESide.Blue:
                    return new Vector3(Random.Range(10, 14), 0, Random.Range(-4, 4));
                default:
                    return Vector3.down;
            }
        }
        public Quaternion GetSideRot (ESide side)
        {
            switch (side)
            {
                case ESide.Red:
                    return Quaternion.Euler(0, 90, 0);
                case ESide.Blue:
                    return Quaternion.Euler(0, 270, 0);
                default:
                    return Quaternion.Euler(Vector3.zero);
            }            
        }
        public void OnSpawnButtonClick(string name)
        {
            Spawn_Minion_ServerRpc(string.Format(name, GameManager.Instance.MySide), GameManager.Instance.MySide);
        }
        [Rpc(SendTo.Server)]
        public void Spawn_Minion_ServerRpc(string name, ESide side)
        {
            Spawn(name, side);
        }

#region test
    public void Test_Spawn(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case UnityEngine.InputSystem.InputActionPhase.Started:
                if (ctx.control.displayName == UnityEngine.InputSystem.Key.Z.ToString())
                {
                    Spawn_Minion_ServerRpc(ConstString.PooledObject.S_Red_Minion, ESide.Red);
                }
                else if (ctx.control.displayName == UnityEngine.InputSystem.Key.X.ToString())
                {
                    Spawn_Minion_ServerRpc(ConstString.PooledObject.S_Red_Minion2, ESide.Red);
                }
                else if (ctx.control.displayName == "/")
                {
                    Spawn_Minion_ServerRpc(ConstString.PooledObject.S_Blue_Minion, ESide.Blue);
                }
                else if (ctx.control.displayName == ".")
                {
                    Spawn_Minion_ServerRpc(ConstString.PooledObject.S_Blue_Minion2, ESide.Blue);
                }
                else if (ctx.control.displayName == "C")
                {
                    Dispatcher.Instance.Dispatch(new RecycleEvent());
                }
                break;
            }
        }
    #endregion
    }
}