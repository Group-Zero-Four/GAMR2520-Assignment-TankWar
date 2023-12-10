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
    public class SmartAbbleTank_RBS_1 : SmartAbbleTank_Base
    {
        #region Fact Strings

        public const string LOWHEALTH = "Low Health", LOWAMMO = "Low Ammo", LOWFUEL = "Low Fuel", DEFENDING = "Defending",
            ENEMYSEEN = "Enemy Seen", BASESEEN = "Base Seen", BASELOST = "Base Lost", RETREATING = "Retreating", COLLECTABLESEEN = "Collectable Seen";
        public const string ATTACKINGBASE = "Attacking Base", ATTACKINGENEMY = "Attacking Enemy", COLLECTING = "Collecting",
            HEALTHFULL = "Health Full", AMMOFULL = "Ammo Full", FUELFULL = "Fuel Full", NEEDSCONSUMABLE = "Needs Consumable", PATROLLING = "Patrolling";
        #endregion Fact Strings
        public Dictionary<string, Type> factStatePairs = new();

        AdvancedState currentState;


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
            closestEnemy = FindClosest(TanksFound);
            closestBase = FindClosest(BasesFound);
            closestConsumable = FindClosest(ConsumablesFound);
            rushing = rushTimer > 0;
            if (!rushing)
            {
                EvaluateRules();
            }
            else
            {
                rushTimer -= Time.deltaTime;
            }
            currentState?.StateUpdate();

        }

        void EvaluateRules()
        {
            //Danger evaluation
            facts[LOWHEALTH] = GetHealth < lowHealthThreshold;
            facts[HEALTHFULL] = GetHealth > 100;
            facts[LOWFUEL] = GetFuel < lowFuelThreshold;
            facts[FUELFULL] = GetFuel > 100;
            facts[LOWAMMO] = GetAmmo < lowAmmoThreshold;
            facts[AMMOFULL] = GetAmmo > 100;

            //Attack evaluation
            facts[ENEMYSEEN] = closestEnemy;
            facts[BASESEEN] = closestBase;
            facts[BASELOST] = GetMyBases.Count < 2;

            facts[COLLECTABLESEEN] = closestConsumable;
            print( $"collectable - {facts[COLLECTABLESEEN]} enemy - {facts[ENEMYSEEN]} base - {facts[BASESEEN]}");

            foreach (var item in rules.GetRules)
            {
                var checkedFacts = item.TryCheckRule(facts);
                //Then patrol

                print($"getting collectable - {checkedFacts[COLLECTING]} attacking enemy - {checkedFacts[ATTACKINGENEMY]} attacking base - {checkedFacts[ATTACKINGBASE]}");


                print($"{checkedFacts[PATROLLING]}, {checkedFacts[RETREATING]}");
                if (checkedFacts[RETREATING])
                {
                    print("patrolling");
                    SwitchState(stateDictionary[factStatePairs[PATROLLING]].GetType());
                    return;
                }
                if (checkedFacts[ATTACKINGENEMY])
                {
                    Debug.Log("Attacking enemy!");
                    SwitchState(stateDictionary[factStatePairs[ATTACKINGENEMY]].GetType());
                    return;
                }
                if (checkedFacts[ATTACKINGBASE])
                {
                    print("Attacking base!");
                    SwitchState(stateDictionary[factStatePairs[ATTACKINGBASE]].GetType());
                    return;
                }
                //Then collect
                if (checkedFacts[COLLECTING]) 
                {
                    print("collecting item!");
                    SwitchState(stateDictionary[factStatePairs[COLLECTING]].GetType());
                    return;
                }
                if (checkedFacts[PATROLLING])
                {
                    print("patrolling");
                    SwitchState(stateDictionary[factStatePairs[PATROLLING]].GetType());
                    return;
                }
            }
        }
        void SwitchState(Type nextState)
        {
            if (currentState != null)
            {
                if (currentState.GetType() != nextState)
                {
                    currentState?.StateExit();
                    currentState = stateDictionary[nextState];
                    currentState?.StateEnter();
                }
            }
            else
            {
                currentState?.StateExit();
                currentState = stateDictionary[nextState];
                currentState?.StateEnter();
            }
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
            facts.Add(PATROLLING, false);
            //Add rules
            //Danger rule - low health, ammo or fuel
            rules.AddRule(new(LOWHEALTH, LOWAMMO, LOWFUEL, RETREATING, Rule.Predicate.Or));
            //Attacking enemy rule - if enemy is seen and not retreating
            rules.AddRule(new(ENEMYSEEN, RETREATING, ATTACKINGENEMY, Rule.Predicate.OnlyFirst));
            //Attacking base rule - if base is seen and not reatreating
            rules.AddRule(new(BASESEEN, RETREATING, ATTACKINGBASE, Rule.Predicate.OnlyFirst));
            //If health, ammo or fuel are NOT full, a consumable is needed
            rules.AddRule(new(HEALTHFULL, AMMOFULL, FUELFULL, NEEDSCONSUMABLE, Rule.Predicate.nAnd));
            //If an enemy or base is NOT visible, and a consumable is needed, path to consumables
            rules.AddRule(new(COLLECTABLESEEN, NEEDSCONSUMABLE, COLLECTING, Rule.Predicate.And));
            //If we have less than two bases and are not in danger, return to the bases to defend them.
            rules.AddRule(new(BASELOST, RETREATING, DEFENDING, Rule.Predicate.OnlyFirst));

            rules.AddRule(new(ENEMYSEEN, BASESEEN, COLLECTING, PATROLLING, Rule.Predicate.nAnd));
        }
        void InitialiseStateMachine()
        {
            stateDictionary.Add(typeof(AS_Rush), new AS_Rush(this));
            stateDictionary.Add(typeof(AS_Patrol), new AS_Patrol(this));
            stateDictionary.Add(typeof(AS_AttackBase), new AS_AttackBase(this));
            stateDictionary.Add(typeof(AS_AttackEnemy), new AS_AttackEnemy(this));
            stateDictionary.Add(typeof(AS_Collect), new AS_Collect(this));

            factStatePairs.Add(PATROLLING, typeof(AS_Patrol));
            factStatePairs.Add(ATTACKINGENEMY, typeof(AS_AttackEnemy));
            factStatePairs.Add(ATTACKINGBASE, typeof(AS_AttackBase));
            factStatePairs.Add(COLLECTING, typeof(AS_Collect));

            SwitchState(typeof(AS_Rush));
        }

        public IEnumerator TimedFactToggle(float time, string fact, bool state)
        {
            facts[fact] = state;
            yield return new WaitForSeconds(time);
            facts[fact] = !state;
            yield return new WaitForFixedUpdate();
        }
        

        

    }
}