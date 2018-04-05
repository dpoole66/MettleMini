using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GoogleARCore;

[RequireComponent(typeof(Animator))]
public class LookAtEnemy : MonoBehaviour {

    Brain m_Brain;
    public Transform m_Head;
    public Transform m_Enemy;
    public Vector3 lookAtTargetPosition;
    public float lookAtCoolTime = 0.2f;
    public float lookAtHeatTime = 0.2f;
    public bool isLooking;

    private Vector3 lookAtPosition;
    private Animator m_Animator;
    private float lookAtWeight = 0.0f;

    private void Awake() {

        m_Brain = GetComponent<Brain>();
        m_Animator = GetComponent<Animator>();


    }

    void Start() {
        if (!m_Head) {
            Debug.LogError("No head transform - LookAt disabled");
            enabled = false;
            return;
        }
        
        lookAtTargetPosition = m_Head.position + transform.forward;
        lookAtPosition = lookAtTargetPosition;

        isLooking = false;
        m_Animator.SetBool("isLooking", false);

    }

    void Update() {
        if ((Vector3.Distance(m_Enemy.position, m_Head.position) < 7.0f)) {

            if (Input.GetButton("Fire")) {
                isLooking = true;
                m_Animator.SetBool("isLooking", true);
            } else {
                m_Animator.SetBool("isLooking", false);
                isLooking = false;
            }
        }  
    }


    void OnAnimatorIK() {

        if (Input.GetButton("Fire")) {
            lookAtTargetPosition.y = m_Head.position.y;
            float lookAtTargetWeight = isLooking ? 1.0f : 0.0f;

            Vector3 curDir = lookAtPosition - m_Head.position;
            Vector3 futDir = lookAtTargetPosition - m_Head.position;

            curDir = Vector3.RotateTowards(curDir, futDir, 26.28f * Time.deltaTime, float.PositiveInfinity);
            lookAtPosition = m_Head.position + curDir;

            float blendTime = lookAtTargetWeight > lookAtWeight ? lookAtHeatTime : lookAtCoolTime;
            lookAtWeight = Mathf.MoveTowards(lookAtWeight, lookAtTargetWeight, Time.deltaTime / blendTime);
            m_Animator.SetLookAtWeight(lookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
            m_Animator.SetLookAtPosition(lookAtPosition);

            Debug.Log(lookAtPosition);
        }
    }

}
