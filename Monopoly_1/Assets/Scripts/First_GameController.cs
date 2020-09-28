using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class First_GameController : MonoBehaviour {

	public InputField MyInput;
	public InputField Start_yr;
	public InputField Start_mnth;
	public InputField Start_date;
	public InputField Start_hr;
	public InputField Start_mnt;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void Change_Scene(){
		DontDestroyOnLoad (this);
		SceneManager.LoadScene("Home Screen");
	}
}
