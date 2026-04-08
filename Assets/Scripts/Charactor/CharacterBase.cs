using UnityEngine;

namespace Character
{
    public abstract class CharacterBase : MonoBehaviour
    {
        [SerializeField] protected float m_fullBlood = 10;
        protected float m_nowBlood;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public float NowBlood => m_nowBlood;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
#endif
        public virtual void Init()
        {
            m_nowBlood = m_fullBlood;
        }
        /// <summary>
        /// 受傷
        /// </summary>
        /// <param name="value"></param>
        public virtual void GetInjured(float value)
        {
            m_nowBlood -= value;
            if (NowBlood <= 0)
                OnDead();
        }
        public abstract void OnDead();
    }
}