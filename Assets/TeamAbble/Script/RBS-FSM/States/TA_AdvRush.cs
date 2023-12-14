using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamAbble.RuleBased
{
    /// <summary>
    /// Moves the tank directly to the enemy base. This state is only used once.
    /// </summary>
    public class TA_AdvRush : TA_AdvancedState
    {
        public TA_AdvRush(TA_SmartTankBase_RBS ourTank) : base(ourTank)
        {
        }

        public override void CollisionCallback(Collision collision)
        {
        }

        public override void StateEnter()
        {
            Debug.Log("Rushing!");
        }

        public override void StateExit()
        {

        }

        public override void StateUpdate()
        {
            ourTank.MoveTankToPoint(ourTank.rushTarget, 0.8f);
        }
    }
}