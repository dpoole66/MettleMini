using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTargetWithTouch : MonoBehaviour {

    //public string targetMe;
    private GameObject setTargetOn;
    public float surfaceOffset = 0.05f;

   // public TransfromRef

    private void Awake() {

        setTargetOn = GameObject.Find("PlayerMini(Clone)");

    }

    void Update() {

        int tcTouches = Input.touchCount;

        if(tcTouches > 0){

            for (int i = 0; i < tcTouches; i++) {

                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began) {

                    Ray screenRay = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(screenRay, out hit)) {
                        transform.position = hit.point + hit.normal * surfaceOffset;
                    }
                        if (setTargetOn != null) {
                            setTargetOn.SendMessage("SetTarget", transform);
                        }
                }

            }

        }

    }
}

