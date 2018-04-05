﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]

public class MiniTouchInputController : MonoBehaviour {


    [HideInInspector] public Animator m_Anim;   //MiniMovement
    [HideInInspector] public Rigidbody m_Rigid;
    public Transform m_Enemy;
    private bool move = false;
    private Transform m_PlayerTrans;       //MiniMovement
    private Vector3 destinationPos;      //MiniMovement
    private float destinationDis;       //MiniMovement
    private bool inRange = false;
    public float setMovementRange = 0.01f;    //MiniMovement
    public float Speed = 0.3f;    //MiniMovement
    private float m_Speed;      //MiniMovement
    //Combat
    public float enGuardRange = 0.2f;
    public float attackRange = 0.1f;


    void Start() {

        Physics.gravity = new Vector3(0, -200f, 0);
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_PlayerTrans = transform;
        destinationPos = m_PlayerTrans.position;
        m_Speed = Speed;
        m_Enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        //attack button
        Button b_Attack = GetComponent<Button>();
        b_Attack.onClick.AddListener(B_Attack_1);

    }

    private void Update() {

        int tcTouches = Input.touchCount;

        if (tcTouches > 0) {

            for (int i = 0; i < Input.touchCount; i++) {

                //Touch touch = Input.GetTouch(i);

                if (Input.GetTouch(i).phase == TouchPhase.Began) {

                        m_Speed = 3.0f;
                        Plane playerPlane = new Plane(Vector3.up, m_PlayerTrans.position);
                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                        //RaycastHit hit;
                        float hitdist = 0.0f;


                        if (!IsPointerOverUIObject()) {

                            Vector3 targetPoint = ray.GetPoint(hitdist);
                            destinationPos = ray.GetPoint(hitdist);
                            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                            m_PlayerTrans.rotation = targetRotation;

                            var Range = Vector3.Distance(m_PlayerTrans.position, targetPoint);

                            m_PlayerTrans.position = Vector3.MoveTowards(m_PlayerTrans.position, destinationPos, Speed * Time.deltaTime);
                            Debug.Log(Speed);

                            if (Range <= setMovementRange) {     //This is to stop the char from continuing to try to hit the target while in input touch down

                                inRange = true;
                                m_Speed = 0.0f;

                            }
                        }
                    }
                

            }

        } else {

            m_Speed = 0.0f;

        }

        if (Input.GetMouseButtonUp(0)) {

            inRange = false;
            m_Speed = 0.0f;

        }


        //Combat
        var combatRange = Vector3.Distance(m_PlayerTrans.position, m_Enemy.position);

        //Movement
        //speed in reference to distance
        if (m_Speed <= 0.1f) {

            m_Anim.ResetTrigger("Chaseing");
            m_Anim.SetTrigger("Idle");

        } else if (m_Speed >= 0.2f) {

            m_Anim.ResetTrigger("Idle");
            m_Anim.SetTrigger("Chaseing");

        }

    }

    // Update is called once per frame
    void FixedUpdate() {

        //monitor distance between player and destinationPos
        destinationDis = Vector3.Distance(destinationPos, m_PlayerTrans.position);

    }

    //UI Touch and Button control to prevent raycast passthrough
    private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }

    //Combat
    public void B_Attack_1() {     //UI Attack button

        StartCoroutine(Attack_1());
        return;

    }
    public IEnumerator Attack_1() {    //Attack coro

        Vector3 relativePos = m_Enemy.position - m_PlayerTrans.position;
        Quaternion lookAtTarget = Quaternion.LookRotation(relativePos);
        m_PlayerTrans.rotation = lookAtTarget;
        m_Anim.SetTrigger("Attack");
        yield break;

    }

    //Defend
    public void B_Defend_1() {

        StartCoroutine(Defend_1());
        return;

    }

    public IEnumerator Defend_1() {     //Defend coro

        Vector3 relativePos = m_Enemy.position - m_PlayerTrans.position;
        Quaternion lookAtTarget = Quaternion.LookRotation(relativePos);
        m_PlayerTrans.rotation = lookAtTarget;
        m_Anim.SetTrigger("Defend");
        yield break;


    }
}