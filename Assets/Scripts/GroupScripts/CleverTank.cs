using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour
{
    public class CleverTank : AITank
    {
        //Grabbed from Dumb Tank
        //store ALL currently visible 
        public Dictionary<GameObject, float> enemyTanksFound = new Dictionary<GameObject, float>();
        public Dictionary<GameObject, float> consumablesFound = new Dictionary<GameObject, float>();
        public Dictionary<GameObject, float> enemyBasesFound = new Dictionary<GameObject, float>();

        public enum State
        {
            search,
            chase,
            attack,
            retreat
        }
        public State currentState;

        public override void AITankStart()
        {
            currentState = State.search;
        }
        public override void AITankUpdate()
        {
            switch (currentState)
            {
                case State.search:
                    break;
                case State.chase:
                    break;
                case State.attack:
                    break;
                case State.retreat:
                    break;
                default:
                    break;
            }
        }
        public override void AIOnCollisionEnter(Collision collision)
        {

        }
        
    }
}