using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LineSightLtd : MonoBehaviour {

    private Animator m_Anim;
    private NavMeshAgent m_Agent;
    private Transform target;
    public float RotationSpeed;
    public float PursueDistance = 0.3f;                    
    private float Range;

    private void Awake() {

        m_Anim = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();

    }

    private void OnTriggerStay(Collider other) {

        target = other.transform;

        Quaternion DestRot = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);

        //Update rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, DestRot, RotationSpeed * Time.deltaTime);

        //Update player location and actions to take
        Range = Vector3.Distance(target.transform.position, this.transform.position);
        if (Range <= PursueDistance){

            m_Agent.isStopped = false;
            m_Agent.destination = target.transform.position;
            m_Anim.SetTrigger("Chaseing");

        }

    }

}
