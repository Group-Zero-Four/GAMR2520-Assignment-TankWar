using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace TeamAbble.RuleBased
{
    /// <summary>
    /// A Rule- based state machine implementation of the Tank AI.
    /// </summary>
    [Obsolete("Please use TA_SmartTank_RBS_2 instead.")]
    public class TA_SmartTank_RBS_1 : TA_SmartTankBase_RBS
    {
        #region Fact Strings

        public const string LOWHEALTH = "Low Health", LOWAMMO = "Low Ammo", LOWFUEL = "Low Fuel", DEFENDING = "Defending",
            ENEMYSEEN = "Enemy Seen", BASESEEN = "Base Seen", BASELOST = "Base Lost", RETREATING = "Retreating", COLLECTABLESEEN = "Collectable Seen";
        public const string ATTACKINGBASE = "Attacking Base", ATTACKINGENEMY = "Attacking Enemy", COLLECTING = "Collecting",
            HEALTHFULL = "Health Full", AMMOFULL = "Ammo Full", FUELFULL = "Fuel Full", NEEDSCONSUMABLE = "Needs Consumable", PATROLLING = "Patrolling";
        #endregion Fact Strings
        public Dictionary<string, Type> factStatePairs = new();

        TA_AdvancedState currentState;


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
            rules.AddRule(new(LOWHEALTH, LOWAMMO, LOWFUEL, RETREATING, TA_StringRule.Predicate.Or));
            //Attacking enemy rule - if enemy is seen and not retreating
            rules.AddRule(new(ENEMYSEEN, RETREATING, ATTACKINGENEMY, TA_StringRule.Predicate.OnlyFirst));
            //Attacking base rule - if base is seen and not reatreating
            rules.AddRule(new(BASESEEN, RETREATING, ATTACKINGBASE, TA_StringRule.Predicate.OnlyFirst));
            //If health, ammo or fuel are NOT full, a consumable is needed
            rules.AddRule(new(HEALTHFULL, AMMOFULL, FUELFULL, NEEDSCONSUMABLE, TA_StringRule.Predicate.nAnd));
            //If an enemy or base is NOT visible, and a consumable is needed, path to consumables
            rules.AddRule(new(COLLECTABLESEEN, NEEDSCONSUMABLE, COLLECTING, TA_StringRule.Predicate.And));
            //If we have less than two bases and are not in danger, return to the bases to defend them.
            rules.AddRule(new(BASELOST, RETREATING, DEFENDING, TA_StringRule.Predicate.OnlyFirst));

            rules.AddRule(new(ENEMYSEEN, BASESEEN, COLLECTING, PATROLLING, TA_StringRule.Predicate.nAnd));
        }
        void InitialiseStateMachine()
        {
            stateDictionary.Add(typeof(TA_AdvRush), new TA_AdvRush(this));
            stateDictionary.Add(typeof(TA_AdvPatrol), new TA_AdvPatrol(this));
            stateDictionary.Add(typeof(TA_AdvAttackBase), new TA_AdvAttackBase(this));
            stateDictionary.Add(typeof(TA_AdvAttackEnemy), new TA_AdvAttackEnemy(this));
            stateDictionary.Add(typeof(TA_AdvCollect), new TA_AdvCollect(this));

            factStatePairs.Add(PATROLLING, typeof(TA_AdvPatrol));
            factStatePairs.Add(ATTACKINGENEMY, typeof(TA_AdvAttackEnemy));
            factStatePairs.Add(ATTACKINGBASE, typeof(TA_AdvAttackBase));
            factStatePairs.Add(COLLECTING, typeof(TA_AdvCollect));

            SwitchState(typeof(TA_AdvRush));
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