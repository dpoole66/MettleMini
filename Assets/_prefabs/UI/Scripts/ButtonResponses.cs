using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonResponses : MonoBehaviour {

    Button thisButton;
    private GameObject m_Player;
    private MiniCombatController m_McC;

    private void Awake() {

        thisButton = GetComponent<Button>();
        //on click
        thisButton.onClick.AddListener(() => { Attack_1(); });

    }

    public void Attack_1() {
        
            m_McC.Attack_1();

    }
}
