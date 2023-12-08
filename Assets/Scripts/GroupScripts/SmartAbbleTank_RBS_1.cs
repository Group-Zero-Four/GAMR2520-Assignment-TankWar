using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace ZeroFour.RuleBased
{
    public class SmartAbbleTank_RBS_1 : AITank
    {
        Dictionary<string, bool> facts = new Dictionary<string, bool>();
        public Dictionary<string, bool> GetFacts { get { return facts; } }
        Rules rules = new();
        Dictionary<Type, AdvancedState> stateDictionary = new();
        AdvancedState currentState;

        public GameObject driveTarget, aimTarget, rushTarget;

        /*Tank needs the following states
         * Rush state - the first one.
         * it'll be in the rush state until it first finds an enemy or collectible.
         * 
         * Patrol state - an "idle" state where the tank will randomly path periodically.
         * 
         * Collect state - When a consumable is found, it'll path to the consumable and pick it up.
         * The collect state will also have a tag to prioritise.
         * 
         * Enemy Attack state - when an enemy is found, the tank will path to a random point around the enemy,
         * fire, and then run away.
         * 
         * Base Attack state - when a base is found, the tank will get within a specified distance, fire, then leave
         * 
         * Base Defence state - when one or both bases are lost, the tank will path back to the start point.
         * 
         * 
         */


        public override void AIOnCollisionEnter(Collision collision)
        {

        }

        public override void AITankStart()
        {
            InitialiseStateMachine();
        }

        public override void AITankUpdate()
        {

            EvaluateRules();
        }

        void EvaluateRules()
        {

        }
        void InitialiseStateMachine()
        {
            stateDictionary.Add(typeof(AdvancedState), new AS_Patrol(this));
        }

        public IEnumerator TimedFactToggle(float time, string fact, bool state)
        {
            facts[fact] = state;
            yield return new WaitForSeconds(time);
            facts[fact] = !state;
            yield return new WaitForFixedUpdate();
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
        public float GetHealth { get { return GetHealthLevel; } }
        public float GetAmmo { get {  return GetAmmoLevel; } }
        public float GetFuel { get { return GetFuelLevel; } }

        #endregion Helper Methods
    }
}