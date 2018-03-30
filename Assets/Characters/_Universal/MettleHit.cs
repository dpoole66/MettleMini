using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MettleHit : MonoBehaviour {

    EnemyHealth m_EnemyHealth;            
    public string m_Opponent;


    // Hit Trigger count

    //Player
    void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag != (m_Opponent)) {

            return;

        } else {

            m_EnemyHealth.TakeDamage(10.0f);


        }

        if (m_EnemyHealth.m_HealthUI.value <= 0) {

            m_EnemyHealth.SetDead();

        }

    }

}
