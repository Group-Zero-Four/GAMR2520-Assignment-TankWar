using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using TeamAbble.StateMachine;

namespace TeamAbble
{
    [Obsolete("Please use TA_SmartTank_RBS_2 instead. FSM-only implementation is not for practical use.")]
    public class TA_SmartTank_FSM : AITank
    {
        //Grabbed from Dumb Tank
        //store ALL currently visible 
        public Dictionary<GameObject, float> enemyTanksFound = new Dictionary<GameObject, float>();
        public Dictionary<GameObject, float> consumablesFound = new Dictionary<GameObject, float>();
        public Dictionary<GameObject, float> enemyBasesFound = new Dictionary<GameObject, float>();
        public GameObject driveTarget, aimTarget, initTarget;
        [SerializeField] protected GameObject closestEnemy, closestEnemyBase, closestCollectible;

        public float lowHealthThreshold, lowFuelThreshold;
        public float ammoThreshold;

        [SerializeField] string statename;
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
        Dictionary<Type, TA_BaseState_FSM> stateDict;
        TA_BaseState_FSM currentState;
        
        public override void AITankStart()
        {
            InitialiseStateMachine();
        }

        GameObject FindClosest(Dictionary<GameObject, float> targets)
        {
            float dist = -1;
            GameObject closest = null;
            foreach (var item in targets)
            {
                if(dist == -1)
                {
                    closest = item.Key;
                    dist = item.Value;
                }

                if(item.Value <  dist)
                {
                    closest = item.Key;
                    dist = item.Value;
                }
            }
            return closest;
        }

        public override void AITankUpdate()
        {
            Debug.DrawRay(driveTarget.transform.position, Vector3.up * 10, Color.red, 0.1f, false);

            //Populate found objects
            enemyTanksFound = TanksFound;
            enemyBasesFound = BasesFound;
            consumablesFound = ConsumablesFound;

            closestEnemyBase = FindClosest(enemyBasesFound);
            closestEnemy = FindClosest(enemyTanksFound);
            closestCollectible = FindClosest(consumablesFound);

            if (currentState != null)
            {
                currentState.UpdateState();
            }
            SwitchState();
        }
        public override void AIOnCollisionEnter(Collision collision)
        {
            StopTank();
        }
        
        void InitialiseStateMachine()
        {
            stateDict = new()
            {
                { typeof(TA_InitialState_FSM), new TA_InitialState_FSM() },
                { typeof(TA_SearchingState_FSM), new TA_SearchingState_FSM() },
                { typeof(TA_AttackState_FSM), new TA_AttackState_FSM() },
            };

            currentState = stateDict[typeof(TA_InitialState_FSM)];
            currentState?.EnterState(this);
        }

        bool IsInDanger()
        {
            return GetHealthLevel < lowHealthThreshold || GetAmmoLevel < ammoThreshold || GetFuelLevel < lowFuelThreshold;
        }

        void SwitchState()
        {
            if (closestEnemy || closestEnemyBase && !IsInDanger())
            {
                if (currentState is not TA_AttackState_FSM)
                {
                    currentState?.ExitState();
                    currentState = stateDict[typeof(TA_AttackState_FSM)];
                    currentState?.EnterState(this);
                    statename = "Attack";
                }
                return;
            }

            if (currentState is TA_InitialState_FSM && closestCollectible == null)
            {
                //Tank is in initial state and has not found a collectible, the state should not change.
                return;
            }
            if (currentState is not TA_SearchingState_FSM)
            {
                currentState?.ExitState();
                currentState = stateDict[typeof(TA_SearchingState_FSM)];
                currentState?.EnterState(this);
                statename = "Retreat";
            }
        }

        #region Proxy Methods
        public void AimTurretAtPoint(GameObject point)
        {
            FaceTurretToPoint(point);
        }
        public void MoveTankToPoint(GameObject point, float normalisedSpeed)
        {
            FollowPathToPoint(point, normalisedSpeed);
        }
        public void MoveTankRandom(float normalisedSpeed)
        {
            FollowPathToRandomPoint(normalisedSpeed);
        }
        public void FireAtSomething(GameObject point)
        {
            FireAtPoint(point);
        }
        public void RandomPointPlease()
        {
            GenerateRandomPoint();
        }
        public float GetHealth()
        {
            return GetHealthLevel;
        }
        public float GetAmmo()
        {
            return GetAmmoLevel;
        }
        public float GetFuel()
        {
            return GetFuelLevel;
        }
        public void GoTankGo()
        {
            StartTank();
        }
        public bool AmIFiring()
        {
            return IsFiring;
        }
        #endregion Proxy Methods

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(driveTarget.transform.position, 1);
            Gizmos.DrawWireSphere(aimTarget.transform.position, 1);
        }
    }
}