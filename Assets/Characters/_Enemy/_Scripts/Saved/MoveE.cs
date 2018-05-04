using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GoogleARCore;
using UnityStandardAssets.CrossPlatformInput;

//[RequireComponent(typeof(Brain))]

public class MoveE : MonoBehaviour {

    public Brain m_Brain;

    public void MoveThis() {

        if (true) {

            RaycastHit hitInfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)) {

                //navMeshAgent
                m_Brain.m_Agent.isStopped = false;
                m_Brain.m_Agent.updatePosition = true;
                m_Brain.m_Agent.updateRotation = true;
                m_Brain.m_Agent.destination = hitInfo.point;
                //animation
                m_Brain.m_Anim.SetBool("Idle", false);
                m_Brain.m_Anim.SetBool("Walking", true);
                return;

            } 

        }

    }


    public IEnumerator Move() {

        while (m_Brain.m_Agent.remainingDistance >= m_Brain.m_Agent.stoppingDistance) {

            RaycastHit hitInfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)) {

                //navMeshAgent
                m_Brain.m_Agent.isStopped = false;
                m_Brain.m_Agent.updatePosition = true;
                m_Brain.m_Agent.updateRotation = true;
                m_Brain.m_Agent.destination = hitInfo.point;
                //animation
                m_Brain.m_Anim.SetBool("Idle", false);
                m_Brain.m_Anim.SetBool("Walking", true);
                yield break;

            } else if (m_Brain.m_Agent.remainingDistance <= m_Brain.m_Agent.stoppingDistance) {

                m_Brain.m_Agent.isStopped = true;
                m_Brain.m_Anim.SetBool("Walking", false);
                m_Brain.m_Anim.SetBool("Idle", true);
                yield break;
            }

            yield return null;

         }
    }

    public IEnumerator Idle() {

        if (m_Brain.m_Agent.velocity.sqrMagnitude <= 0.03f) {

            m_Brain.m_Agent.isStopped = true;
            m_Brain.m_Anim.SetBool("Walking", false);
            m_Brain.m_Anim.SetBool("Idle", true);

            yield return null;
        }

    }

}
