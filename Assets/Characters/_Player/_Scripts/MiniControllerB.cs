﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GoogleARCore;

[RequireComponent(typeof(Animator))]

public class MiniControllerB : MonoBehaviour {

    [HideInInspector] public Animator m_Anim;
    [HideInInspector] public Rigidbody m_Rigid;
    public Transform m_Enemy;
    private bool move = false;
    private Transform m_PlayerTrans;
    private Vector3 destinationPos;
    private float destinationDis;
    private bool inRange = false;
    public float setMovementRange = 0.1f;
    public float Speed = 0.3f;
    private float m_Speed;
    //Combat
    public float enGuardRange;
    public float attackRange;
    public Button b_Attack;

    private MettleARController marc;
    private GameObject mettleStage;
    private Plane playerPlane;
    public float MoveClamp;


    void Start() {
        Physics.gravity = new Vector3(0, -200f, 0);
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_PlayerTrans = transform;
        destinationPos = m_PlayerTrans.position;
        m_Speed = Speed;
        m_Enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        //marc = GameObject.FindGameObjectWithTag("ARManager").GetComponent<MettleARController>();
        mettleStage = GameObject.FindGameObjectWithTag("Stage");
        //attack button
        b_Attack = GetComponent<Button>();
        //b_Attack.onClick.AddListener(B_Attack_1);


    }

    private void Update() {
        if (m_Enemy != null) {

            Debug.Log(m_Enemy.position);

        }

        //Combat
        var combatRange = Vector3.Distance(m_PlayerTrans.position, m_Enemy.position);


        //attack
        if (Input.GetMouseButtonDown(1)) {

            StartCoroutine(Attack_1());

        }

        //defend
        if (Input.GetMouseButton(2)) {

            StartCoroutine(Defend_1());

        }


        //Movement
        //speed in reference to distance
        if (m_Speed <= 0.1f) {

            m_Anim.ResetTrigger("Chaseing");
            m_Anim.SetTrigger("Idle");

        } else if (m_Speed >= 0.2f) {

            m_Anim.ResetTrigger("Idle");
            m_Anim.SetTrigger("Chaseing");

        }

        //touch/mouse input to move player
        if (Input.GetMouseButton(0) && !inRange && !EventSystem.current.IsPointerOverGameObject()) {

            m_Speed = 3.0f;
            Plane playerPlane = new Plane(Vector3.up, mettleStage.transform.position);
            RaycastHit hitInfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = hitInfo.distance;

            if (playerPlane.Raycast(ray, out hitdist)) {

                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPos = targetPoint;
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

        } else {

            m_Speed = 0.0f;

        }

        if (Input.GetMouseButtonUp(0)) {

            inRange = false;
            m_Speed = 3.0f;

        }

    }

    //UI Touch and Button control to prevent raycast passthrough
    /*private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current) {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }    */

    private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current) {

            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)  

        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }


    public void GetMovementRange(Vector3 mvRange) {

        //move clamp
        mettleStage = GetComponent<MettleARController>().mettleStageInstance;
        mvRange = Vector3.ClampMagnitude(mettleStage.transform.position, MoveClamp);

        return;

    }

    //Methods

    //Combat
    public void B_Attack_1() {     //UI Attack button

        StartCoroutine(Attack_1());
        return;

    }
    public IEnumerator Attack_1() {    //Attack coro

        while (true) {

            m_Speed = 0.0f;

            Vector3 relativePos = m_Enemy.position - m_PlayerTrans.position;
            Quaternion lookAtTarget = Quaternion.LookRotation(relativePos);
            m_PlayerTrans.rotation = lookAtTarget;
            m_Anim.Play("Attacking");
            yield break;

         }

    }

    //Defend
    public void B_Defend_1() {

        StartCoroutine(Defend_1());
        return;

    }

    public IEnumerator Defend_1() {     //Defend coro

        m_Speed = 0.0f;

        Vector3 relativePos = m_Enemy.position - m_PlayerTrans.position;
        Quaternion lookAtTarget = Quaternion.LookRotation(relativePos);
        m_PlayerTrans.rotation = lookAtTarget;
        m_Anim.Play("Defending");

        yield break;

    }


}