using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.StateMachine
{
    public abstract class BaseState
    {
        protected CleverTank currentTank;
        public abstract void EnterState(CleverTank tank);
        public abstract void ExitState();
        public abstract void UpdateState();
    }
}
