using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.StateMachine
{
    public class AttackState : BaseState
    {

        float minCircleRadius = 20, maxCircleRadius = 40;
        float currentCircleRadius = 0;
        bool targetingEnemy = false;
        bool circling = false;
        float currentCircleAngle = 0;
        float circleRotateSpeed = 10, circlingNormalisedMoveSpeed = 0.5f;
        float attackInterval = 10, currentAttackInterval = 0.2f;
        Vector3 circlePosition;
        public override void EnterState(CleverTank tank)
        {
            currentTank = tank;
            Debug.Log($"Entered State on {tank.gameObject.name}");
            currentCircleRadius = Random.Range(minCircleRadius, maxCircleRadius);
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

            if(targetingEnemy && !targetFound)
            {
                
            }

            if (targetFound != null)
            {
                targetingEnemy = true;
                currentTank.AimTurretAtPoint(targetFound);
                if (!circling)
                {
                    currentTank.driveTarget.transform.position = targetFound.transform.position;
                    currentTank.MoveTankToPoint(currentTank.driveTarget, 0.5f);
                }
                currentTank.aimTarget.transform.position = targetFound.transform.position;
                currentAttackInterval -= Time.deltaTime;
                if(currentAttackInterval <= 0)
                {
                    currentTank.FireAtSomething(targetFound);
                    currentAttackInterval = attackInterval;
                }

                if (Vector3.Distance(targetFound.transform.position, currentTank.transform.position) <= currentCircleRadius)
                {
                    CircleTarget(targetFound);
                    circling = true;
                }
                else
                {
                    circling = false;
                }
            }
            targetFound = null;

        }

        void CircleTarget(GameObject enemy)
        {
            circlePosition = Quaternion.Euler(0, currentCircleAngle, 0) * (Vector3.forward * currentCircleRadius)
                + enemy.transform.position;
            currentCircleAngle += Time.deltaTime * circleRotateSpeed;
            currentTank.driveTarget.transform.position = circlePosition;
            currentTank.MoveTankToPoint(currentTank.driveTarget, 0.5f);
        }
        
    }
}