﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectHit : MonoBehaviour {
    //[HideInInspector] public AudioSource m_NPCAudio;
    private Transform m_EnemyUI;
    //private Slider m_E_healthbar;
    //Animator m_Anim;
    public string m_Opponent;

    void Start() {

        //m_Anim = GetComponent<Animator>();
        m_EnemyUI = GetComponent<Transform>();
        //m_E_healthbar = m_EnemyUI.GetComponent<Slider>();
        //m_E_healthbar.value = 100;

    }


    // Hit Trigger count
    void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag != m_Opponent) {

            //m_Anim.SetBool("isHit", true);
            Debug.Log("Hit");
            return;

        } //else  {

            //m_E_healthbar.value -= 5;

        //}

        //if(m_E_healthbar.value <= 0){

            //m_Anim.SetBool("isDead", true);

       // }
      
    }

    private void OnTriggerExit(Collider other) {
        //m_Anim.SetBool("isHit", false);
        Debug.Log("Hit Done");
    }

}
