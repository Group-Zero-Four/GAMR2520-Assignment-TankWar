using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.RuleBased
{
    public class RuleTypes
    {
        public void AddRule(RuleType rule)
        {
            GetRules.Add(rule);
        }
        public List<RuleType> GetRules { get; } = new List<RuleType>();
    }
}