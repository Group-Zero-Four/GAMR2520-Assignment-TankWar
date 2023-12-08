using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.StateMachine
{
    public class AttackState : BaseState
    {

        bool targetingEnemy = false;
        float attackInterval = 10f, currentAttackInterval = 0.2f;
        float distancingTimer = 5f, currentDistancingTimer = 0f;
        bool targetIsBase = false;
        float circleAngle = 0, circleSpeed = 20, circleRadius = 30;
        public override string ToString()
        {
            return "Attack State";
        }
        public override void EnterState(SmartAbbleTank_FSM_1 tank)
        {
            currentTank = tank;
            Debug.Log($"Entered State on {tank.gameObject.name}");

            GameObject targetFound = currentTank.GetClosestEnemy();
            if (!targetFound)
                targetFound = currentTank.GetClosestEnemyBase();

            circleAngle = Vector3.Angle(tank.transform.position, targetFound.transform.position);
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
            targetIsBase = false;
            if (!targetFound)
            {
                targetFound = currentTank.GetClosestEnemyBase();
                if (targetFound)
                    targetIsBase = false;
            }
            //No target was found still. Why are we even in this state?
            if (!targetFound)
                return;

            if (targetIsBase)
            {
                Vector3 direction = (targetFound.transform.position - currentTank.transform.position).normalized;
                circleAngle += Time.deltaTime * circleSpeed;
                Vector3 drivePosition = Quaternion.Euler(0, circleAngle, 0) * (Vector3.forward * circleRadius) + targetFound.transform.position;
                currentTank.driveTarget.transform.position = drivePosition;
                currentTank.MoveTankToPoint(currentTank.driveTarget, 0.5f);
            }
            else
            {

            }
            currentTank.AimTurretAtPoint(targetFound);
            targetFound = null;
        }
    }
}