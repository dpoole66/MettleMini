﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]

public class MiniMovementController : MonoBehaviour {

    [HideInInspector] public Animator m_Anim;
    [HideInInspector] public Rigidbody m_Rigid;
    private Transform m_PlayerTrans;
    private Vector3 destinationPos;
    private float destinationDis;
    public Vector3 targetPoint;
    public float rangeFinder;
    private bool inRange = false;
    public float setMovementRange = 0.01f;
    public float Speed = 0.3f;
    private float m_Speed;
    public bool isMoveing;

    // Use this for initialization
    void Start () {

        m_PlayerTrans = transform;
        destinationPos = m_PlayerTrans.position;
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {

        rangeFinder = Vector3.Distance(m_PlayerTrans.position, targetPoint);
        if (rangeFinder <= setMovementRange) {

            inRange = true;

        } else {

            inRange = false;

        }

        int tCountTouches = Input.touchCount;

        if (tCountTouches > 0) {

            for (int i = 0; i < Input.touchCount; i++) {

                if (Input.GetTouch(i).phase == TouchPhase.Began && !inRange) {

                    isMoveing = true;

                    Plane playerPlane = new Plane(Vector3.up, m_PlayerTrans.position);
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);                             
                    float hitdist = 0.0f;

                    if (playerPlane.Raycast(ray, out hitdist) && !IsPointerOverUIObject()) {

                        targetPoint = ray.GetPoint(hitdist);
                        destinationPos = ray.GetPoint(hitdist);
                        //Move
                        Mover();

                    }

                } else if(Input.GetTouch(0).phase == TouchPhase.Ended) {

                    isMoveing = false;
                    Stopper();

                }
            }
        }

    }


    //UI Touch and Button control to prevent raycast passthrough
    private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }

    public void Mover(){

        m_Speed = 3.0f;
            if (isMoveing == true){ 

                //set rotation
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                m_PlayerTrans.rotation = targetRotation;

                //move player
                destinationDis = Vector3.Distance(destinationPos, m_PlayerTrans.position);
                m_Rigid.position = Vector3.MoveTowards(m_PlayerTrans.position, destinationPos, Speed * Time.deltaTime);    //
        
                //animator
                m_Anim.ResetTrigger("Idle");
                m_Anim.SetTrigger("Chaseing");

            } else if (m_PlayerTrans.position == destinationPos) {     //This is to stop the char 

                isMoveing = false;
                Stopper();

            }
    }

    public void Stopper(){

        if(isMoveing == false){

            m_Rigid.position = this.transform.position; 
            m_Speed = 0.0f;
            //animator
            m_Anim.ResetTrigger("Chaseing");
            m_Anim.SetTrigger("Idle");

        }

    }

}
