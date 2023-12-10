using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ZeroFour.RuleBased
{

    public class SmartAbbleTank_RBS_2 : SmartAbbleTank_Base
    {
        /// <summary>
        /// Determinant Fact - determined by conditions in the world.
        /// </summary>
        public const string ENEMYSEEN = "Enemy Seen", BASESEEN = "Base Seen", CONSUMABLESEEN = "Consumable Seen",
            LOWHEALTH = "Low Health", LOWFUEL = "Low Fuel", LOWAMMO = "Low Ammo";
        /// <summary>
        /// Asserted Fact - determined by other facts.
        /// </summary>
        public const string DANGER = "Danger", ATTACKINGENEMY = "Attacking Enemy", ATTACKINGBASE = "Attacking Base",
            GETTINGCONSUMABLE = "Getting Consumable", PATROLLING = "Patrolling";
        AdvancedState currentState;
        public string stateName;
        public override void AIOnCollisionEnter(Collision collision)
        {
            base.AIOnCollisionEnter(collision);
        }
        public override void AITankStart()
        {
            base.AITankStart();
            InitFacts();
            InitRules();
            InitStateMachine();
        }
        public override void AITankUpdate()
        {
            base.AITankUpdate();
            closestEnemy = GetClosestEnemy;
            closestConsumable = GetClosestConsumable;
            closestBase = GetClosestBase;
            EvaluateRules();
            stateName = currentState?.GetType().Name;
            currentState?.StateUpdate();
        }

        void InitFacts()
        {
            facts.Add(ENEMYSEEN, false);
            facts.Add(BASESEEN, false);
            facts.Add(CONSUMABLESEEN, false);

            facts.Add(LOWHEALTH, false);
            facts.Add(LOWFUEL, false);
            facts.Add(LOWAMMO, false);
            facts.Add(DANGER, false);
            facts.Add(PATROLLING, false);
        }
        void InitRules()
        {
            ruleTypes.AddRule(new(ENEMYSEEN, DANGER, typeof(AS_AttackEnemy), RuleType.Predicate.OnlyFirst));
        }
        void InitStateMachine()
        {
            stateDictionary.Add(typeof(AS_AttackEnemy), new AS_AttackEnemy(this));
        }
        void EvaluateRules()
        {
            facts[ENEMYSEEN] = closestEnemy;
            facts[BASESEEN] = closestBase;
            facts[CONSUMABLESEEN] = closestConsumable;
            string rulePrint = $" D Enemy Seen - {facts[ENEMYSEEN]}, D Base Seen - {facts[BASESEEN]}, " +
    $"D Consumable Seen - {facts[CONSUMABLESEEN]}";
            print(rulePrint);


            foreach (var item in ruleTypes.GetRules)
            {
                Type nextState = item.TryCheckRule(facts);
                if (nextState != null)
                {
                    TrySwitchState(nextState);
                    return;
                }
            }
        }
        void TrySwitchState(Type nextStateType)
        {
            if(currentState != null)
            {
                if(currentState.GetType() != nextStateType)
                {
                    currentState?.StateExit();
                    currentState = stateDictionary[nextStateType];
                    currentState?.StateEnter();
                }
            }
            else
            {
                currentState = stateDictionary[nextStateType];
                currentState?.StateEnter();

            }
        }
    }
}