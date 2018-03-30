using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController2 : MonoBehaviour {
    private Rigidbody ThisRigid= null;
    private Transform ThisTransform = null;
    private Animator ThisAnimator;
    public float RotateSpeed = 90f;
    public float MaxSpeed = 1.0f;
    public float JumpForce = 50f;
    public float GroundedDist = 0.1f;
    public bool IsGrounded = false;
    private Vector3 m_Target;
    //for input
    public float surfaceOffset;
    //
    private Vector3 Velocity = Vector3.zero;
    public LayerMask GroundLayer;

    // Use this for initialization
    void Awake() {
        ThisRigid = GetComponent<Rigidbody>();
        ThisTransform = GetComponent<Transform>();
        ThisAnimator = GetComponent<Animator>();
    }

    private void Update() {

        //Rotate
        Quaternion targetRotation = Quaternion.LookRotation(m_Target - transform.position);
        ThisTransform.rotation = targetRotation;

        //Input
        if (Input.GetMouseButton(0)) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) {
                return;
            }
            m_Target = hit.point + hit.normal * surfaceOffset;

        } else {

            ThisAnimator.SetTrigger("Idle");

        }

    }


    // Update is called once per frame
    void FixedUpdate() {

        //Move
        //ThisRigid.transform.position = Vector3.MoveTowards(ThisTransform.position, m_Target, 1.0f * Time.deltaTime);
        ThisRigid.MovePosition(ThisRigid.position + m_Target * 1.0f * Time.deltaTime);
        ThisAnimator.SetTrigger("Chaseing");

    }

    public float DistanceToGround() {
        RaycastHit hit;
        float distanceToGround = 0;
        if (Physics.Raycast(ThisTransform.position, -Vector3.up, out hit, Mathf.Infinity, GroundLayer))
            distanceToGround = hit.distance;
        return distanceToGround;
    }
}
