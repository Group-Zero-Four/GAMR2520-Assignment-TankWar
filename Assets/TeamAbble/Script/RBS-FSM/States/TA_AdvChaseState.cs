using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamAbble.RuleBased
{
    public class TA_AdvChaseState : TA_AdvancedState
    {
        public TA_AdvChaseState(TA_SmartTankBase_RBS ourTank) : base(ourTank)
        {
        }

        public override void CollisionCallback(Collision collision)
        {

        }

        public override void StateEnter()
        {

        }

        public override void StateExit()
        {

        }

        public override void StateUpdate()
        {
            GameObject closestEnemy = ourTank.GetClosestEnemy;
            if (closestEnemy)
            {
                ourTank.MoveTankToPoint(closestEnemy, 0.7f);
            }
        }
    }
}