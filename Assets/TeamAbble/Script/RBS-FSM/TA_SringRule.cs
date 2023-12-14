using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TeamAbble.RuleBased
{
    public class TA_StringRule
    {
        public string conditionA, conditionB, conditionC, conditionD;
        public string result;
        int conditionCount;
        public Predicate compare;
        public enum Predicate { And, Or, nAnd, OnlyFirst, nOr };
        public TA_StringRule(string conditionA, string conditionB, string result, Predicate compare)
        {
            this.conditionA = conditionA;
            this.conditionB = conditionB;
            this.result = result;
            this.compare = compare;
            conditionCount = 2;
        }
        public TA_StringRule(string conditionA, string conditionB, string conditionC, string result, Predicate compare)
        {
            this.conditionA = conditionA;
            this.conditionB = conditionB;
            this.conditionC = conditionC;
            this.result = result;
            this.compare = compare;
            conditionCount = 3;
        }

        public TA_StringRule(string conditionA, string conditionB, string conditionC, string conditionD, string result, Predicate compare)
        {
            this.conditionA = conditionA;
            this.conditionB = conditionB;
            this.conditionC = conditionC;
            this.conditionD = conditionD;
            this.result = result;
            this.compare = compare;
            conditionCount = 4;
        }

        public Dictionary<string, bool> TryCheckRule(Dictionary<string, bool> facts)
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
        public Dictionary<string, bool> CheckRuleTwo(Dictionary<string, bool> facts)
        {
            Debug.Log($"checking {nameof(facts)}");
            bool condABool = facts[conditionA];
            bool condBBool = facts[conditionB];
            switch (compare)
            {
                case Predicate.And:
                    facts[result] = condABool && condBBool;
                    break;
                case Predicate.Or:
                    facts[result] = condABool || condBBool;
                    break;
                case Predicate.nAnd:
                    facts[result] = !condABool && !condBBool;
                    break;
                case Predicate.OnlyFirst:
                    facts[result] = condABool && !condBBool;
                        break;
                case Predicate.nOr:
                    facts[result] = !(condABool || condBBool);
                    break;
                default:
                    return null;
            }
            return facts;
        }
        public Dictionary<string, bool> CheckRuleThree(Dictionary<string, bool> facts)
        {
            Debug.Log($"checking {nameof(facts)}");
            bool condABool = facts[conditionA];
            bool condBBool = facts[conditionB];
            bool condCBool = facts[conditionC];
            switch (compare)
            {
                case Predicate.And:
                    facts[result] = condABool && condBBool && condCBool;
                    break;
                case Predicate.Or:
                    facts[result] = condABool || condBBool || condCBool;
                    break;
                case Predicate.nAnd:
                    facts[result] = !condABool && !condBBool && !condCBool;
                    break;
                case Predicate.OnlyFirst:
                    facts[result] = condABool && !condBBool && !condCBool;
                    break;
                case Predicate.nOr:
                    facts[result] = !(condABool || condBBool || condCBool);
                    break;
                default:
                    return null;
            }
            return facts;
        }
        public Dictionary<string, bool> CheckRuleFour(Dictionary<string, bool> facts)
        {
            Debug.Log($"checking {nameof(facts)}");
            bool condABool = facts[conditionA];
            bool condBBool = facts[conditionB];
            bool condCBool = facts[conditionC];
            bool condDbool = facts[conditionD];
            switch (compare)
            {
                case Predicate.And:
                    facts[result] = condABool && condBBool && condBBool && condCBool;
                    break;
                case Predicate.Or:
                    facts[result] = condABool || condBBool || condCBool || condDbool;
                    break;
                case Predicate.nAnd:
                    facts[result] = !condABool && !condBBool && !condBBool && !condCBool;
                    break;
                case Predicate.OnlyFirst:
                    facts[result] = condABool && !condBBool && !condBBool && !condCBool;
                    break;
                case Predicate.nOr:
                    facts[result] = !(condABool || condBBool || condCBool || condDbool);
                    break;
                default:
                    return null;
            }
            return facts;
        }

    }
}