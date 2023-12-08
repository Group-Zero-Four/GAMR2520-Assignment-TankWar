using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.RuleBased
{
    public class Rules
    {
        public void AddRule(Rule rule)
        {
            GetRules.Add(rule);
        }
        public List<Rule> GetRules { get; } = new List<Rule>();
    }
}