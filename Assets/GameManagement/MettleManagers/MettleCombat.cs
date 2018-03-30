using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MettleCombat : MonoBehaviour {

    MettleStatus m_Status;
    Animator m_Anim;

	void Awake () {

        m_Anim = GetComponent<Animator>();
		
	}
	
    public void Strike(float attackForce, float attackRate){
        //I'm keeping the floats from Status so I can tie health/stamina to the attack force and rate. Different anims to reflect stamina.
        m_Anim.SetTrigger("AttackStrong");

        if(attackForce <= 10.0f){

            m_Anim.SetTrigger("AttackWeak");

        }
 
    }
}
