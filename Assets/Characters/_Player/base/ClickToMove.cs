﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour {

    public float surfaceOffset = -0.1f;
    private GameObject _movePlayer;


    private void Start() {
 
      _movePlayer = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update() {

        //_movePlayer = GameObject.Find("MettlePlayerOne(Clone)");

        if (!Input.GetMouseButtonDown(0)) {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) {
            return;
        }
        transform.position = hit.point + hit.normal * surfaceOffset;
        if ((_movePlayer != null) && (hit.transform.gameObject.tag == "Ground")) {
            _movePlayer.SendMessage("SetTarget", transform);

            Debug.Log(_movePlayer);
        }
    }
}
