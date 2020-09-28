using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
using System;

public class GameController : MonoBehaviour {
	//prvate
	private Vector3 v1 = new Vector3 (0.29f, -0.68f, -6.244985f);
	private Vector3 v2 = new Vector3 (0.29f, 11f, -6.244985f);
	private Vector3 v3 = new Vector3 (0.29f, -12.36f, -6.244985f);
	private bool done = false;
	private int LastMinute;

	//fundamental setting
	public int myname;
	public string ID;

	//others gameobject
	public Camera Cma;
	public Plane Pln_1;
	public Plane Pln_2;
	public Plane Pln_3;
	public GameObject others_display;
	public Image buyin_messenge_display;
	public Image sellout_messenge_display;
	public Image confirm_messenge_display;
	public GameObject MyNet;
	public InputField MyBuyInput;
	public InputField MySellInput;

	//stocks
	public double[,] stock_inf;//1: slope, 2: price, 3: remain;
	public GameObject[] mystock;
	public int[,] stock_slope;

	//text
	public Text Name;
	public Text Total_Asset_digit;
	public Text Cash_digit;
	public Text Bank_digit;
	public Text Estate_digit;
	public Text Stock_digit;
	public Text Knife_digit;
	public Text timer;
	public Text buyin_messenger;
	public Text sellout_messenger;

	//button
	public Button Back_others;
	public Button buyin_yes;
	public Button buyin_no;
	public Button sellout_yes;
	public Button sellout_no;
	public Button Confirm;
	public Button All_Update;
	public Button Stock_Update;

	//int and string
	public int Total_Asset;
	public int Cash;
	public int Bank;
	public int Estate;
	public int Stock;
	public int Knife;
	public int time_idx;
	public string stock_buying_selling_pk;
	public int stock_buying_selling_state; //1 for buying, -1 for selling

	//time
	public DateTime StartTime;

	//url
	private const string myurl = "http://davidbrother.pythonanywhere.com/admin/";

	// Use this for initialization
	void Start () {
		//transfer from other scene
		First_GameController myscript = GameObject.Find ("First_GameController").GetComponent<First_GameController>();
		Name.text = myscript.MyInput.text;
		myname = (int)Convert.ToUInt32 (Name.text);

		int yr = (int)Convert.ToUInt32 (myscript.Start_yr.text);
		int mnth = (int)Convert.ToUInt32 (myscript.Start_mnth.text);
		int date = (int)Convert.ToUInt32 (myscript.Start_date.text);
		int hr = (int)Convert.ToUInt32 (myscript.Start_hr.text);
		int mnt = (int)Convert.ToUInt32 (myscript.Start_mnt.text);

		StartTime = new DateTime (yr, mnth, date, hr, mnt, 0);

		//fundemental setting
		ID = "1";

		LastMinute = -1;

		stock_inf = new double[4, 7];

		stock_slope = new int[31, 7];

		DateTime nowDateTime = DateTime.Now;
		TimeSpan delta = nowDateTime.Subtract (StartTime);
		time_idx = (int)(delta.TotalMinutes);

		Network_Update MyNetScript = MyNet.GetComponent<Network_Update>();
		MyNetScript.Connection (Name.text, time_idx);
	}
	
	// Update is called once per frame
	void Update () {
		//time evolution
		DateTime nowDateTime = DateTime.Now;
		timer.text = String.Format ("{0:D4}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}\n遊戲進行時間(分鐘): {6:D}", nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, nowDateTime.Hour, nowDateTime.Minute, nowDateTime.Second, time_idx);		

		if (nowDateTime.Minute != LastMinute) {
			LastMinute = nowDateTime.Minute;
			time_idx++;
		}

		//confirm update frequency	
		if (nowDateTime.Minute % 5 == 4)
			done = false; 

		//per 5 minutes update -> url, instant in the bank, Knife effect, stock_inf development;
		if (nowDateTime.Minute % 5 == 0 && !done) {
			done = true;
		
			Network_Update MyNetScript = MyNet.GetComponent<Network_Update>();
			MyNetScript.Connection (Name.text, time_idx);
		}


		//calculate and display stock digit
		Stock = 0;
		for (int i = 1; i < mystock.Length; i++) {
			Text[] buffer = mystock [i].GetComponentsInChildren<Text> ();
			Stock += (int)Convert.ToUInt32 (buffer [2].text) * (int)Convert.ToUInt32 (buffer [3].text);
		}
		Stock_digit.text = String.Format ("{0:D}", Stock);

		//calculate and display Total_Asset digit
		Total_Asset = (int)Convert.ToUInt32(Cash_digit.text) + (int)Convert.ToUInt32(Bank_digit.text) + (int)Convert.ToUInt32(Estate_digit.text);
		Total_Asset_digit.text = String.Format("{0:D}", Total_Asset);
	}

