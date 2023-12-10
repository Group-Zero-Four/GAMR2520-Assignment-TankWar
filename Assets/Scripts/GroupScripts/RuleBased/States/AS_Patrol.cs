using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.RuleBased {
    /// <summary>
    /// Periodically determines a random point for the tank to move to.
    /// </summary>
    public class AS_Patrol : AdvancedState
    {
        float repathTime = 20, currRepathTime = 0;
        float speed = 0.6f;
        public AS_Patrol(SmartAbbleTank_Base ourTank) : base(ourTank)
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