using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamAbble.RuleBased
{
    public class TA_StringRules
    {
        public void AddRule(TA_StringRule rule)
        {
            GetRules.Add(rule);
        }
        public List<TA_StringRule> GetRules { get; } = new List<TA_StringRule>();
    }
}