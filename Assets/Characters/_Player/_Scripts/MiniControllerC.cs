using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GoogleARCore;

public class MiniControllerC : MonoBehaviour {

    [HideInInspector] public Animator m_Anim;
    [HideInInspector] public NavMeshAgent m_Nav;
    public GameObject m_Enemy;
    public Button b_Attack, b_Defend;
    //movement
    private Vector3 moveTarget;

    private void Awake() {

        m_Anim = GetComponent<Animator>();
        m_Nav = GetComponent<NavMeshAgent>();
        m_Enemy = GameObject.FindGameObjectWithTag("Enemy");
        b_Attack = GetComponent<Button>();   
        b_Defend = GetComponent<Button>();    
    }

    void Start () {

        b_Attack.onClick.AddListener(() => B_Attack_1());
        b_Defend.onClick.AddListener(() => B_Defend_1());
        CurrentState = PLAYER_STATE.IDLE;

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0) && !IsPointerOverUIObject()) {          

            CurrentState = PLAYER_STATE.MOVE;

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
    //-------------------------------------------------------
    public IEnumerator Player_Idle() {

        while (currentState == PLAYER_STATE.IDLE) {

            m_Anim.SetBool("Idle", true);
            m_Anim.SetBool("Attacking", false);
            m_Anim.SetBool("Defending", false);

            if (Input.GetMouseButtonDown(0)) {

                RaycastHit hit = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit) && !IsPointerOverUIObject()) {

                    moveTarget = hit.point;
                    
                }
   
            }
            yield return null;
         }

        CurrentState = PLAYER_STATE.MOVE;

        yield break;

    }

    public IEnumerator Player_Move() {

        while (currentState == PLAYER_STATE.MOVE) {

            m_Nav.destination = moveTarget;
   
            if(m_Nav.remainingDistance >= m_Nav.stoppingDistance){

                m_Anim.SetBool("Idle", false);
                m_Anim.SetBool("Walking", true);

            } else if(!m_Nav.hasPath || m_Nav.velocity.sqrMagnitude == 0f){

                m_Anim.SetBool("Walking", false);
                CurrentState = PLAYER_STATE.IDLE;

            }

            yield return null;
         }

        yield break;

    }

    public IEnumerator Player_Attack() {

        while (currentState == PLAYER_STATE.ATTACK) {

            Vector3 relativePos = m_Enemy.transform.position - this.transform.position;

            if (IsPointerOverUIObject()) {

                Quaternion lookAtTarget = Quaternion.LookRotation(relativePos);
                this.transform.rotation = lookAtTarget;
                m_Anim.SetBool("Attacking", true);

            }    else {

                CurrentState = PLAYER_STATE.IDLE;

            }

            yield return null;
         }

        yield break;

    }

    public IEnumerator Player_Defend() {

        while (currentState == PLAYER_STATE.DEFEND) {
            //m_Anim.SetBool("Defending", true);
            yield return null;
        }

        CurrentState = PLAYER_STATE.IDLE;

        yield break;

    }

    public IEnumerator Player_Injured() {

        while (currentState == PLAYER_STATE.INJURED) {
            yield return null;
        }

        yield break;

    }

    public IEnumerator Player_Dead() {

        while (currentState == PLAYER_STATE.DEAD) {
            yield return null;
        }

        yield break;

    }
    //---------------------------------------------------------

    //Combat
    public void B_Attack_1() {     //UI Attack button

        Debug.Log("Clicked");
        CurrentState = PLAYER_STATE.ATTACK;
        
        return;

    }
    public IEnumerator Attack_1() {    //Attack coro

        m_Anim.SetBool("Attacking", true);
        Debug.Log("In Attack_1");     
        yield break;

    }

    //Defend
    public void B_Defend_1() {

        StartCoroutine(Defend_1());
        return;

    }

    public IEnumerator Defend_1() {     //Defend coro
       
        Vector3 relativePos = m_Enemy.transform.position - this.transform.position;
        Quaternion lookAtTarget = Quaternion.LookRotation(relativePos);
        this.transform.rotation = lookAtTarget;
        m_Anim.SetBool("Defending", true);

        yield break;

    }

    private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }
}
