using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets;
using UnityEngine.EventSystems;

public class DragPlayer : MonoBehaviour {

    const float k_Spring = 50.0f;
    const float k_Damper = 5.0f;
    const float k_Drag = 10.0f;
    const float k_AngularDrag = 5.0f;
    const float k_Distance = 0.2f;
    const bool k_AttachToCenterOfMass = false;

    public bool isMoveing;

    private SpringJoint m_SpringJoint;

    private void Update() {

        int tCountTouches = Input.touchCount;

        // Make sure the user pressed the mouse down
        if (tCountTouches <= 0) {
            return;
        }

        //var mainCamera = FindCamera();

        // We need to actually hit an object
        RaycastHit hit = new RaycastHit();
        if (tCountTouches > 0) {

            for (int i = 0; i < Input.touchCount; i++) {

                if (Input.GetTouch(i).phase == TouchPhase.Began && !IsPointerOverUIObject()) {

                    // We need to hit a rigidbody that is not kinematic
                    if (!hit.rigidbody || hit.rigidbody.isKinematic) {
                        return;
                    }

                    if (!m_SpringJoint) {
                        var go = new GameObject("Rigidbody dragger");
                        Rigidbody body = go.AddComponent<Rigidbody>();
                        m_SpringJoint = go.AddComponent<SpringJoint>();
                        body.isKinematic = true;
                    }

                    m_SpringJoint.transform.position = hit.point;
                    m_SpringJoint.anchor = Vector3.zero;

                    m_SpringJoint.spring = k_Spring;
                    m_SpringJoint.damper = k_Damper;
                    m_SpringJoint.maxDistance = k_Distance;
                    m_SpringJoint.connectedBody = hit.rigidbody;

                    StartCoroutine("DragObject", hit.distance);

                }

            }
        }
      
     }


    private IEnumerator DragObject(float distance) {
        var oldDrag = m_SpringJoint.connectedBody.drag;
        var oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
        m_SpringJoint.connectedBody.drag = k_Drag;
        m_SpringJoint.connectedBody.angularDrag = k_AngularDrag;
        var mainCamera = FindCamera();
        while (Input.GetTouch(0).phase == TouchPhase.Began) {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            m_SpringJoint.transform.position = ray.GetPoint(distance);
            yield return null;
        }
        if (m_SpringJoint.connectedBody) {
            m_SpringJoint.connectedBody.drag = oldDrag;
            m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
            m_SpringJoint.connectedBody = null;
        }
    }


    private Camera FindCamera() {
        if (GetComponent<Camera>()) {
            return GetComponent<Camera>();
        }

        return Camera.main;
    }

    //UI Touch and Button control to prevent raycast passthrough
    private bool IsPointerOverUIObject() {

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current) {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }
}


