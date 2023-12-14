using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamAbble.RuleBased {
    /// <summary>
    /// Periodically determines a random point for the tank to move to.
    /// </summary>
    public class TA_AdvPatrol : TA_AdvancedState
    {
        float repathTime = 20, currRepathTime = 0;
        float speed = 0.6f;
        public TA_AdvPatrol(TA_SmartTankBase_RBS ourTank) : base(ourTank)
        {
        }

        public override void CollisionCallback(Collision collision)
        {

        }

        public override void StateEnter()
        {
            Debug.Log("Tank is patrolling...");
        }

        public override void StateExit()
        {

        }

        public override void StateUpdate()
        {
            if (currRepathTime > repathTime)
            {
                ourTank.NewRandomPoint();
                currRepathTime = repathTime;
            }
            ourTank.MoveTankRandomly(speed);
        }

    }
}