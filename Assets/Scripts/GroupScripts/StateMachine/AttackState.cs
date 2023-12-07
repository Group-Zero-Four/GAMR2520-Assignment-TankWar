using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.StateMachine
{
    public class AttackState : BaseState
    {

        bool targetingEnemy = false;
        float attackInterval = 10, currentAttackInterval = 0.2f;
        float distancingTimer = 4f, currentDistancingTimer = 0f;
        public override string ToString()
        {
            return "Attack State";
        }
        public override void EnterState(CleverTank tank)
        {
            currentTank = tank;
            Debug.Log($"Entered State on {tank.gameObject.name}");
        }

        public override void ExitState()
        {
            //is health or fuel critically low:
            //Retreat
            //has enemy left line of sight?:
            //Chase
        }

        public override void UpdateState()
        {
            //Hardscope the enemy!
            GameObject targetFound = currentTank.GetClosestEnemy();
            if (!targetFound)
                targetFound = currentTank.GetClosestEnemyBase();

            //No target was found still. Why are we even in this state?
            if (!targetFound)
                return;

            if (Vector3.Distance(currentTank.transform.position, targetFound.transform.position) > 30)
            {
                currentTank.driveTarget.transform.position =
                    Vector3.Lerp(currentTank.transform.position, targetFound.transform.position, 0.7f);
                currentTank.MoveTankToPoint(currentTank.driveTarget, 0.4f);
            }
            else if (Vector3.Distance(currentTank.transform.position, targetFound.transform.position) <= 30)
            {
                currentDistancingTimer -= Time.deltaTime;
                if (!currentTank.AmIFiring())
                {
                    currentDistancingTimer -= Time.deltaTime;
                    Vector2 circ = (Random.insideUnitCircle * 25);
                    currentDistancingTimer -= Time.deltaTime;
                    if(currentDistancingTimer <= 0)
                    {
                        currentTank.driveTarget.transform.position = currentTank.transform.position + new Vector3(circ.x, 0, circ.y);
                        currentDistancingTimer = distancingTimer;
                    }
                    currentTank.MoveTankToPoint(currentTank.driveTarget, 1f);
                    currentDistancingTimer = distancingTimer;

                }
                if (currentAttackInterval <= 0 && !currentTank.AmIFiring())
                {
                    currentTank.FireAtSomething(targetFound);
                }

            }

            targetFound = null;
        }
    }
}