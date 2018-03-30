using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToDest : MonoBehaviour {

    public Transform goal;

	// Use this for initialization
	void Start () {

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;

	}
	
}
