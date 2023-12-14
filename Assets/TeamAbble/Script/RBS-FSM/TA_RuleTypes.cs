using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamAbble.RuleBased
{
    public class TA_RuleTypes
    {
        public void AddRule(TA_RuleType rule)
        {
            GetRules.Add(rule);
        }
        public List<TA_RuleType> GetRules { get; } = new List<TA_RuleType>();
    }
}