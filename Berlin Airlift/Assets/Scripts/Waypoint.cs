using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint NextWaypint;
    

}

/*
 * 
 * using System.Collections;
using System.Collections.Generic;
using AI.Tools;
using UnityEngine;

namespace AI.Actions
{
    public class AIActionPatrol : AIAction
    {
        public Waypoint nextWaypoint;

        private CharacterMovement _characterMovement;
        private float _minDistanceToWaypoint = 0.05f;

        public override void Init()
        {
            base.Init();
            _characterMovement = GetComponentInParent<CharacterMovement>();
        }

        Vector2 VectorToWaypoint()
        {
            if (nextWaypoint == null)
                return Vector2.zero;
            Vector3 vec = nextWaypoint.transform.position - transform.position;
            return vec;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            if (_character.characterAnimator)
            {
                _character.characterAnimator.SetBool("Chasing", false);
            }
        }

        public override void OnExitState()
        {
            base.OnExitState();
            _characterMovement.SetMoveDirection(Vector2.zero);
            
        }

        public override void PerformAction()
        {
            if (nextWaypoint == null)
                return;
            
            //update waypoint
            Vector2 toward = VectorToWaypoint();
            if (toward.magnitude < _minDistanceToWaypoint)
            {
                nextWaypoint = nextWaypoint.GetRandomNextWaypoint();
                if (nextWaypoint == null)
                    return;
            }

            //walk toward waypoint
            toward = VectorToWaypoint();
            Vector2 direction = toward.normalized;
            _characterMovement.SetMoveDirection(direction);
        }
    }
}
*/