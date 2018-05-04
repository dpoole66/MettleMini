using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MettleAI : MonoBehaviour {

    Animator anim;
    public GameObject Player;

    public GameObject GetPlayer(){

        return Player;

    }

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {

        anim.SetFloat("Distance", Vector3.Distance(transform.position, Player.transform.position));
		
	}
}
