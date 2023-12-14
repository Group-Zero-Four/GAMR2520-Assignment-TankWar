using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamAbble.RuleBased
{
    public class TA_AdvBaseDefence : TA_AdvancedState
    {

        private float currentCircleAngle = 0;
        private float circleSpeed = 30;
        private readonly float circleRadius = 20;

        public TA_AdvBaseDefence(TA_SmartTankBase_RBS ourTank) : base(ourTank)
        {
        }

        public override void StateEnter()
        {

        }

        public override void StateExit()
        {

        }

        public override void StateUpdate()
        {
            currentCircleAngle += Time.deltaTime * circleSpeed;
            ourTank.driveTarget.transform.position = ourTank.defenceTarget.transform.position +
                (Quaternion.Euler(0, currentCircleAngle, 0) * Vector3.forward * circleRadius); ;

            ourTank.MoveTankToPoint(ourTank.driveTarget, 0.8f);
        }

        public override void CollisionCallback(Collision collision)
        {

        }
    }
}