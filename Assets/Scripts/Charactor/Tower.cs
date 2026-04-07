using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class Tower : CharacterBase
    {
        public override void OnDead()
        {
            //GameOver
            throw new System.NotImplementedException();
        }
    }
}