	public void ChangePage(string ID){
		this.ID = ID;
	
		if (ID == "1") {
			Cma.transform.position = v1;
		}
		if (ID == "2") {
			Cma.transform.position = v2;
		}
		if (ID == "3") {
			Cma.transform.position = v3;
		}
	}

	public void See_others(int mode){
		//Consider all button;
		if(mode == 1){
			//on click
			others_display.gameObject.SetActive(true);
		
			Back_others.interactable = true;
		}
		if(mode == 2){
			//go back
			others_display.gameObject.SetActive(false);
		}
	}

	public void buyin__money(string pk){
		stock_buying_selling_pk = pk;
		stock_buying_selling_state = 1;
	}

	public void Sellout__money(string pk){
		stock_buying_selling_pk = pk;
		stock_buying_selling_state = -1;
	}

	public void Send_Stock_money(){
		if (stock_buying_selling_state == 1) {
			Network_Update MyNetScript = MyNet.GetComponent<Network_Update>();
			MyNetScript.Buy_Stock_Connection (Name.text, stock_buying_selling_pk ,MyBuyInput.text);
		}
		if (stock_buying_selling_state == -1) {
			Network_Update MyNetScript = MyNet.GetComponent<Network_Update>();
			MyNetScript.Buy_Stock_Connection (Name.text, stock_buying_selling_pk ,"-" + MySellInput.text);
		}

	}

	public void buyin_sellout(int mode){
		if (mode == 0) {
			buyin_messenge_display.gameObject.SetActive (false);
			sellout_messenge_display.gameObject.SetActive (false);
			confirm_messenge_display.gameObject.SetActive (false);
		}
		if (mode == 1) {
			buyin_messenge_display.gameObject.SetActive (true);
			sellout_messenge_display.gameObject.SetActive (false);
			confirm_messenge_display.gameObject.SetActive (false);

			buyin_yes.interactable = true;
			buyin_no.interactable = true;
		}
		if (mode == 2) {
			buyin_messenge_display.gameObject.SetActive (false);
			sellout_messenge_display.gameObject.SetActive (true);
			confirm_messenge_display.gameObject.SetActive (false);

			sellout_yes.interactable = true;
			sellout_no.interactable = true;
		}
		if (mode == 3) {
			buyin_messenge_display.gameObject.SetActive (false);
			sellout_messenge_display.gameObject.SetActive (false);
			confirm_messenge_display.gameObject.SetActive (true);

			Confirm.interactable = true;
		}
		if (mode == 4) {
			buyin_messenge_display.gameObject.SetActive (false);
			sellout_messenge_display.gameObject.SetActive (false);
			confirm_messenge_display.gameObject.SetActive (true);

			Confirm.interactable = true;
		}
	}

	public void Instant_All_Update(){
		Network_Update MyNetScript = MyNet.GetComponent<Network_Update>();
		MyNetScript.UpdateConnection (Name.text, time_idx);

		All_Update.image.color = All_Update.colors.normalColor;
		}

	public void Instant_Stock_Update(){
		Network_Update MyNetScript = MyNet.GetComponent<Network_Update>();
		MyNetScript.UpdateConnection_Stock_Only (Name.text, time_idx);

		Stock_Update.image.color = Stock_Update.colors.normalColor;
	}
}
