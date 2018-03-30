using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    private Animator m_Anim;

	// Use this for initialization
	void Start () {

        m_Anim = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonDown(1)){

                m_Anim.SetTrigger("Attack");

        }

        if (Input.GetMouseButtonDown(2)) {

            m_Anim.SetTrigger("Defend");

        }

    }
}
