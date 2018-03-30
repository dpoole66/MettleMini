using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MettleAI/Actions/Idle")]

public class IdleAction : Action {

    public override void Act(StateController controller) {

        Idle(controller);
    }

    private void Idle(StateController controller) {
  
        controller.m_Agent.destination = controller.m_Agent.transform.position;
        controller.m_Agent.isStopped = true;
        controller.m_Anim.SetTrigger("Idle");

        //debug
        controller.debugUI.color = Color.blue;

        Quaternion DestRot = Quaternion.LookRotation(controller.transform.position - controller.chaseTarget.transform.position, Vector3.up);
        controller.transform.rotation = Quaternion.RotateTowards(controller.transform.rotation, controller.chaseTarget.transform.rotation, 90.0f * Time.deltaTime);

    }
}
