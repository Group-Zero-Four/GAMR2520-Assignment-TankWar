using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace ZeroFour.RuleBased
{
    /// <summary>
    /// A Rule- based state machine implementation of the Tank AI.
    /// </summary>
    public class SmartAbbleTank_RBS_1 : AITank
    {
        #region Fact Strings

        public const string LOWHEALTH = "Low Health", LOWAMMO = "Low Ammo", LOWFUEL = "Low Fuel", DEFENDING = "Defending",
            ENEMYSEEN = "Enemy Seen", BASESEEN = "Base Seen", BASELOST = "Base Lost", RETREATING = "Retreating", COLLECTABLESEEN = "Collectable Seen";
        public const string ATTACKINGBASE = "Attacking Base", ATTACKINGENEMY = "Attacking Enemy", COLLECTING = "Collecting",
            HEALTHFULL = "Health Full", AMMOFULL = "Ammo Full", FUELFULL = "Fuel Full", NEEDSCONSUMABLE = "Needs Consumable";
        #endregion Fact Strings



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
            InitialiseRules();
            InitialiseStateMachine();
        }

        public override void AITankUpdate()
        {
            EvaluateRules();
        }

        void EvaluateRules()
        {

        }
        void InitialiseRules()
        {
            //Add facts
            facts.Add(LOWHEALTH, false);
            facts.Add(LOWAMMO, false);
            facts.Add(LOWFUEL, false);
            facts.Add(ENEMYSEEN, false);
            facts.Add(BASESEEN, false);
            facts.Add(BASELOST, false);
            facts.Add(RETREATING, false);
            facts.Add(ATTACKINGBASE, false);
            facts.Add(ATTACKINGENEMY, false);
            facts.Add(COLLECTABLESEEN, false);
            facts.Add(COLLECTING, false);
            facts.Add(NEEDSCONSUMABLE, false);
            facts.Add(HEALTHFULL, true);
            facts.Add(AMMOFULL, true);
            facts.Add(FUELFULL, true);
            //Add rules
            string[] ruleconditions = new string[]
            {
                LOWHEALTH, LOWAMMO, LOWFUEL
            };
            //Danger rule - low health, ammo or fuel
            rules.AddRule(new(ruleconditions, RETREATING, Rule.Predicate.Or));
            //Attacking enemy rule - if enemy is seen and not retreating
            rules.AddRule(new(ENEMYSEEN, RETREATING, ATTACKINGENEMY, Rule.Predicate.OnlyFirst));
            //Attacking base rule - if base is seen and not reatreating
            rules.AddRule(new(BASESEEN, RETREATING, ATTACKINGBASE, Rule.Predicate.OnlyFirst));
            ruleconditions = new string[]
            {
                HEALTHFULL, AMMOFULL, FUELFULL
            };
            //If health, ammo or fuel are NOT full, a consumable is needed
            rules.AddRule(new(ruleconditions, NEEDSCONSUMABLE, Rule.Predicate.nAnd));
            ruleconditions = new string[]
            {
                NEEDSCONSUMABLE, ATTACKINGBASE, ATTACKINGENEMY
            };
            //If an enemy or base is NOT visible, and a consumable is needed, path to consumables
            rules.AddRule(new(ruleconditions, COLLECTING, Rule.Predicate.OnlyFirst));
            //If we have less than two bases and are not in danger, return to the bases to defend them.
            rules.AddRule(new(BASELOST, RETREATING, DEFENDING, Rule.Predicate.OnlyFirst));
        }
        void InitialiseStateMachine()
        {
            stateDictionary.Add(typeof(AS_Rush), new AS_Rush(this));
            stateDictionary.Add(typeof(AS_Patrol), new AS_Patrol(this));
            

            currentState = stateDictionary[typeof(AS_Rush)];
            currentState?.StateEnter();
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
        public void StopTheTank()
        {
            StopTank();
        }
        public float GetHealth { get { return GetHealthLevel; } }
        public float GetAmmo { get {  return GetAmmoLevel; } }
        public float GetFuel { get { return GetFuelLevel; } }

        #endregion Helper Methods
    }
}