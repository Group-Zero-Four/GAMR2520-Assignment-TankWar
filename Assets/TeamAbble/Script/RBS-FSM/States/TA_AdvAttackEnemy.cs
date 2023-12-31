using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamAbble.RuleBased
{
    public class TA_AdvAttackEnemy : TA_AdvancedState
    {

        float fireInterval = 10f, currentFireInterval = 5f;
        private float currentCircleAngle = 0;
        private float circleSpeed = 25;
        private readonly float circleRadius = 30;

        public TA_AdvAttackEnemy(TA_SmartTankBase_RBS ourTank) : base(ourTank)
        {
        }

        public override void StateEnter()
        {
            Debug.Log("entering attack state");
            
        }

        public override void StateExit()
        {

        }

        public override void StateUpdate()
        {
            GameObject closestEnemy = ourTank.GetClosestEnemy;
            float distance = Vector3.Distance(ourTank.transform.position, closestEnemy.transform.position);
            bool tooClose = distance < 15;
            if (closestEnemy)
            {
                Debug.Log("Attempting to attack an enemy");
                if (currentFireInterval > 0 || tooClose)
                {
                    currentCircleAngle += Time.deltaTime * circleSpeed;
                    ourTank.driveTarget.transform.position = closestEnemy.transform.position +
                        Quaternion.Euler(0, currentCircleAngle, 0) * Vector3.forward * circleRadius;
                    ourTank.MoveTankToPoint(ourTank.driveTarget, 0.5f);
                    ourTank.AimAtPoint(closestEnemy);
                    currentFireInterval -= Time.deltaTime;
                }
                else
                {
                    if (!ourTank.GetFiring && !tooClose)
                    {
                        ourTank.Fire(closestEnemy);
                        currentFireInterval = fireInterval;
                    }
                }
            }
            else
            {
                Debug.Log("hoe the fuck are we in the attack state without an enemy???");
                ourTank.MoveTankToPoint(ourTank.driveTarget, 0.5f);
                currentCircleAngle += Time.deltaTime * circleSpeed;
                Vector3 aimTargPos = Quaternion.Euler(0, currentCircleAngle, 0) * Vector3.forward * circleRadius;
                ourTank.aimTarget.transform.position = ourTank.transform.position + aimTargPos;
                ourTank.AimAtPoint(ourTank.aimTarget);
            }
            Debug.DrawRay(ourTank.driveTarget.transform.position, Vector3.forward, Color.red, 0.1f);
            Debug.DrawRay(ourTank.driveTarget.transform.position, Vector3.right, Color.red, 0.1f);
            Debug.DrawRay(ourTank.driveTarget.transform.position, Vector3.left, Color.red, 0.1f);
            Debug.DrawRay(ourTank.driveTarget.transform.position, Vector3.back, Color.red, 0.1f);


        }

        public override void CollisionCallback(Collision collision)
        {
        }
    }
}