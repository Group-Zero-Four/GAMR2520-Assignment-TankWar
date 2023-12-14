using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamAbble.StateMachine
{
    public abstract class TA_BaseState_FSM
    {
        /// <summary>
        /// The current tank. This is our precious little boy :)
        /// </summary>
        protected TA_SmartTank_FSM currentTank;
        /// <summary>
        /// Called when the tank selects or enters this state. 
        /// <br></br>Passes the tank as a parameter so we always have the right tank.
        /// </summary>
        /// <param name="tank"></param>
        public abstract void EnterState(TA_SmartTank_FSM tank);
        /// <summary>
        /// Called when the state machine exits this state
        /// </summary>
        public abstract void ExitState();
        /// <summary>
        /// Called every frame
        /// </summary>
        public abstract void UpdateState();

    }
}
