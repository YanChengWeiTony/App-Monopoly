using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
using System;

public class Canvas_3Controller : MonoBehaviour {


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Reset_Active(int mode){
		Button[] buttons = this.GetComponentsInChildren<Button>();
		if (mode == 0) {
			//on click
			for (int i = 0; i < buttons.Length; i++) {
				buttons [i].interactable = false;
			}
		}
		if (mode == 1) {
			//normal
			for (int i = 0; i < buttons.Length; i++) {
				buttons [i].interactable = true;
			}
		}
	}
}
