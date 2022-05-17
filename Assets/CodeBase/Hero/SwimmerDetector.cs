using System;
using CodeBase.GameEntities;
using UnityEngine;

namespace CodeBase.Hero
{
    public class SwimmerDetector : MonoBehaviour
    {
        private readonly Collider[] _hits = new Collider[1];
        private int _layerMask;
        private ISwimmer _swimmer;
        private void Start()
        {
            _swimmer = GetComponent<ISwimmer>();
            _layerMask = 1 << LayerMask.NameToLayer("Water");
        }

        public void LeaveWaterTrigger()
        {
            _hits[0] = null;
            Physics.OverlapSphereNonAlloc(transform.position + transform.forward*2, 0.1f, _hits, _layerMask);

            if (CheckIfStopSwim())
            {
                _swimmer.StopSwim();
            }
        }

        private bool CheckIfStopSwim()
        {
            return _hits[0] == null && _swimmer.SwimCondition;
        }
    }
}