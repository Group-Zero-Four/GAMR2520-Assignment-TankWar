using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ZeroFour.StateMachine
{
    public class InitialState : BaseState
    {

        public override string ToString()
        {
            return "Wander State";
        }

        float timer = 10; //timer variable
        //minimum and maximum random move distance
        //minimum and maximum times between finding new place to move to
        //is the tank moving to the target point?
        //distance from the target point to start using random points again.
        public override void EnterState(CleverTank tank)
        {
            currentTank = tank;
            Debug.Log($"Entered State on {tank.gameObject.name}");
            tank.StartCoroutine(DelayFirstPath());
        }
        IEnumerator DelayFirstPath()
        {
            yield return new WaitForSeconds(2f);
            currentTank.RandomPointPlease();
        }

        public override void ExitState()
        {
            //If Enemy in view
            //Attack
            Debug.Log($"Exited State");
        }

        public override void UpdateState()
        {
            //Check for consumables, enemies or bases,
            //Move to target point if investigating or random point if not
            timer -= Time.deltaTime;
            currentTank.MoveTankRandom(0.5f);
            if (timer <= 0)
            {
                timer = 10;
                currentTank.RandomPointPlease();
            }
                
        }
    }
}
