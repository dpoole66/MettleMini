using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightConverted : MonoBehaviour {
    private Transform target;
    public float RotationSpeed;
    //sight
    public float FieldOfView = 45f;
    public Transform EyePoint = null;
    public Vector3 LastKnowSighting = Vector3.zero;
    private Transform ThisTransform = null;
    private MettleStatus m_MettleStatus;

    //How sensitive should we be to sight
    public enum SightSensitivity { STRICT, LOOSE };
    //Sight sensitivity
    public SightSensitivity Sensitity = SightSensitivity.STRICT;
    //Can we see target
    public bool CanSeeTarget = false;


    private void Awake() {

        ThisTransform = GetComponent<Transform>();
        LastKnowSighting = ThisTransform.position;

    }

    private void OnTriggerStay(Collider other) {

        target = other.transform;

        Quaternion DestRot = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);

        //Update rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, DestRot, RotationSpeed * Time.deltaTime);

        UpdateSight();

        //Update last known sighting
        if (CanSeeTarget)
            LastKnowSighting = target.position;

    }

    bool InFOV() {
        //Get direction to target
        Vector3 DirToTarget = target.position - EyePoint.position;

        //Get angle between forward and look direction
        float Angle = Vector3.Angle(EyePoint.forward, DirToTarget);

        //Are we within field of view?
        if (Angle <= FieldOfView)
            return true;

        //Not within view
        return false;
    }

    bool ClearLineofSight() {
        RaycastHit Info;

        if (Physics.Raycast(EyePoint.position, (target.position - EyePoint.position).normalized, out Info, m_MettleStatus.lookSphereCastRadius)) {
            //If player, then can see player
            if (Info.transform.CompareTag("Player"))
                return true;
        }

        return false;
    }

    void UpdateSight() {
        switch (Sensitity) {
            case SightSensitivity.STRICT:
                CanSeeTarget = InFOV() && ClearLineofSight();
                break;

            case SightSensitivity.LOOSE:
                CanSeeTarget = InFOV() || ClearLineofSight();
                break;
        }
    }
}
