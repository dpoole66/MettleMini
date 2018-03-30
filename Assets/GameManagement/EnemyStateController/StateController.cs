using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class StateController : MonoBehaviour {

    public State currentState;
    public MettleStatus mettleStatus;
    public Transform m_MettleEye;
    public State remainState;

    //debug
    [HideInInspector] public Image debugUI;

    [HideInInspector] public NavMeshAgent m_Agent, m_AgentEnemy;    
    [HideInInspector] public Animator m_Anim;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public MettleCombat m_MettleCombat;

    private bool aiActive;
    public BoolReference allMettlesSpawned;


    void Awake() { 
    
        m_Agent = GetComponent<NavMeshAgent>();
        m_Anim = GetComponent<Animator>();

        m_AgentEnemy = GameObject.Find("PlayerMini(Clone)").GetComponent<NavMeshAgent>();
        Debug.Log(m_AgentEnemy);

        chaseTarget = GameObject.Find("PlayerMini(Clone)").transform;

        debugUI = GetComponent<Image>();


        if (!m_AgentEnemy) {

            Debug.Log("No Agent found for enemy");

        }

    }

    public void SetupAI(bool value) {

        if(allMettlesSpawned.Value == true){

                aiActive = true;

        }
      

        if (aiActive) {
            m_Agent.enabled = true;
        } else {
            m_Agent.enabled = false;
        }
    }

    void Update() {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
    }

    void OnDrawGizmos() {
        if (currentState != null && m_MettleEye != null) {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(m_MettleEye.position, mettleStatus.lookSphereCastRadius);
        }
    }

    public void TransitionToState(State nextState) {
        if (nextState != remainState) {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration) {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState() {
        stateTimeElapsed = 0;
    }
}