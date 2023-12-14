using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeamAbble.RuleBased
{
    public class TA_AdvAttackBase : TA_AdvancedState
    {
        public TA_AdvAttackBase(TA_SmartTankBase_RBS ourTank) : base(ourTank)
        {
        }

        public override void CollisionCallback(Collision collision)
        {
            if (collision.transform.CompareTag("Base"))
            {
                ourTank.StopTheTank();
            }
        }

        public override void StateEnter()
        {

        }

        public override void StateExit()
        {

        }

        public override void StateUpdate()
        {
            if (ourTank.closestBase)
            {
                if (Vector3.Distance(ourTank.transform.position, ourTank.closestBase.transform.position) < 15)
                {
                    if (!ourTank.GetFiring)
                    {
                        ourTank.Fire(ourTank.closestEnemy);
                    }
                }
                else
                {
                    ourTank.MoveTankToPoint(ourTank.closestBase, 0.5f);
                }
            }
        }
    }
}