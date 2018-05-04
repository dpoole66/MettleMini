using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GoogleARCore;
using UnityStandardAssets.CrossPlatformInput;

public class MoveEnemy : MonoBehaviour {

    NavMeshAgent m_Agent;              
    Animator m_Anim;
    public Transform m_ChaseTarget;
    public Rigidbody m_PlayerRB;

    public float RotateSpeed = 90f;
    public float MaxSpeed = 50f;
    public float JumpForce = 50f;
    public float GroundedDist = 0.1f;
    public bool IsGrounded = false;
    private Vector3 Velocity = Vector3.zero;
    public LayerMask GroundLayer;

    public float m_MoveSpeed = 1.0f;

    private void Awake() {

        m_Agent = GetComponent<NavMeshAgent>();                
        m_ChaseTarget = GetComponent<Transform>();
        m_PlayerRB = GetComponent<Rigidbody>();
        m_Anim = GetComponent<Animator>();

    }


    // Update is called once per frame
    void FixedUpdate() {

        float Horz = m_PlayerRB.velocity.x;
        float Vert = m_PlayerRB.velocity.z;

        m_Anim.SetFloat("Horz", Horz);
        m_Anim.SetFloat("Horz", Vert);

        m_ChaseTarget.rotation *= Quaternion.Euler(new Vector3(0, RotateSpeed * Time.deltaTime * Horz, 0));

        //Calculate Move Dir
        Velocity.z = Vert * MaxSpeed;

        //Are we grounded?
        IsGrounded = (DistanceToGround() < GroundedDist) ? true : false;

        //Should we jump?
        if (CrossPlatformInputManager.GetAxisRaw("Jump") != 0 && IsGrounded)
            Velocity.y = JumpForce;

        //Apply gravity
        Velocity.y += Physics.gravity.y * Time.deltaTime;

        //Move
        m_Agent.destination = m_ChaseTarget.transform.position;
        Debug.Log("m_ChaseTarget");
        
    }

    public float DistanceToGround() {
        RaycastHit hit;
        float distanceToGround = 0;
        if (Physics.Raycast(m_ChaseTarget.position, -Vector3.up, out hit, Mathf.Infinity, GroundLayer))
            distanceToGround = hit.distance;
        return distanceToGround;
    }
}
