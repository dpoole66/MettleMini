using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleMove : MonoBehaviour {

    NavMeshAgent m_Agent;	

	void Start () {

        m_Agent = GetComponent<NavMeshAgent>();
		
	}
	
	void Update () {

        //Touch touch;

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){

            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if(Physics.Raycast(ray)){

                m_Agent.destination = transform.position;

            }

        }
		
	}
}
