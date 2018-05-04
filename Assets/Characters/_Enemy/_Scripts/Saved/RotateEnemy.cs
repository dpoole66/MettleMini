using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GoogleARCore;
using UnityStandardAssets.CrossPlatformInput;

//This movement controller is for scaled NavMesh characters and stages
[RequireComponent(typeof(NavMeshAgent))]

public class RotateEnemy : MonoBehaviour {

    NavMeshAgent m_ThisAgent;
    Animator m_ThisAnim;
    public Transform m_Player;
    public float m_ThisRotation;           
    RaycastHit hitInfo = new RaycastHit();
    public float m_EnemyRange;
    public float m_EnemyRotation;
    public float m_TurnSpeed = 2.0f;
    public float m_ForwardSpeed = 2.0f;

    private void Awake() {

        m_ThisAgent = GetComponent<NavMeshAgent>();
        m_ThisAnim = GetComponent<Animator>();
        m_Player = GetComponent<Transform>();

    }

	
	// Update is called once per frame
	void Update () {

        this.m_ThisRotation += Input.GetAxis("Horizontal");
        transform.eulerAngles = new Vector3(0, m_ThisRotation, 0);
 
        //Debug.Log(m_Enemy.transform.position);
        m_EnemyRange = Vector3.Distance(this.transform.position, m_Player.position);
        m_EnemyRotation = m_ThisAgent.transform.rotation.y;
        Vector3 lookRotation = m_Player.transform.position - this.transform.position;

        //transform.LookAt(m_Enemy.transform.position);
        Quaternion rotation = Quaternion.LookRotation(lookRotation);

        //check rotation
        float angle = Quaternion.Angle(this.transform.rotation, m_Player.transform.rotation);
        var deltaAngle = angle - angle * Time.deltaTime;                  
        //Debug.Log(m_EnemyRotation);


    }
}
