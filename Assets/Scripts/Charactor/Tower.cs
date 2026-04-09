using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class Tower : CharacterBase
    {
        [SerializeField] Transform m_bloodBar;
        void Start()
        {
            Init();
        }
        public override void GetInjured(float value)
        {
            base.GetInjured(value);
            Vector3 scaleValue = this.m_bloodBar.localScale;
            scaleValue.x = Mathf.Lerp(0, 2, NowBlood / FullBlood);
            this.m_bloodBar.localScale = scaleValue;
        }
        public override void OnDead()
        {
            Debug.Log($"{this.name} Dead");
        }
    }
}