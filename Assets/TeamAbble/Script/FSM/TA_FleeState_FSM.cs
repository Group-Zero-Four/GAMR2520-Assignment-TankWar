using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamAbble;
using TeamAbble.StateMachine;

public class TA_FleeState_FSM : TA_BaseState_FSM
{
    private GameObject target;
    public override string ToString()
    {
        return "FleeState";
    }
    public override void EnterState(TA_SmartTank_FSM tank)
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
