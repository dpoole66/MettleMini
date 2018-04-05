using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GoogleARCore;
using UnityEngine.AI;

//This movement controller is for scaled NavMesh characters and stages
[RequireComponent(typeof(NavMeshAgent))]

public class Brain : MonoBehaviour {

    NavMeshAgent m_Agent;
    public GameObject m_Enemy;
    RaycastHit hitInfo = new RaycastHit();
    public float m_EnemyRange;
    public float m_EnemyRotation;


    private void Awake() {

        m_Agent = GetComponent<NavMeshAgent>();                  
        //m_Enemy = GameObject.FindGameObjectWithTag("Player");

    }


    // Update is called once per frame
    void Update() {

        m_EnemyRange = Vector3.Distance(this.transform.position, m_Enemy.transform.position);
        m_EnemyRotation = m_Enemy.transform.rotation.y;

    }

}
