using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZeroFour.StateMachine;

namespace ZeroFour
{
    public class CleverTank : AITank
    {
        //Grabbed from Dumb Tank
        //store ALL currently visible 
        public Dictionary<GameObject, float> enemyTanksFound = new Dictionary<GameObject, float>();
        public Dictionary<GameObject, float> consumablesFound = new Dictionary<GameObject, float>();
        public Dictionary<GameObject, float> enemyBasesFound = new Dictionary<GameObject, float>();

        Dictionary<Type, BaseState> stateDict;
        
        public override void AITankStart()
        {
            InitialiseStateMachine();
        }
        public override void AITankUpdate()
        {
            
        }
        public override void AIOnCollisionEnter(Collision collision)
        {

        }
        
        void InitialiseStateMachine()
        {
            stateDict = new()
            {
                { typeof(WanderState), new WanderState() },
                { typeof(ChaseState), new ChaseState() },
                { typeof(AttackState), new AttackState() },
            };
        }
    }
}