﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace TeamAbble.StateMachine
{
    public class TA_InitialState_FSM : TA_BaseState_FSM
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
        public override void EnterState(TA_SmartTank_FSM tank)
        {
            currentTank = tank;
            Debug.Log($"Entered State on {tank.gameObject.name}");

        }


        public override void ExitState()
        {
            //If Enemy in view
            //Attack
            Debug.Log($"Exited State");
        }

        public override void UpdateState()
        {
            currentTank.MoveTankToPoint(currentTank.initTarget,0.5f);
                
        }
    }
}
