using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseFSM : StateMachineBehaviour {

    public GameObject Enemy;
    public GameObject Player;     
    public float speed = 0.08f;
    public float rotSpeed = 0.05f;
    public float accuracy = 0.001f;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        Enemy = animator.gameObject;         
        //Player = Enemy.GetComponent<MettleAI>().GetPlayer();        
        
    }

}
