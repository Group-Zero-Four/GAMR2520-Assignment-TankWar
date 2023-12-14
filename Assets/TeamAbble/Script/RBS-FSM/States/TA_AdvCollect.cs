using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamAbble.RuleBased {
    public class TA_AdvCollect : TA_AdvancedState
    {
        public TA_AdvCollect(TA_SmartTankBase_RBS ourTank) : base(ourTank)
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
            ourTank.MoveTankToPoint(ourTank.closestConsumable, 0.75f);
        }
    }
}