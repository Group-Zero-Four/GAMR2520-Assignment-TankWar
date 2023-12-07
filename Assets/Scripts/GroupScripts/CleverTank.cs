using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
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
        [SerializeField] protected GameObject closestEnemy, closestEnemyBase, closestCollectible;

        [Tooltip("Prioritise at X, Deprioritise at Y")] public Vector2 lowHealthThreshold, lowFuelThreshold;
        [Tooltip("Prioritise at X, Deprioritise at Y")] public Vector2 ammoThreshold;

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
        Dictionary<Type, BaseState> stateDict;
        BaseState currentState;
        
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
                { typeof(WanderState), new WanderState() },
                { typeof(RetreatState), new RetreatState() },
                { typeof(AttackState), new AttackState() },
            };

            SwitchState();
        }
        void SwitchState()
        {
            //Evaluate conditions and then select the appropriate state
            //Prioritise attacking if health is above low health threshold
            if (GetHealthLevel < lowHealthThreshold.x || GetFuelLevel < lowFuelThreshold.x || GetAmmoLevel < ammoThreshold.x)
            {
                if (currentState is not RetreatState)
                {
                    currentState?.ExitState();
                    currentState = stateDict[typeof(RetreatState)];
                    currentState?.EnterState(this);
                    statename = "Retreat";
                }
                return;
            }

            if (closestEnemy || closestEnemyBase)
            {
                if (currentState is not AttackState)
                {
                    currentState?.ExitState();
                    currentState = stateDict[typeof(AttackState)];
                    currentState?.EnterState(this);
                    statename = "Attack";
                }
                return;
            }

            if (currentState is not WanderState)
            {
                currentState?.ExitState();
                currentState = stateDict[typeof(WanderState)];
                currentState?.EnterState(this);
                statename = "Wander";
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