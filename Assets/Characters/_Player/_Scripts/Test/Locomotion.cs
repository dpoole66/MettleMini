using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GoogleARCore;
using UnityStandardAssets.CrossPlatformInput;

// this controls the animator using input from "Movement".
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]

public class Locomotion : MonoBehaviour {

    private Animator m_Anim;
    private NavMeshAgent m_Agent;
    Vector2 smoothDeltaPos = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    void Start() {

        m_Anim = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
  
        // don't update the agent until requested
        m_Agent.updatePosition = false;
        //m_Agent.updateRotation = false;

    }

    void Update() {

        Vector3 worldDeltaPos = m_Agent.nextPosition - transform.position;

        // map worldDeltaPos to local space and create a new "deltaPositon" vector2 
        float dPosX = Vector3.Dot(transform.right, worldDeltaPos);
        float dPosY = Vector3.Dot(transform.forward, worldDeltaPos);
        Vector2 deltaPosition = new Vector2(dPosX, dPosY);

        // low pass to smooth motion using a var "smooth" (minimum time/time) and a lerp
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPos = Vector2.Lerp(smoothDeltaPos, deltaPosition, smooth);

        // input for movement destination
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hitInfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)) {

                m_Agent.stoppingDistance = 0.05f;
                m_Agent.destination = hitInfo.point;

            }

        }
  
        // update velocity with time
        if (Time.deltaTime > 1e-5f) {

            velocity = smoothDeltaPos / Time.deltaTime;

        }

        // talk to the animator  
        m_Anim.SetFloat("Turn", velocity.x);
        m_Anim.SetFloat("Forward", velocity.y);

        if (worldDeltaPos.magnitude > m_Agent.radius) {

            transform.position = m_Agent.nextPosition - 0.3f * worldDeltaPos;

        }

        // tell the animator the agent is moving
        if (m_Agent.nextPosition == m_Agent.destination) {

            m_Anim.SetBool("Idle", true);
            m_Anim.SetBool("Walking", false);

        } else {

            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Walking", true);

        }

    }

    private void OnAnimatorMove() {

        Vector3 position = m_Anim.rootPosition;
        position.y = m_Agent.nextPosition.y;
        transform.position = position;

    }

}
