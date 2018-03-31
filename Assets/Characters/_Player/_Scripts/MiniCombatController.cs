using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]

public class MiniCombatController : MonoBehaviour {

    public Transform m_Enemy;
    private Transform m_PlayerTrans;
    [HideInInspector] public Animator m_Anim;
    public float enGuardRange = 0.2f;
    public float attackRange = 0.1f;
    public float combatRangeFinder;
    private bool move = false;

    // Use this for initialization
    void Start () {

        Physics.gravity = new Vector3(0, -200f, 0);
        m_Anim = GetComponent<Animator>();
        m_PlayerTrans = transform;
        m_Enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        //attack button
        //Button b_Attack = GetComponent<Button>();
        //b_Attack.onClick.AddListener(B_Attack_1);
        //Button b_Defend = GetComponent<Button>();
        //b_Attack.onClick.AddListener(B_Defend_1);

    }


	
	// Update is called once per frame
	void Update () {

        combatRangeFinder = Vector3.Distance(m_PlayerTrans.position, m_Enemy.position);

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
