using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GoogleARCore;


public class PStateController : MonoBehaviour {

    //----------------------------------------------------                                       
    NavMeshAgent m_Agent = null;
    Animator m_Anim;
    GameObject m_Enemy, m_Player;
    //PlayerAttack m_PlayerAttack;
    public Text m_PlayerStateText;
    //private Text m_playerStateText;
   public  float enemyRange;
    Vector3 enemyHeading;
    // Movement
    public float surfaceOffset = 0.01f;
    //private GameObject setTargetOn;    

    //-----------------------------------------------------
    private GameObject mettleStage;
    private Transform m_PlayerTrans;
    private Vector3 destinationPos;
    private float destinationDis;
    private bool inRange = false;
    public float setMovementRange = 0.1f;
    public float Speed = 0.3f;
    private float m_Speed;


    private void Awake() {

        m_Agent = GetComponent<NavMeshAgent>();
        m_Anim = GetComponent<Animator>();
        m_Player = this.gameObject; ;
        m_Enemy = GameObject.FindGameObjectWithTag("Enemy");

    }

    void Start() {

        m_PlayerStateText = GetComponent<Text>();

        CurrentState = PLAYER_STATE.IDLE;
 
        if (m_Enemy != null) {

            Debug.Log(m_Enemy);

        }

    }


    //-------------Player finite state machine

    public enum PLAYER_STATE { IDLE, MOVE, ATTACK, DEFEND, INJURED, DEAD };

    [SerializeField]
    private PLAYER_STATE currentState = PLAYER_STATE.IDLE;

    // get private currentState from public encapsulation and return corresponding state
    public PLAYER_STATE CurrentState {

        get { return currentState; }
        set {
            currentState = value;

            StopAllCoroutines();

            switch (currentState) {
                case PLAYER_STATE.IDLE:
                    StartCoroutine(Player_Idle());
                    break;

                case PLAYER_STATE.MOVE:
                    StartCoroutine(Player_Move());
                    break;

                case PLAYER_STATE.ATTACK:
                    StartCoroutine(Player_Attack());
                    break;

                case PLAYER_STATE.DEFEND:
                    StartCoroutine(Player_Defend());
                    break;

                case PLAYER_STATE.INJURED:
                    StartCoroutine(Player_Injured());
                    break;

                case PLAYER_STATE.DEAD:
                    StartCoroutine(Player_Dead());
                    break;

            }

        }

    }


    public IEnumerator Player_Idle() {

        while (currentState == PLAYER_STATE.IDLE) {

            m_Agent.isStopped = true;
            // ClearAnimParams();
            m_Anim.SetBool("Walking", false);
            m_Anim.SetBool("Idle", true);
            m_PlayerStateText.text = "Idle";



            //touch/mouse input to move player
            if (Input.GetMouseButton(0) && !inRange && !IsPointerOverUIObject()) {

                m_Speed = 3.0f;
                Plane playerPlane = new Plane(Vector3.up, mettleStage.transform.position);
                RaycastHit hitInfo = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = hitInfo.distance;

                if (playerPlane.Raycast(ray, out hitdist)) {
                    Debug.Log("Raycast");
                    Vector3 targetPoint = ray.GetPoint(hitdist);
                    destinationPos = targetPoint;
                    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                    m_Agent.destination = targetPoint;
                    m_Player.transform.rotation = targetRotation;

                    m_Agent.isStopped = false;
                    CurrentState = PLAYER_STATE.MOVE;

                    var Range = Vector3.Distance(m_PlayerTrans.position, targetPoint);                   
           
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

            // attack  and move input button
            if (Input.GetMouseButtonDown(1)) {

                m_Agent.isStopped = true;
                m_Anim.SetBool("Walking", false);
                m_Anim.SetBool("Idle", false);
                m_Anim.SetBool("Attacking", true);
                CurrentState = PLAYER_STATE.ATTACK;

                yield break;

            }

            // attack  and move input button
            if (Input.GetMouseButton(2)) {

                m_Agent.isStopped = true;
                m_Anim.SetBool("Defending", true);
                m_Anim.SetBool("Walking", false);
                m_Anim.SetBool("Idle", false);
                m_Anim.SetBool("Attacking", false);
                CurrentState = PLAYER_STATE.DEFEND;

                yield break;

            }

            yield return null;
        }

    }


    //---MOVE
    public IEnumerator Player_Move() {

        while (currentState == PLAYER_STATE.MOVE) {

            //Wait until path is computed
            while (m_Agent.pathPending)
                yield return null;

            m_Agent.isStopped = false;
            //ClearAnimParams();
            m_Anim.SetBool("Idle", false);
            m_Anim.SetBool("Walking", true);
            m_Anim.SetBool("Defending", false);
            //m_Anim.SetBool("Attacking", false);
            m_PlayerStateText.text = "Walking";

            // attack  and move input button
            if (Input.GetMouseButtonDown(1)) {

                m_Agent.isStopped = true;
                CurrentState = PLAYER_STATE.ATTACK;

                yield break;

            }

            // stop  and defend input button
            if (Input.GetMouseButton(2)) {

                m_Agent.isStopped = true;
                CurrentState = PLAYER_STATE.DEFEND;

                yield break;

            }

            // Once Player is at destination
            if (m_Agent.remainingDistance <= m_Agent.stoppingDistance) {

                m_Agent.isStopped = true;
                CurrentState = PLAYER_STATE.IDLE;

                yield break;
            }

            yield return null;
        }

    }

    public IEnumerator Player_Attack() {

        while (currentState == PLAYER_STATE.ATTACK) {

            //ClearAnimParams();
            if (Input.GetMouseButtonDown(1)) {

                m_Agent.transform.LookAt(m_Enemy.transform.position);
                m_Anim.SetBool("Idle", false);
                m_Anim.SetBool("Walking", false);
                m_Anim.SetBool("Defending", false);
                m_Anim.SetBool("Attacking", true);
                m_PlayerStateText.text = "Attacking";
                yield return null;

            } else {

                m_Anim.SetTrigger("Idle");
                CurrentState = PLAYER_STATE.IDLE;

                yield break;
            }

        }

        yield return null;

    }




    public IEnumerator Player_Injured() {

        while (currentState == PLAYER_STATE.INJURED) {
            yield return null;
        }

        yield break;

    }

    public IEnumerator Player_Defend() {

        while (currentState == PLAYER_STATE.DEFEND) {

            if (Input.GetMouseButton(2)) {

                m_Agent.transform.LookAt(m_Enemy.transform);
                m_Anim.SetBool("Idle", false);
                m_Anim.SetBool("Walking", false);
                m_Anim.SetBool("Defending", true);
                m_Anim.SetBool("Attacking", false);
                m_PlayerStateText.text = "Defending";

                yield return null;

            } else {

                CurrentState = PLAYER_STATE.IDLE;

                yield break;
            }

        }

        yield return null;

    }

    public IEnumerator Player_Dead() {

        while (currentState == PLAYER_STATE.DEAD) {
            yield return null;
        }

        yield break;

    }

    //UI Touch and Button control to prevent raycast passthrough
    private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current) {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }

}