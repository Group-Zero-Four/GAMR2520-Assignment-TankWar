using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.RuleBased {
    public class AS_Patrol : AdvancedState
    {
        float repathTime = 20, currRepathTime = 0;
        float speed = 0.6f;
        public AS_Patrol(SmartAbbleTank_RBS_1 ourTank) : base(ourTank)
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
            if (currRepathTime > repathTime)
            {
                ourTank.NewRandomPoint();
            }
            ourTank.MoveTankRandomly(speed);
        }

    }
}