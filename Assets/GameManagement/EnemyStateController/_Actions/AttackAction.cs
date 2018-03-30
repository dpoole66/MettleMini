using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MettleAI/Actions/Attack")]
public class AttackAction : Action {
    public override void Act(StateController controller) {
        Attack(controller);
    }


    private void Attack(StateController controller) {
        RaycastHit hit;

        Debug.DrawRay(controller.m_MettleEye.position, controller.m_MettleEye.forward.normalized * controller.mettleStatus.attackRange, Color.red);

        if (Physics.SphereCast(controller.m_MettleEye.position, controller.mettleStatus.lookSphereCastRadius, controller.m_MettleEye.forward, out hit, controller.mettleStatus.attackRange)
            && hit.collider.CompareTag("Player")) {
            if (controller.CheckIfCountDownElapsed(controller.mettleStatus.attackRate)) {
                controller.m_MettleCombat.Strike(controller.mettleStatus.attackForce, controller.mettleStatus.attackRate);
            }
        }

        //debug
        controller.debugUI.color = Color.red;


    }
}
