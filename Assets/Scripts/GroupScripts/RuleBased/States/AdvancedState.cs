using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.RuleBased
{
    public abstract class AdvancedState
    {
        protected SmartAbbleTank_RBS_1 ourTank;
        public AdvancedState(SmartAbbleTank_RBS_1 ourTank)
        {
            this.ourTank = ourTank;
        }

        public abstract void StateEnter();
        public abstract void StateExit();
        public abstract void StateUpdate();
    }
}