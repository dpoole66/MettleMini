using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GoogleARCore;
using UnityEngine.AI;

//This movement controller is for scaled NavMesh characters and stages
[RequireComponent(typeof(NavMeshAgent))]

public class LocoInput : MonoBehaviour {

    public NavMeshAgent m_Agent;               
    public Transform m_Enemy;
    public float m_Range;
    RaycastHit hitInfo = new RaycastHit();     

    // input to Locomotion
    Locomotion m_Loco;

    private void Awake() {

        m_Agent = GetComponent<NavMeshAgent>();
        m_Enemy = GetComponent<Transform>();

    }


    // Update is called once per frame
    void Update() {

        m_Range = Vector3.Distance(m_Agent.transform.position, m_Enemy.transform.position);
        Debug.Log(m_Range);

        // input for movement destination
        if (Input.GetMouseButtonDown(0)) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)) {

                m_Agent.stoppingDistance = 0.05f;
                m_Agent.destination = hitInfo.point;

            }

        }

    }
}
