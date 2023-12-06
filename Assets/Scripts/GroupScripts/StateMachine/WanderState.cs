using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ZeroFour.StateMachine
{
    public class WanderState : BaseState
    {
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

        }
    }
}
