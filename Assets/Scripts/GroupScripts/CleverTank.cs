using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        public GameObject targetPoint;
        public GameObject GetTargetPoint() { return targetPoint; }
        [SerializeField] protected GameObject closestEnemy, closestEnemyBase, closestCollectible;

        [SerializeField] float lowHealthThreshold, lowFuelThreshold;
        [SerializeField] int lowAmmoThreshold;
        public GameObject GetClosestEnemy()
        {
            return closestEnemy;
        }
        public GameObject GetClosestEnemyBase()
        {
            return closestEnemyBase;
        }
        public GameObject GetClosestCollectible()
        {
            return closestCollectible;
        }
        Dictionary<Type, BaseState> stateDict;
        BaseState currentState;
        
        public override void AITankStart()
        {
            InitialiseStateMachine();
        }
        public override void AITankUpdate()
        {
            if (currentState != null)
            {
                currentState.UpdateState();
                if (currentState.StateNeedsToChange())
                {
                    SwitchState();
                }
            }
        }
        public override void AIOnCollisionEnter(Collision collision)
        {
            StopTank();
        }
        
        void InitialiseStateMachine()
        {
            stateDict = new()
            {
                { typeof(WanderState), new WanderState() },
                { typeof(RetreatState), new RetreatState() },
                { typeof(AttackState), new AttackState() },
            };
        }
        void SwitchState()
        {
            //Evaluate conditions and then select the appropriate state
            //Prioritise attacking if health is above low health threshold
            if(GetHealthLevel < lowHealthThreshold)
            {
                currentState = stateDict[typeof(RetreatState)];
            }


            if((closestEnemy || closestEnemyBase))
            {
                currentState = stateDict[typeof(AttackState)];
                return;
            }

        }


        #region Proxy Methods
        public void AimTurretAtPoint(GameObject point)
        {
            FaceTurretToPoint(point);
        }
        #endregion Proxy Methods
    }
}