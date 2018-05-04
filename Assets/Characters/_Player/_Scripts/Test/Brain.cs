using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GoogleARCore;
using UnityStandardAssets.CrossPlatformInput;

//This movement controller is for scaled NavMesh characters and stages
[RequireComponent(typeof(NavMeshAgent))]

public class Brain : MonoBehaviour {
    //----------------------------------------------
    public enum PLAYER_STATE{ IDLE, MOVE, ATTACK, DEFEND};
    //----------------------------------------------
    public PLAYER_STATE CurrentState{

        get { return currentstate; }

        set {
            //Update current state
            currentstate = value;

            //Stop all running coroutines
            StopAllCoroutines();

            switch (currentstate) {
                case PLAYER_STATE.IDLE:
                    StartCoroutine(P_Idle());
                    break;

                case PLAYER_STATE.MOVE:
                    StartCoroutine(P_Move());
                    break;

                case PLAYER_STATE.ATTACK:
                    StartCoroutine(P_Attack());
                    break;

                case PLAYER_STATE.DEFEND:
                    StartCoroutine(P_Defend());
                    break;
            }
        }    
    }
    //----------------------------------------------
    [SerializeField]
    private PLAYER_STATE currentstate = PLAYER_STATE.IDLE;
    //----------------------------------------------
    GameObject m_PlayerObj;
    //----------------------------------------------
    public NavMeshAgent m_Agent = null;
    public Animator m_Anim = null;
    public Transform m_ThisTransform = null;
    //----------------------------------------------
    public Transform m_MyTarget = null;     // this is the opposing mettle
    //----------------------------------------------
    public float RotSpeed = 90f;
    public float Damping = 55f;
    //----------------------------------------------
    RaycastHit hitInfo = new RaycastHit();
    public float m_EnemyRange;
    public float m_EnemyRotation;                     
    //----------------------------------------------
    private void Awake() {

        m_Agent = GetComponent<NavMeshAgent>();
        m_Anim = GetComponent<Animator>();
        m_ThisTransform = GetComponent<Transform>();
        m_MyTarget = GetComponent<Transform>();
        m_PlayerObj = this.gameObject;
  
    }
    //---------------------------------------------- 
    private void Start() {


        CurrentState = PLAYER_STATE.IDLE;

    }
    //----------------------------------------------
   
    //----------------------------------------------
    public IEnumerator P_Idle(){

        while (currentstate == PLAYER_STATE.IDLE){

                m_Agent.isStopped = true;
                m_Anim.SetBool("Idle", true);
                m_Anim.SetBool("Walking", false);

                if(Input.GetMouseButtonDown(0)){

                    currentstate = PLAYER_STATE.MOVE;

                }


            yield return null;

         }

    }
    //----------------------------------------------
    public IEnumerator P_Move() {

        while (currentstate == PLAYER_STATE.MOVE) {

            //m_Agent.isStopped = false;
            //m_Anim.SetBool("Idle", false);
            //m_Anim.SetBool("Walking", true);

            //yield return Move();

            //if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)  {

            //currentstate = PLAYER_STATE.IDLE;

            //}       

            // input for movement destination
            if (Input.GetMouseButton(0) && !m_Agent.pathPending) {
                RaycastHit hitInfo = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)) {

                    //navMeshAgent
                    m_Agent.isStopped = false;
                    m_Agent.updatePosition = true;
                    m_Agent.updateRotation = true;
                    m_Agent.destination = hitInfo.point;
                    //animation
                    m_Anim.SetBool("Idle", false);
                    m_Anim.SetBool("Walking", true);

                }

            } else if (m_Agent.velocity.sqrMagnitude <= 0.03f) {

                m_Agent.isStopped = true;
                m_Anim.SetBool("Walking", false);
                m_Anim.SetBool("Idle", true);

                yield break;
            }

            yield return null;

         }

    }
    //----------------------------------------------
    public IEnumerator P_Attack() {

        while (currentstate == PLAYER_STATE.ATTACK) {

            m_Agent.isStopped = true;
            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Walking", false);
            m_Anim.SetBool("Attacking", true);
            m_Anim.SetBool("Defending", false);

            yield return null;

         }

    }
    //----------------------------------------------
    public IEnumerator P_Defend() {

        while (currentstate == PLAYER_STATE.DEFEND) {

            m_Agent.isStopped = true;
            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Walking", false);
            m_Anim.SetBool("Attacking", false);
            m_Anim.SetBool("Defending", true);

            yield return null;

         }

    }
    //----------------------------------------------
    public IEnumerator Move() {

        while (m_Agent.remainingDistance >= m_Agent.stoppingDistance) {

            RaycastHit hitInfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)) {

                //navMeshAgent
                m_Agent.isStopped = false;
                m_Agent.updatePosition = true;
                m_Agent.updateRotation = true;
                m_Agent.destination = hitInfo.point;
                //animation
                m_Anim.SetBool("Idle", false);
                m_Anim.SetBool("Walking", true);
                yield break;

            } else if (m_Agent.remainingDistance <= m_Agent.stoppingDistance) {

                m_Agent.isStopped = true;
                m_Anim.SetBool("Walking", false);
                m_Anim.SetBool("Idle", true);
                yield break;
            }

            yield return null;

        }
    }
}

