using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GoogleARCore;
using UnityStandardAssets.CrossPlatformInput;

public class MovePlayer : MonoBehaviour {
    NavMeshAgent m_Agent;
    Animator m_Anim;

    private void Awake() {

        m_Agent = GetComponent<NavMeshAgent>();
        m_Anim = GetComponent<Animator>();

    }

    private void Start() {

        m_Agent.isStopped = true;

    }


    private void Update() {
        
         // input for movement destination
         if (Input.GetMouseButtonDown(0) && !m_Agent.pathPending) {
             RaycastHit hitInfo = new RaycastHit();
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)) {

                //navMeshAgent
                 m_Agent.destination = hitInfo.point;
                 m_Agent.isStopped = false;
                 m_Agent.updatePosition = true;
                 m_Agent.updateRotation = true;
                 m_Agent.destination = hitInfo.point;
                 //animation
                 m_Anim.SetBool("Idle", false);
                 m_Anim.SetBool("Walking", true);

             }

         } else if (m_Agent.velocity.sqrMagnitude <= 0.03f) {

             m_Agent.isStopped = true;
             m_Anim.SetBool("Walking", false);
             m_Anim.SetBool("Idle", true);

         }
      
    }


}
