using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MettleAI/Actions/Pursue")]

public class PursueAction : Action {

    public override void Act(StateController controller) {

        Chase(controller);

    }

    private void Chase(StateController controller) {

        controller.m_Agent.destination = controller.chaseTarget.position;
        if (controller.m_Agent.remainingDistance >= controller.m_Agent.stoppingDistance && !controller.m_Agent.pathPending)
            controller.m_Agent.isStopped = false;
        controller.m_Agent.destination = controller.chaseTarget.position;

        Debug.DrawRay(controller.m_MettleEye.transform.position, controller.m_MettleEye.transform.forward.normalized *
        controller.mettleStatus.lookRange, Color.yellow);

        //debug
        controller.debugUI.color = Color.yellow;

        Quaternion DestRot = Quaternion.LookRotation(controller.transform.position - controller.chaseTarget.transform.position, Vector3.up);
        controller.transform.rotation = Quaternion.RotateTowards(controller.transform.rotation, controller.chaseTarget.transform.rotation, 90.0f * Time.deltaTime);

        controller.m_Anim.SetTrigger("Chaseing");

    }

}
