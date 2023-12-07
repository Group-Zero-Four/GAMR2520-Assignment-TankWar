using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.StateMachine
{
    public class SearchingState : BaseState
    {
        float countdownTimer = 10f;

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
            if (collectibleFound != null)
            {
                if (collectibleFound.CompareTag("Fuel") && currentTank.GetFuel() < 125)
                {
                    currentTank.MoveTankToPoint(collectibleFound, 0.5f);
                }
                else if (collectibleFound.CompareTag("Health") && currentTank.GetHealth() < 125)
                {
                    currentTank.MoveTankToPoint(collectibleFound, 0.5f);
                }
                else if (collectibleFound.CompareTag("Ammo") && currentTank.GetAmmo() < 15)
                {
                    currentTank.MoveTankToPoint(collectibleFound, 0.5f);
                }
            }
            else
            {
                currentTank.MoveTankRandom(0.5f);
                countdownTimer -= Time.deltaTime;
                if (countdownTimer < 0)
                {
                    countdownTimer = 10;
                    currentTank.RandomPointPlease();
                }
            }
        }
    }
}