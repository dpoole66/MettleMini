using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonOver : MonoBehaviour, IPointerClickHandler {

    public MiniContorllerX m_MiniController;

    void Awake(){

        m_MiniController = GetComponent<MiniContorllerX>();

    }

//Detect click
    public void OnPointerClick(PointerEventData pointerEventData){

        m_MiniController.B_Attack_1();

    }
}
