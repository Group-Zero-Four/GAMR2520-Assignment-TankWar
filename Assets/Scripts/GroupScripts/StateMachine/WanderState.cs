using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ZeroFour.StateMachine
{
    public class WanderState : BaseState
    {
        //minimum and maximum random move distance
        //minimum and maximum times between finding new place to move to
        //is the tank moving to the target point?
        //distance from the target point to start using random points again.
        public override void EnterState(CleverTank tank)
        {
            currentTank = tank;
            Debug.Log($"Entered State on {tank.gameObject.name}");
        }

        public override void ExitState()
        {
            Debug.Log($"Exited State");
        }

        public override void UpdateState()
        {
            //Check for consumables, enemies or bases,
            //Move to target point if investigating or random point if not
        }
    }
}
