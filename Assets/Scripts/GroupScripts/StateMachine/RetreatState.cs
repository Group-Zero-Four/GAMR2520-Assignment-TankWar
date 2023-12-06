using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.StateMachine
{
    public class RetreatState : BaseState
    {

        public override void EnterState(CleverTank tank)
        {
            currentTank = tank;
            Debug.Log($"Entered State on {tank.gameObject.name}");
        }

        public override void ExitState()
        {
            //if health and fuel are above critical AND enemy target is in view
            //AttackState
            //if health and fuel are above critical AND enemy is NOT in view
            //Wander
        }


        public override void UpdateState()
        {

        }
    }
}