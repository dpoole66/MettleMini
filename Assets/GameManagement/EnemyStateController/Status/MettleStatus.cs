using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 [CreateAssetMenu(menuName = "MettleAI/MettleStatus")]

    public class MettleStatus : ScriptableObject {

        public float moveSpeed = 1;
        public float rotateSpeed = 3.0f;
        public float lookRange = 15f;
        public float lookSphereCastRadius = 1f;

        public float advanceRange = 5.0f;
        public float alertRange = 5.0f;
        public float chaseRange = 3.0f;
        public float attackRange = 1f;

        public float attackRate = 1f;
        public float attackForce = 15f;
        public int attackDamage = 50;

        public float searchDuration = 4f;
        public float searchingTurnSpeed = 120f;

        public bool enemyAttacking, enemyDefending;

    }
