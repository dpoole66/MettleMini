using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Animator))]

public class PlayerControllerMini : MonoBehaviour {

    private Transform m_PlayerTrans;
    private Vector3 destinationPos;
    private float destinationDis;
    public float Speed;
    public float moveAnimSpeed;
    public float animSpeed = 1.5f;
    public int idleState = Animator.StringToHash("Base Layer.Idle");
    public int chaseState = Animator.StringToHash("Base Layer.Chaseing");
    public float Direction;
    [HideInInspector] public Animator m_Anim;
    private AnimatorStateInfo currentBaseState;
    private Collider m_Collider;

	void Start () {
        Physics.gravity = new Vector3(0, -200f, 0);
        m_Anim = GetComponent<Animator>();
        m_PlayerTrans = transform;
        destinationPos = m_PlayerTrans.position;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        //monitor distance between player and destinationPos
        //currentBaseState = m_Anim.GetCurrentAnimatorStateInfo(0);
        destinationDis = Vector3.Distance(destinationPos, m_PlayerTrans.position);

        //speed in reference to distance
        if(destinationDis < 0.005f){

            Speed = 0;                 

        }     else if(destinationDis > 0.005f) {

            Speed = 0.3f;                  

        }

        //animator
        if(Speed > 0.2f){

            m_Anim.ResetTrigger("Idle");
            m_Anim.SetTrigger("Chaseing");

        }   else if (Speed < 0.2f){

            m_Anim.ResetTrigger("Chaseing");
            m_Anim.SetTrigger("Idle");

        }

        //touch/mouse input to move player
       if(Input.GetMouseButton(0)){

            Plane playerPlane = new Plane(Vector3.up, m_PlayerTrans.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist)) {

                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPos = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                m_PlayerTrans.rotation = targetRotation;
    
                m_PlayerTrans.position = Vector3.MoveTowards(m_PlayerTrans.position, destinationPos, Speed * Time.deltaTime);
      
            }   
            
            if (Input.GetMouseButtonUp(0)) {

                m_PlayerTrans.position = Vector3.MoveTowards(m_PlayerTrans.position, this.transform.position, Speed = 0.0f);
                //m_Anim.ResetTrigger("Chaseing");
                //m_Anim.SetTrigger("Idle");

            }
       }

	}

}
