using UnityEngine;

namespace Charactor
{
    public class MinionDetection
    {
        private Transform self;
        private float detectRange;
        private LayerMask enemyLayer;

        public Transform CurrentTarget { get; private set; }

        public MinionDetection(Transform self, float detectRange, LayerMask targetLayer)
        {
            this.self = self;
            this.detectRange = detectRange;
            this.enemyLayer = targetLayer;
        }

        public void Detect()
        {
            Collider[] hits = Physics.OverlapSphere(self.position, detectRange, enemyLayer);

            if (hits.Length > 0)
            {
                CurrentTarget = hits[0].transform;
            }
            else
            {
                CurrentTarget = null;
            }
        }
    }
}