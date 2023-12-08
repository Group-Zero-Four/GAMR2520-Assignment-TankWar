using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.RuleBased
{
    public class AS_Rush : AdvancedState
    {
        public AS_Rush(SmartAbbleTank_RBS_1 ourTank) : base(ourTank)
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