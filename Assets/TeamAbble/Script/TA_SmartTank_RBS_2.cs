using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeamAbble.RuleBased
{

    public class TA_SmartTank_RBS_2 : TA_SmartTankBase_RBS
    {
        /// <summary>
        /// Determinant Fact - determined by conditions in the world.
        /// </summary>
        public const string ENEMYSEEN = "Enemy Seen", BASESEEN = "Base Seen", CONSUMABLESEEN = "Consumable Seen",
            LOWHEALTH = "Low Health", LOWFUEL = "Low Fuel", LOWAMMO = "Low Ammo", BASELOST = "Base Lost", ENEMYTOOFAR = "Enemy Too Far";
        /// <summary>
        /// Asserted Fact - determined by other facts.
        /// </summary>
        public const string DANGER = "Danger", ATTACKINGENEMY = "Attacking Enemy", ATTACKINGBASE = "Attacking Base",
            GETTINGCONSUMABLE = "Getting Consumable", PATROLLING = "Patrolling";
        TA_AdvancedState currentState;
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

            closestEnemy = null;
            closestConsumable = null;
            closestBase = null;
        }

        void InitFacts()
        {
            facts.Add(ENEMYSEEN, false);
            facts.Add(BASESEEN, false);
            facts.Add(CONSUMABLESEEN, false);
            facts.Add(ENEMYTOOFAR, false);

            facts.Add(LOWHEALTH, false);
            facts.Add(LOWFUEL, false);
            facts.Add(LOWAMMO, false);

            facts.Add(BASELOST, false);
            facts.Add(DANGER, false);
            facts.Add(PATROLLING, false);
        }
        void InitRules()
        {
            ruleTypes.AddRule(new(CONSUMABLESEEN, DANGER, typeof(TA_AdvCollect), TA_RuleType.Predicate.And));
            ruleTypes.AddRule(new(ENEMYSEEN, DANGER, ENEMYTOOFAR, typeof(TA_AdvAttackEnemy), TA_RuleType.Predicate.OnlyFirst));
            ruleTypes.AddRule(new(ENEMYSEEN, ENEMYTOOFAR, typeof(TA_AdvChaseState), TA_RuleType.Predicate.And));
            ruleTypes.AddRule(new(BASELOST, DANGER, typeof(TA_AdvBaseDefence), TA_RuleType.Predicate.OnlyFirst));
            ruleTypes.AddRule(new(BASESEEN, DANGER, typeof(TA_AdvAttackBase), TA_RuleType.Predicate.OnlyFirst));
            ruleTypes.AddRule(new(CONSUMABLESEEN, ENEMYSEEN, BASESEEN, typeof(TA_AdvCollect), TA_RuleType.Predicate.OnlyFirst));
            ruleTypes.AddRule(new(ENEMYSEEN, CONSUMABLESEEN, BASESEEN, typeof(TA_AdvPatrol), TA_RuleType.Predicate.nAnd));
        }
        void InitStateMachine()
        {
            stateDictionary.Add(typeof(TA_AdvChaseState), new TA_AdvChaseState(this));
            stateDictionary.Add(typeof(TA_AdvAttackEnemy), new TA_AdvAttackEnemy(this));
            stateDictionary.Add(typeof(TA_AdvAttackBase), new TA_AdvAttackBase(this));
            stateDictionary.Add(typeof(TA_AdvPatrol), new TA_AdvPatrol(this));
            stateDictionary.Add(typeof(TA_AdvCollect), new TA_AdvCollect(this));
            stateDictionary.Add(typeof(TA_AdvBaseDefence), new TA_AdvBaseDefence(this));
        }
        void EvaluateRules()
        {
            facts[ENEMYSEEN] = closestEnemy != null;
            facts[BASESEEN] = closestBase != null;
            facts[CONSUMABLESEEN] = closestConsumable != null;
            facts[ENEMYTOOFAR] = closestEnemy != null && DistanceFromTank(closestEnemy) > 35;

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