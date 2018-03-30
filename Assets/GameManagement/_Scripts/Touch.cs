using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour {

    public GameObject flare;
	

	void Update () {

        for (int i = 0; i < Input.touchCount; ++i){

                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);

                if (Physics.Raycast(ray)){

                    Instantiate(flare, transform.position, transform.rotation);

                }

        }

	}
}
