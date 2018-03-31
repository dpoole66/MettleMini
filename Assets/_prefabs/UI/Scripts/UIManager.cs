using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject ui_Canvas;
    public GameObject a_Button, d_Button;

	// Use this for initialization
	void Start () {

        GameObject UI_Canvas = Instantiate(ui_Canvas) as GameObject;
        GameObject A_Button = Instantiate(a_Button) as GameObject;
        GameObject D_Button = Instantiate(d_Button) as GameObject;
        A_Button.transform.SetParent(UI_Canvas.transform, false);
        D_Button.transform.SetParent(UI_Canvas.transform, false);

    }

}
