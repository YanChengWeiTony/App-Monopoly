using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
//using LitJson;
using System;

public class Network_Update : MonoBehaviour {

	private const string myurl = "http://davidbrother.pythonanywhere.com/";

	//other game objects
	public GameObject[] mystock;
	public GameObject Buyin_yes;
	public GameObject Buyin_no;
	public GameObject Sellout_yes;
	public GameObject Sellout_no;

	//Text
	public Text timer;
	public Text Total_Asset_digit;
	public Text Cash_digit;
	public Text Bank_digit;
	public Text Estate_digit;
	public Text Stock_digit;
	public Text Knife_digit;
	public Text[] Land;
	public Text confirm_messenger;

	//int
	public int Total_Asset;
	public int Cash;
	public int Bank;
	public int Estate;
	public int Stock;
	public int Knife;

	public int[] LandPrice;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Connection(string str_name, int time_idx){
		StartCoroutine (MyList (str_name, "request-periodic-update-bank-money/", Bank_digit));
		StartCoroutine (MyList (str_name, "request-pocket-money/", Cash_digit));
		StartCoroutine (MyList (str_name, "request-knives/", Knife_digit));

		Estate = 0;
		for (int i = 1; i <= 18; i++) {
			StartCoroutine (MyLandList (str_name, i, Land));
		}
			
		for (int i = 1; i <= mystock.Length - 1; i++) {
			Text[] buffer = mystock [i].GetComponentsInChildren<Text> ();

			StartCoroutine (MyStockAmountList(str_name, i, buffer[3]));
			StartCoroutine(MyStockValueList(i, buffer[2]));
			StartCoroutine(MyStockSlopeList(i, time_idx, buffer[1]));
		}
	}

	public void UpdateConnection(string str_name, int time_idx){
		StartCoroutine (MyList (str_name, "request-bank-money/", Bank_digit));
		StartCoroutine (MyList (str_name, "request-pocket-money/", Cash_digit));
		StartCoroutine (MyList (str_name, "request-knives/", Knife_digit));

		Estate = 0;
		for (int i = 1; i <= 18; i++) {
			StartCoroutine (MyLandList (str_name, i, Land));
		}

		for (int i = 1; i <= mystock.Length - 1; i++) {
			Text[] buffer = mystock [i].GetComponentsInChildren<Text> ();

			StartCoroutine (MyStockAmountList(str_name, i, buffer[3]));
			StartCoroutine(MyStockValueList(i, buffer[2]));
			StartCoroutine(MyStockSlopeList(i, time_idx, buffer[1]));
		}
	}

	public void UpdateConnection_Stock_Only(string str_name, int time_idx){
		for (int i = 1; i <= mystock.Length - 1; i++) {
			Text[] buffer = mystock [i].GetComponentsInChildren<Text> ();

			StartCoroutine (MyStockAmountList(str_name, i, buffer[3]));
			StartCoroutine(MyStockValueList(i, buffer[2]));
			StartCoroutine(MyStockSlopeList(i, time_idx, buffer[1]));
		}
	}

	public void Buy_Stock_Connection(string str_name, string pk, string number){
		confirm_messenger.text = "連線中...";
		StartCoroutine (MyBuyStockList(str_name, pk, number));
	}

	public IEnumerator MyList(String str_name, String request, Text mytext){
		string url = myurl + request + str_name ;

		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();


		if (www.isError) {
			//GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("網路不穩定\n請重新確認網路連線！", false);
			//Debug.Log(www.error);
		} else {
			mytext.text = www.downloadHandler.text;
		}
	}

	public IEnumerator MyLandList(String str_name, int idx, Text[] Land){
		string pk = Convert.ToString (idx);
		string url = myurl + "request-lands/" + str_name + "/" + pk;

		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();

		if (www.isError) {
			//GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("網路不穩定\n請重新確認網路連線！", false);
			//Debug.Log(www.error);
		} else {
			Land[idx].text = www.downloadHandler.text;
			if (Char.IsDigit(Land[idx].text[0]) == true) {
				Estate += (int)((double)LandPrice [idx] + ((double)Convert.ToUInt32 (Land [idx].text)) * (double)LandPrice [idx] * 0.4f);
			}
		}

		Estate_digit.text = String.Format ("{0:D}", Estate);
	}

	public IEnumerator MyStockAmountList(String str_name, int idx, Text mytext){
		string pk = Convert.ToString (idx);
		string url = myurl + "request-stock-amount/" + pk + "/" + str_name;

		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();

		if (www.isError) {
			//GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("網路不穩定\n請重新確認網路連線！", false);
			//Debug.Log(www.error);
		} else {
			mytext.text = www.downloadHandler.text;
		}
	}

	public IEnumerator MyStockValueList(int idx, Text mytext){
		string pk = Convert.ToString (idx);
		string url = myurl + "request-stock-value/" + pk;

		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();

		if (www.isError) {
			//GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("網路不穩定\n請重新確認網路連線！", false);
			//Debug.Log(www.error);
		} else {
			mytext.text = www.downloadHandler.text;
		}
	}

	public IEnumerator MyStockSlopeList(int idx, int time_idx, Text mytext){
		string pk = Convert.ToString (idx);
		string str_time_idx = Convert.ToString (time_idx);
		string url = myurl + "request-stock-last-risefall/" + pk + "/" + str_time_idx;

		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();

		if (www.isError) {
			//GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("網路不穩定\n請重新確認網路連線！", false);
			//Debug.Log(www.error);
		} else {
			mytext.text = www.downloadHandler.text + "%";
		}
	}

	public IEnumerator MyBuyStockList(string str_name, string pk, string number){
		string url = myurl + "sell-stock/" + str_name + "/" + pk + "/" + number;

		print (url);
	
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();

		if (www.isError) {
			//GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("網路不穩定\n請重新確認網路連線！", false);
			//Debug.Log(www.error);
		} else {
			confirm_messenger.text = www.downloadHandler.text;
		}
	}
}
