using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GoogleARCore;
using UnityEngine.AI;

//This movement controller is for scaled NavMesh characters and stages
[RequireComponent(typeof(NavMeshAgent))]

public class Movement : MonoBehaviour {

    public NavMeshAgent m_Agent;
    RaycastHit hitInfo = new RaycastHit();

    private void Awake() {

        m_Agent = GetComponent<NavMeshAgent>();

    }


    // Update is called once per frame
    void Update () {

        if(Input.GetMouseButtonDown(0)){

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)){

                m_Agent.stoppingDistance = 0.05f;
                m_Agent.destination = hitInfo.point;

            }

        }
		
	}
}
