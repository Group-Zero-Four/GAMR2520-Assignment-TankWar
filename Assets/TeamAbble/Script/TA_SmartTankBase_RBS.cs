using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamAbble.RuleBased
{
    public class TA_SmartTankBase_RBS : AITank
    {
        public Dictionary<string, bool> GetFacts { get { return facts; } }
        protected Dictionary<string, bool> facts = new Dictionary<string, bool>();
        public GameObject closestEnemy, closestBase, closestConsumable, prioritisedConsumable;
        [SerializeField] protected float lowHealthThreshold, lowFuelThreshold, lowAmmoThreshold;
        [Obsolete("Exists for back-compat with SmartAbbleTank_RBS_1")] protected TA_StringRules rules = new();
        protected TA_RuleTypes ruleTypes = new();
        protected float rushTimer = 10;
        protected bool rushing = true;
        protected Dictionary<Type, TA_AdvancedState> stateDictionary = new();
        public GameObject driveTarget, aimTarget, rushTarget, defenceTarget;
        public GameObject pointPrefab;
        public Vector3 driveTargStart, aimTargStart, rushTargStart, defTargStart;
        public override void AITankStart()
        {
            driveTarget = Instantiate(pointPrefab, driveTargStart, Quaternion.identity);
            driveTarget.name = "drive target";
            aimTarget = Instantiate(pointPrefab, aimTargStart, Quaternion.identity);
            aimTarget.name = "aim target";
            rushTarget = Instantiate(pointPrefab, rushTargStart, Quaternion.identity);
            rushTarget.name = "rush target";
            defenceTarget = Instantiate(pointPrefab, defTargStart, Quaternion.identity);
            defenceTarget.name = "defence target";
        }
        public override void AIOnCollisionEnter(Collision collision)
        {

        }
        public override void AITankUpdate()
        {

        }
        [ContextMenu("starting point setup")]
        public void GrabStartingPoints()
        {
            if (driveTarget)
                driveTargStart = driveTarget.transform.position;
            if (aimTarget)
                aimTargStart = driveTarget.transform.position;
            if(rushTarget)
                rushTargStart = rushTarget.transform.position;
            if(defenceTarget)
                defTargStart = defenceTarget.transform.position;

        }

        #region Helper Methods

        public void MoveTankToPoint(GameObject point, float normalisedSpeed)
        {
            FollowPathToPoint(point, normalisedSpeed);
        }
        public void MoveTankRandomly(float normalisedSpeed)
        {
            FollowPathToRandomPoint(normalisedSpeed);
        }
        public void NewRandomPoint()
        {
            GenerateRandomPoint();
        }
        public void AimAtPoint(GameObject point)
        {
            FaceTurretToPoint(point);
        }
        public void Fire(GameObject point)
        {
            FireAtPoint(point);
        }
        public void StopTheTank()
        {
            StopTank();
        }
        protected GameObject FindClosest(Dictionary<GameObject, float> targets)
        {
            float dist = -1;
            GameObject closest = null;
            foreach (var item in targets)
            {
                if (dist == -1)
                {
                    closest = item.Key;
                    dist = item.Value;
                }

                if (item.Value < dist)
                {
                    closest = item.Key;
                    dist = item.Value;
                }
            }
            return closest;
        }
        public float GetHealth { get { return GetHealthLevel; } }
        public float GetAmmo { get { return GetAmmoLevel; } }
        public float GetFuel { get { return GetFuelLevel; } }
        public bool GetFiring { get { return IsFiring; } }
        public GameObject GetClosestEnemy { get { return FindClosest(TanksFound); } }
        public GameObject GetClosestBase { get { return FindClosest(BasesFound); } }
        public GameObject GetClosestConsumable { get { return FindClosest(ConsumablesFound); } }
        #endregion Helper Methods
    }
}