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
        public GameObject driveTarget, aimTarget;
        public (GameObject drive, GameObject aim) GetTargetPoint() { return (driveTarget, aimTarget); }
        [SerializeField] protected GameObject closestEnemy, closestEnemyBase, closestCollectible;

        [SerializeField, Tooltip("Prioritise at X, Deprioritise at Y")] Vector2 lowHealthThreshold, lowFuelThreshold;
        [SerializeField, Tooltip("Prioritise at X, Deprioritise at Y")] Vector2 ammoThreshold;
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
                SwitchState();
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
            if(GetHealthLevel < lowHealthThreshold.x || GetFuelLevel < lowFuelThreshold.x)
            {
                currentState = stateDict[typeof(RetreatState)];
            }


            if(closestEnemy || closestEnemyBase)
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