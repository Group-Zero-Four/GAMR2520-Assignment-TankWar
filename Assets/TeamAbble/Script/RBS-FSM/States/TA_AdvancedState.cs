using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamAbble.RuleBased
{
    public abstract class TA_AdvancedState
    {
        protected TA_SmartTankBase_RBS ourTank;
        public TA_AdvancedState(TA_SmartTankBase_RBS ourTank)
        {
            this.ourTank = ourTank;
        }

        public abstract void StateEnter();
        public abstract void StateExit();
        public abstract void StateUpdate();
        public abstract void CollisionCallback(Collision collision);
    }
}