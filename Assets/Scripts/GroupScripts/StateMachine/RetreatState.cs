using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
            GameObject collectibleFound = currentTank.GetClosestCollectible();

            if (collectibleFound.tag == "Health" && currentTank.GetHealth() != 125 )
            {
                currentTank.MoveTankToPoint(collectibleFound, 0.5f);
            }
            if (collectibleFound.tag == "Fuel" && currentTank.GetFuel() != 125)
            {
                currentTank.MoveTankToPoint(collectibleFound, 0.5f);
            }
            if (collectibleFound.tag == "Ammo" && currentTank.GetAmmo() != 15)
            {
                currentTank.MoveTankToPoint(collectibleFound, 0.5f);
            }
        }
    }
}