using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.RuleBased
{
    public class Rule
    {
        public string conditionA, conditionB;
        public string result;
        public Predicate compare;
        public enum Predicate { And, Or, nAnd };
        public Rule(string conditionA, string conditionB, string result, Predicate compare)
        {
            this.conditionA = conditionA;
            this.conditionB = conditionB;
            this.result = result;
            this.compare = compare;
        }
        public Dictionary<string, bool> CheckRule(Dictionary<string, bool> facts)
        {
            bool condABool = facts[conditionA];
            bool condBBool = facts[conditionB];
            switch (compare)
            {
                case Predicate.And:
                    facts[result] = condABool & condBBool;
                    break;
                case Predicate.Or:
                    facts[result] = condABool | condBBool;
                    break;
                case Predicate.nAnd:
                    facts[result] = !condABool & !condBBool;
                    break;
                default:
                    return null;
            }
            return facts;
        }
    }
}