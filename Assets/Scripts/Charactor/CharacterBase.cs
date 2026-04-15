using Unity.Netcode;
using UnityEngine;

namespace Character
{
    public abstract class CharacterBase : NetworkBehaviour
    {
        [SerializeField] protected float m_fullBlood = 10;
        protected NetworkVariable<float> m_nowBlood = new NetworkVariable<float>();

        public float FullBlood => m_fullBlood;
        public float NowBlood => m_nowBlood.Value;

        public virtual void Init()
        {
            if (IsServer)
                m_nowBlood.Value = m_fullBlood;
        }

        public virtual void GetInjured(float value)
        {
            if (!IsServer) return;

            m_nowBlood.Value -= value;

            if (m_nowBlood.Value <= 0)
                OnDead();
        }

        public virtual void OnDead() { }
    }
}