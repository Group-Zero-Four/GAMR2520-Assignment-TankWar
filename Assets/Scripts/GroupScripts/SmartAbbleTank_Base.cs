using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.RuleBased
{
    public class SmartAbbleTank_Base : AITank
    {
        public Dictionary<string, bool> GetFacts { get { return facts; } }
        protected Dictionary<string, bool> facts = new Dictionary<string, bool>();
        public GameObject closestEnemy, closestBase, closestConsumable, prioritisedConsumable;
        [SerializeField] protected float lowHealthThreshold, lowFuelThreshold, lowAmmoThreshold;
        protected Rules rules = new();
        protected float rushTimer = 10;
        protected bool rushing = true;
        public override void AITankStart()
        {

        }
        public override void AIOnCollisionEnter(Collision collision)
        {

        }
        public override void AITankUpdate()
        {

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
        #endregion Helper Methods
    }
}