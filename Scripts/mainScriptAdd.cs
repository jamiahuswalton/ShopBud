/*
Licensed to the Apache Software Foundation (ASF) under one
or more contributor license agreements.  See the NOTICE file
distributed with this work for additional information
regarding copyright ownership.  The ASF licenses this file
to you under the Apache License, Version 2.0 (the
"License"); you may not use this file except in compliance
with the License.  You may obtain a copy of the License at

  http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing,
software distributed under the License is distributed on an
"AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, either express or implied.  See the License for the
specific language governing permissions and limitations
under the License.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
//using ItemSerial;

public class mainScriptAdd : MonoBehaviour {
	public static mainScriptAdd control;
	public InputField itemName;
	public InputField itemCost;

	public GameObject userMessageObject;

	//private List<string> myItemNames;
	//private List<string> myItemCosts;
	private List<Item> myListOfItems = new List<Item> ();

	private Text userMessageText;

	private string listSceneName;

	private float userDelayTime = 2.0f;
	private float currentUserDelayTime = 0;

	private bool isDisplayingUserMessage = false;

	void Awake(){
		if(control == null){
			DontDestroyOnLoad (gameObject);
			control = this;
		}
		else if(control != this){
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		DataControl.control.Load ();

		//Load Scene Names from data controls
		listSceneName = DataControl.control.getListSceneName ();

		//myItemNames = DataControl.control.getItemNames ();
		//myItemCosts = DataControl.control.getItemCosts ();
		myListOfItems = DataControl.control.getListOfItems ();

		//Get the error message component
		userMessageText = userMessageObject.GetComponent <Text>();

		//Clear current user message
		//userMessageText.text = "";

	}
	
	// Update is called once per frame
	void Update () {
		if (isDisplayingUserMessage) {
			currentUserDelayTime -= Time.deltaTime;
			if (currentUserDelayTime < 0) {
				isDisplayingUserMessage = false;
			}
		} else if(!isDisplayingUserMessage){
			userMessageText.text = "";
			currentUserDelayTime = userDelayTime;
		}
	}

	public void showList(){
		SceneManager.LoadScene (listSceneName);
	}

	public void clearNewItemInput(){
		//Remove the curent text
		itemName.text = "";
		itemCost.text = "";
	}

	public void saveNewItemInput(){
		//Check to see if text is empty
		string itemNameCheck = itemName.text;
		string itemCostCheck = itemCost.text;

		//Remove any extra spaces from name and cost
		itemNameCheck = itemNameCheck.Replace (" ", "");
		itemCostCheck = itemCostCheck.Replace (" ", "");

		if (!itemNameCheck.Equals ("") && !itemCostCheck.Equals ("") && !itemCostCheck.Equals (".")) { //This check for valid entry
			//Debug.Log ("Name or Cost is empty");
			//Get the current list
			//myItemNames.Add (itemName.text);
			//myItemCosts.Add (itemCost.text);
			Item newItem = new Item (itemName.text, float.Parse (itemCost.text));
			myListOfItems.Add (newItem);

			//DataControl.control.setItemNames (myItemNames);
			//DataControl.control.setItemCosts (myItemCosts);
			DataControl.control.setListOfItems (myListOfItems);

			//Save Data
			DataControl.control.Save ();

			//Debugging
			List<Item> test = DataControl.control.getListOfItems ();
			for (int i = 0; i < DataControl.control.getListOfItems ().Count; i++) {
				Item currentItem = test [i];
				Debug.Log ("Names: " + currentItem.getItemName());
				Debug.Log ("Cost: " + currentItem.getItemCost());
			}

			//Tell user data was saved
			userMessageText.text = "Item was Saved!";
			//Allow user message to display for some time
			isDisplayingUserMessage = true;
			/*
			string MessageToSend = "Item was Saved!";
			float PopUpDuration = 3.0f;
			DisplayPopUpMessage (MessageToSend, PopUpDuration);
			*/

			//Remove the curent text
			itemName.text = "";
			itemCost.text = "";

		} else if (itemNameCheck.Equals ("")) { // This checks to make sure there is an actual name
			//A valid item name is needed
			userMessageText.text = "Please enter valid user name!";
			/*
			string MessageToSend = "Please enter item name!";
			float Duration = 3.0f;
			DisplayPopUpMessage (MessageToSend, Duration);
			*/
		} else if (itemCostCheck.Equals ("")) { //This check to make sure the cost is not empty
			userMessageText.text = "Please cost of item!";
			/*
			string MessageToSend = "Please enter item cost!";
			float Duration = 3.0f;
			DisplayPopUpMessage (MessageToSend, Duration);
			*/
		}else if(itemCostCheck.Equals (".")){ //This check to make sure there is more than one decimal
			userMessageText.text = "Please enter a valid item cost!";
			/*
			string MessageToSend = "Please enter valid number!";
			float Duration = 3.0f;
			DisplayPopUpMessage (MessageToSend, Duration);
			*/
		}
	}
}
