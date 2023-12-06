using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.StateMachine
{
    public class AttackState : BaseState
    {
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
            GameObject enemyFound = currentTank.GetClosestEnemy();

            if (enemyFound != null)
            {
                currentTank.AimTurretAtPoint(enemyFound);
                currentTank.driveTarget.transform.position = enemyFound.transform.position;
                currentTank.aimTarget.transform.position = enemyFound.transform.position;
            }
        }
    }
}