using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class Tower : CharacterBase
    {
        void Start()
        {
            Init();
        }
        public override void OnDead()
        {
            //GameOver
            throw new System.NotImplementedException();
        }
    }
}