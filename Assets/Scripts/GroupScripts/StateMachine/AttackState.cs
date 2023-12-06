using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.StateMachine
{
    public class AttackState : BaseState
    {

        float minCircleRadius = 5, maxCircleRadius = 15;
        float currentCircleRadius = 0;
        bool targetingEnemy = false;
        bool circling = false;
        float currentCircleAngle = 0;
        float circleRotateSpeed = 10, circlingNormalisedMoveSpeed = 0.5f;
        float attackInterval;
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
            GameObject enemyFound = currentTank.GetClosestEnemy();

            if(targetingEnemy && !enemyFound)
            {
                
            }

            if (enemyFound != null)
            {
                targetingEnemy = true;
                currentTank.AimTurretAtPoint(enemyFound);
                currentTank.driveTarget.transform.position = enemyFound.transform.position;
                currentTank.aimTarget.transform.position = enemyFound.transform.position;

                if (Vector3.Distance(enemyFound.transform.position, currentTank.transform.position) <= currentCircleRadius)
                {
                    CircleTarget(enemyFound);
                }
            }
            enemyFound = null;
        }

        void CircleTarget(GameObject enemy)
        {
            circlePosition = Quaternion.Euler(0, currentCircleAngle, 0) * (Vector3.forward * currentCircleRadius)
                + enemy.transform.position;
            currentCircleAngle += Time.deltaTime * circleRotateSpeed;
            currentTank.driveTarget.transform.position = circlePosition;
        }
    }
}