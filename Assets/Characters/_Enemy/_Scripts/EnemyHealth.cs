using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    public float m_StartingHealth = 100.0f;
    public Slider m_HealthUI;

    private bool m_Dead;
    private float m_HealthNow;

	void OnEnable () {

        m_HealthNow = m_StartingHealth;
        m_Dead = false;
		
	}
	
    public void TakeDamage(float amount){

            m_HealthNow -= amount;

            SetHealthUI();

            if(m_HealthNow <= 0.0f && !m_Dead){

                SetDead();

            }

    }

    private void SetHealthUI(){

        m_HealthUI.value = m_HealthNow;

    }

    public void SetDead(){

        m_Dead = true;
        gameObject.SetActive(false);

    }
}
