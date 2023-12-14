using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TeamAbble.RuleBased
{
    public class TA_RuleType
    {
        public string conditionA, conditionB, conditionC, conditionD;
        public Type result;
        int conditionCount;
        public Predicate compare;
        public enum Predicate { And, Or, nAnd, OnlyFirst, nOr };
        public TA_RuleType(string conditionA, string conditionB, Type result, Predicate compare)
        {
            this.conditionA = conditionA;
            this.conditionB = conditionB;
            this.result = result;
            this.compare = compare;
            conditionCount = 2;
        }
        public TA_RuleType(string conditionA, string conditionB, string conditionC, Type result, Predicate compare)
        {
            this.conditionA = conditionA;
            this.conditionB = conditionB;
            this.conditionC = conditionC;
            this.result = result;
            this.compare = compare;
            conditionCount = 3;
        }

        public TA_RuleType(string conditionA, string conditionB, string conditionC, string conditionD, Type result, Predicate compare)
        {
            this.conditionA = conditionA;
            this.conditionB = conditionB;
            this.conditionC = conditionC;
            this.conditionD = conditionD;
            this.result = result;
            this.compare = compare;
            conditionCount = 4;
        }

        public Type TryCheckRule(Dictionary<string, bool> facts)
        {
            switch (conditionCount)
            {
                case 2:
                    return CheckRuleTwo(facts);
                case 3:
                    return CheckRuleThree(facts);
                case 4:
                    return CheckRuleFour(facts);
                default:
                    return null;
            }
        }
        public Type CheckRuleTwo(Dictionary<string, bool> facts)
        {
            bool condABool = facts[conditionA];
            bool condBBool = facts[conditionB];
            switch (compare)
            {
                case Predicate.And:
                    return condABool && condBBool ? result : null;
                case Predicate.Or:
                    return condABool || condBBool ? result : null;
                case Predicate.nAnd:
                    return !condABool && !condBBool ? result : null;
                case Predicate.OnlyFirst:
                    return condABool && !condBBool ? result : null;
                case Predicate.nOr:
                    return !(condABool || condBBool) ? result : null;
                default:
                    return null;
            }
        }
        public Type CheckRuleThree(Dictionary<string, bool> facts)
        {
            bool condABool = facts[conditionA];
            bool condBBool = facts[conditionB];
            bool condCBool = facts[conditionC];
            switch (compare)
            {
                case Predicate.And:
                    return condABool && condBBool && condCBool ? result : null;
                case Predicate.Or:
                    return condABool || condBBool || condCBool ? result : null;
                case Predicate.nAnd:
                    return !condABool && !condBBool && !condCBool ? result : null;
                case Predicate.OnlyFirst:
                    return condABool && !condBBool && !condCBool ? result : null;
                case Predicate.nOr:
                    return !(condABool || condBBool || condCBool) ? result : null;
                default:
                    return null;
            }
        }
        public Type CheckRuleFour(Dictionary<string, bool> facts)
        {
            bool condABool = facts[conditionA];
            bool condBBool = facts[conditionB];
            bool condCBool = facts[conditionC];
            bool condDbool = facts[conditionD];
            switch (compare)
            {
                case Predicate.And:
                    return condABool && condBBool && condBBool && condCBool ? result : null;
                case Predicate.Or:
                    return condABool || condBBool || condCBool || condDbool ? result : null;
                case Predicate.nAnd:
                    return !condABool && !condBBool && !condBBool && !condCBool ? result : null;
                case Predicate.OnlyFirst:
                    return condABool && !condBBool && !condBBool && !condCBool ? result : null;
                case Predicate.nOr:
                    return !(condABool || condBBool || condCBool || condDbool) ? result : null;
                default:
                    return null;
            }
        }
    }
}