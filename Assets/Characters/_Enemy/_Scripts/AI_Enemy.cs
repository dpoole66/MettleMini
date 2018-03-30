﻿using UnityEngine;
using System.Collections;
//------------------------------------------
public class AI_Enemy : MonoBehaviour
{
    //------------------------------------------
    public enum ENEMY_STATE { PATROL, CHASE, ATTACK };
    //------------------------------------------
    public ENEMY_STATE CurrentState {
        get { return currentstate; }

        set {
            //Update current state
            currentstate = value;

            //Stop all running coroutines
            StopAllCoroutines();

            switch (currentstate) {
                case ENEMY_STATE.PATROL:
                StartCoroutine(AIPatrol());
                break;

                case ENEMY_STATE.CHASE:
                StartCoroutine(AIChase());
                break;

                case ENEMY_STATE.ATTACK:
                StartCoroutine(AIAttack());
                break;
            }
        }
    }
    //------------------------------------------
    [SerializeField]
    private ENEMY_STATE currentstate = ENEMY_STATE.PATROL;

    //Reference to line of sight component
    private LineSight ThisLineSight = null;

    //Reference to nav mesh agent
    private UnityEngine.AI.NavMeshAgent ThisAgent = null;

    //Reference to Animator
    private Animator ThisAnim = null;

    //Reference to transform
    private Transform ThisTransform = null;

    //Reference to player health
    public PlayerHealth PlayerHealth = null;

    //Reference to player transform
    private Transform PlayerTransform = null;

    //Reference to patrol destination
    public Transform PatrolDestination = null;

    //Damage amount per second
    public float MaxDamage = 10f;
    //------------------------------------------
    void Awake() {
        ThisLineSight = GetComponent<LineSight>();
        ThisAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        ThisAnim = GetComponent<Animator>();
        ThisTransform = GetComponent<Transform>();
        PlayerTransform = PlayerHealth.GetComponent<Transform>();
    }
    //------------------------------------------
    void Start() {
        //Configure starting state
        CurrentState = ENEMY_STATE.PATROL;
    }
    //------------------------------------------
    public IEnumerator AIPatrol() {
        //Loop while patrolling
        while (currentstate == ENEMY_STATE.PATROL) {
            //Set strict search
            ThisLineSight.Sensitity = LineSight.SightSensitivity.STRICT;
            ThisAnim.ResetTrigger("Chaseing");
            ThisAnim.ResetTrigger("Atacking");
            ThisAnim.SetTrigger("Walking");
            //Chase to patrol position
            ThisAgent.isStopped = false;
            ThisAgent.SetDestination(PatrolDestination.position);

            //Wait until path is computed
            while (ThisAgent.pathPending)
                yield return null;

            //If we can see the target then start chasing
            if (ThisLineSight.CanSeeTarget) {
                ThisAgent.isStopped = true;
                CurrentState = ENEMY_STATE.CHASE;
                yield break;
            }

            //Wait until next frame
            yield return null;
        }
    }
    //------------------------------------------
    public IEnumerator AIChase() {
        //Loop while chasing
        while (currentstate == ENEMY_STATE.CHASE) {
            //Set loose search
            ThisLineSight.Sensitity = LineSight.SightSensitivity.LOOSE;
            ThisAnim.ResetTrigger("Walking");
            ThisAnim.SetTrigger("Chaseing");

            //Chase to last known position
            ThisAgent.isStopped = false;
            ThisAgent.SetDestination(ThisLineSight.LastKnowSighting);

            //Wait until path is computed
            while (ThisAgent.pathPending)
                yield return null;

            //Have we reached destination?
            if (ThisAgent.remainingDistance <= ThisAgent.stoppingDistance) {
                //Stop agent
                ThisAgent.isStopped = true;

                //Reached destination but cannot see player
                if (!ThisLineSight.CanSeeTarget)
                    CurrentState = ENEMY_STATE.PATROL;
                else //Reached destination and can see player. Reached attacking distance
                    CurrentState = ENEMY_STATE.ATTACK;

                yield break;
            }

            //Wait until next frame
            yield return null;
        }
    }
    //------------------------------------------
    public IEnumerator AIAttack() {
        //Loop while chasing and attacking
        while (currentstate == ENEMY_STATE.ATTACK) {
            //Chase to player position
            ThisAgent.isStopped = false;
            ThisAgent.SetDestination(PlayerTransform.position);        
            ThisAnim.SetTrigger("Chaseing");

            //Wait until path is computed
            while (ThisAgent.pathPending)
                yield return null;

            //Has player run away?
            if (ThisAgent.remainingDistance > ThisAgent.stoppingDistance) {
                //Change back to chase
                CurrentState = ENEMY_STATE.CHASE;
                yield break;
            } else {
                //Attack
                PlayerHealth.m_HealthNow -= MaxDamage * Time.deltaTime;
                ThisAnim.ResetTrigger("Chaseing");
                ThisAnim.SetTrigger("Attacking");
            }

            //Wait until next frame
            yield return null;
        }

        yield break;
    }
    //------------------------------------------
}