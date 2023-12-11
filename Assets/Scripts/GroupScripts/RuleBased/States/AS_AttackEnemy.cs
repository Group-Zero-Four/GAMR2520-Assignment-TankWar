using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.RuleBased
{
    public class AS_AttackEnemy : AdvancedState
    {

        float fireInterval = 10f, currentFireInterval = 5f;
        private float currentCircleAngle = 0;
        private float circleSpeed = 25;
        private readonly float circleRadius = 30;

        public AS_AttackEnemy(SmartAbbleTank_Base ourTank) : base(ourTank)
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
            if (closestEnemy)
            {
                Debug.Log("Attempting to attack an enemy");
                if (currentFireInterval > 0)
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
                    if (!ourTank.GetFiring)
                    {
                        ourTank.Fire(closestEnemy);
                    }
                    currentFireInterval = fireInterval;
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
    }
}