using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.RuleBased
{
    public abstract class AdvancedState
    {
        protected SmartAbbleTank_Base ourTank;
        public AdvancedState(SmartAbbleTank_Base ourTank)
        {
            this.ourTank = ourTank;
        }

        public abstract void StateEnter();
        public abstract void StateExit();
        public abstract void StateUpdate();
    }
}