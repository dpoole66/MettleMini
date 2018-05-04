using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : EnemyBaseFSM {

    GameObject[] waypoints;
    int currentWP;   

    private void Awake() {

        waypoints = GameObject.FindGameObjectsWithTag("waypoint");

    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
          base.OnStateEnter(animator, stateInfo, layerIndex);
          currentWP = 0;
	
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (waypoints.Length == 0) {
            Debug.Log("No waypoints");
            return;
        }
        if(Vector3.Distance(waypoints[currentWP].transform.position, Enemy.transform.position) < accuracy){

            currentWP++;
            if(currentWP >= waypoints.Length){
                currentWP = 0;
            }
        }

        //turn to currentWP
        var direction = waypoints[currentWP].transform.position - Player.transform.position;
        Enemy.transform.rotation = Quaternion.Slerp(Enemy.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
        //move to currentWP
        Enemy.transform.Translate(0, 0, Time.deltaTime * speed);

        Debug.Log(currentWP);
	
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

       // animator.SetBool("Move", true);

	}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
