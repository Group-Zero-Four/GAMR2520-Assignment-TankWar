using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZeroFour.RuleBased
{
    public class Rule
    {
        public string conditionA, conditionB;
        public string[] conditions;
        public string result;
        public Predicate compare;
        public enum Predicate { And, Or, nAnd, OnlyFirst };
        public Rule(string conditionA, string conditionB, string result, Predicate compare)
        {
            this.conditionA = conditionA;
            this.conditionB = conditionB;
            this.result = result;
            this.compare = compare;
        }
        public Rule(string[] conditions, string result, Predicate compare)
        {
            this.conditions = conditions;
            this.result= result;
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
                case Predicate.OnlyFirst:
                    facts[result] = condABool & !condBBool;
                        break;
                default:
                    return null;
            }
            return facts;
        }
        public Dictionary<string, bool> CheckRuleArray(Dictionary<string, bool> facts)
        {
            bool meetsPredicate = false;
            int evaluated = -1;
            foreach (var item in facts)
            {
                if(evaluated == -1)
                {
                    switch (compare)
                    {
                        case Predicate.And:
                            meetsPredicate = item.Value;
                            break;
                        case Predicate.Or:
                            meetsPredicate = item.Value;
                            break;
                        case Predicate.nAnd:
                            meetsPredicate = !item.Value;
                            break;
                        case Predicate.OnlyFirst:
                            meetsPredicate = item.Value;
                            break;
                        default:
                            return null;
                    }
                    continue;
                }

                switch (compare)
                {
                    case Predicate.And:
                        meetsPredicate &= item.Value;
                        break;
                    case Predicate.Or:
                        meetsPredicate |= item.Value;
                        break;
                    case Predicate.nAnd:
                        meetsPredicate &= !item.Value;
                        break;
                    case Predicate.OnlyFirst:
                        meetsPredicate &= !item.Value;
                        break;
                    default:
                        return null;
                }
            }
            facts[result] = meetsPredicate;
            return facts;
        }
    }
}