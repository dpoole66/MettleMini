using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Attack : MonoBehaviour {

    private MiniInputController m_MiC;

    private void Awake() {

        m_MiC = GetComponent<MiniInputController>();

    }

    public void OnMouseDown() {

        m_MiC.Attack_1();

    }
}
