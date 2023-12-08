using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZeroFour;
using ZeroFour.StateMachine;

public class FleeState : BaseState
{
    private GameObject target;
    public override string ToString()
    {
        return "FleeState";
    }
    public override void EnterState(CleverTank tank)
    {
        currentTank = tank;
        Debug.Log($"Entered State on {tank.gameObject.name}");
    }
    public override void ExitState()
    {
        Debug.Log("exited state");
    }

    public override void UpdateState()
    {
        currentTank.MoveTankToPoint(currentTank.GetClosestCollectible(),1);
    }
}
