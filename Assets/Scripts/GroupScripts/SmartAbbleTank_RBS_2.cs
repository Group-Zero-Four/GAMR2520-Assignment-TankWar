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
            LOWHEALTH = "Low Health", LOWFUEL = "Low Fuel", LOWAMMO = "Low Ammo", BASELOST = "Base Lost";
        /// <summary>
        /// Asserted Fact - determined by other facts.
        /// </summary>
        public const string DANGER = "Danger", ATTACKINGENEMY = "Attacking Enemy", ATTACKINGBASE = "Attacking Base",
            GETTINGCONSUMABLE = "Getting Consumable", PATROLLING = "Patrolling";
        AdvancedState currentState;
        public string stateName;
        public float stateCheckTimer = 0.5f;
        float currStateCheckTimer = 0.5f;
        public float baseLostAggressionTime = 30;
        public override void AIOnCollisionEnter(Collision collision)
        {
            base.AIOnCollisionEnter(collision);
            currentState?.CollisionCallback(collision);
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
            if (currStateCheckTimer <= 0.5f)
            {
                EvaluateRules();
                stateName = currentState?.GetType().Name;
                currStateCheckTimer = stateCheckTimer;
            }
            else
            {
                currStateCheckTimer -= Time.deltaTime;
            }
            if (facts[BASELOST])
            {
                baseLostAggressionTime -= Time.deltaTime;
            }
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

            facts.Add(BASELOST, false);
            facts.Add(DANGER, false);
            facts.Add(PATROLLING, false);
        }
        void InitRules()
        {
            ruleTypes.AddRule(new(CONSUMABLESEEN, DANGER, typeof(AS_Collect), RuleType.Predicate.And));
            ruleTypes.AddRule(new(ENEMYSEEN, DANGER, typeof(AS_AttackEnemy), RuleType.Predicate.OnlyFirst));
            ruleTypes.AddRule(new(BASELOST, DANGER, typeof(AS_BaseDefence), RuleType.Predicate.OnlyFirst));
            ruleTypes.AddRule(new(BASESEEN, DANGER, typeof(AS_AttackBase), RuleType.Predicate.OnlyFirst));
            ruleTypes.AddRule(new(CONSUMABLESEEN, ENEMYSEEN, BASESEEN, typeof(AS_Collect), RuleType.Predicate.OnlyFirst));
            ruleTypes.AddRule(new(ENEMYSEEN, CONSUMABLESEEN, BASESEEN, typeof(AS_Patrol), RuleType.Predicate.nAnd));
        }
        void InitStateMachine()
        {
            stateDictionary.Add(typeof(AS_AttackEnemy), new AS_AttackEnemy(this));
            stateDictionary.Add(typeof(AS_AttackBase), new AS_AttackBase(this));
            stateDictionary.Add(typeof(AS_Patrol), new AS_Patrol(this));
            stateDictionary.Add(typeof(AS_Collect), new AS_Collect(this));
            stateDictionary.Add(typeof(AS_BaseDefence), new AS_BaseDefence(this));
        }
        void EvaluateRules()
        {
            facts[ENEMYSEEN] = closestEnemy != null;
            facts[BASESEEN] = closestBase != null;
            facts[CONSUMABLESEEN] = closestConsumable != null;

            facts[BASELOST] = GetMyBases.Count < 2 && baseLostAggressionTime > 0;

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