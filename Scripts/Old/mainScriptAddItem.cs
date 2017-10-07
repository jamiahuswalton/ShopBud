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

public class mainScriptAddItem : MonoBehaviour {

	public InputField itemName;
	public InputField itemCost;
	public GameObject saveVerification;
	public GameObject ValidationButtons;
	public GameObject mainButtons;

	/*
	private List<string> myItemNames;
	private List<string> myItemCosts;
	*/
	private List<Item> myListofItems;

	private string PopUpMessage;
	private bool showPopUpMessage = false; //True = Show message
	private Text PopUpMessageText;
	private float PopUpMessageTimer; //This tell how long the message will show
	private bool readyToClearList = false; //True = ready to clear list, False = Now ready to clear list and need to make sure
	private bool readyToShowList = false;
	private string validationMessage;
	private Text validationText;
	private bool checkClearList = false; //false = not checking clearlist, true = currently checking clear list verification
	private bool checkContinueToShowList = false; // false = not checking if list should be shown, true = currently checking if list should be shown

	// Use this for initialization
	void Start () {

		//turn off validation buttons
		ValidationButtons.SetActive (false);
		
		DataControl.control.Load ();
		/*
		myItemNames = DataControl.control.getItemNames ();
		myItemCosts = DataControl.control.getItemCosts ();
		*/
		myListofItems = DataControl.control.getListOfItems ();

		//Text component on Save Verification
		PopUpMessageText = saveVerification.GetComponent <Text>();

		//Get the text of the validation title
		validationText = ValidationButtons.transform.GetChild (0).GetComponent <Text> ();
	}
	
	// Update is called once per frame
	void Update () {

		//Show Saved Verification
		if (showPopUpMessage) {
			saveVerification.SetActive (true);
			PopUpMessageText.text = PopUpMessage;
			PopUpMessageTimer -= Time.deltaTime;
			if (PopUpMessageTimer < 0f) {
				showPopUpMessage = false;
			}

		} else {
			saveVerification.SetActive (false);
		}
	}

	void OnGUI(){
		
	}

	public void saveItem(){
		Debug.Log ("Item Name: " + itemName.text);
		Debug.Log ("Item Cost: " + itemCost.text);
		Debug.Log (itemCost.text.GetType ());

		//Check to see if text is empty
		string itemNameCheck = itemName.text;
		string itemCostCheck = itemCost.text;

		itemNameCheck = itemNameCheck.Replace (" ", "");
		itemCostCheck = itemCostCheck.Replace (" ", "");

		if (!itemNameCheck.Equals ("") && !itemCostCheck.Equals ("") && !itemCostCheck.Equals (".")) { //This check for valid entry
			//Debug.Log ("Name or Cost is empty");
			//Get the current list and add items
			/*
			myItemNames.Add (itemName.text);
			myItemCosts.Add (itemCost.text);
			*/
			Item newItem = new Item (itemName.text, float.Parse(itemCost.text));
			myListofItems.Add (newItem);
			/*
			DataControl.control.setItemNames (myItemNames) ;
			DataControl.control.setItemCosts (myItemCosts) ;
			*/
			DataControl.control.setListOfItems (myListofItems);

			//Save Data
			DataControl.control.Save ();

			//for (int i = 0; i < DataControl.control.getItemNames ().Count; i++) {
			for (int i = 0; i < DataControl.control.getListOfItems ().Count; i++) {
				Debug.Log ("Names: " + DataControl.control.getListOfItems () [i].getItemName());
				Debug.Log ("Cost: " + DataControl.control.getListOfItems () [i].getItemCost());
			}

			//Tell user data was saved
			string MessageToSend = "Item was Saved!";
			float PopUpDuration = 3.0f;
			DisplayPopUpMessage (MessageToSend, PopUpDuration);

			//Remove the curent text
			itemName.text = "";
			itemCost.text = "";
		} else if (itemNameCheck.Equals ("")) { // This checks to make sure there is an actual name
			string MessageToSend = "Please enter item name!";
			float Duration = 3.0f;
			DisplayPopUpMessage (MessageToSend, Duration);
		} else if (itemCostCheck.Equals ("")) { //This check to make sure the cost is not empty
			string MessageToSend = "Please enter item cost!";
			float Duration = 3.0f;
			DisplayPopUpMessage (MessageToSend, Duration);
		}else if(itemCostCheck.Equals (".")){ //This check to make sure there is more than one decimal
			string MessageToSend = "Please enter valid number!";
			float Duration = 3.0f;
			DisplayPopUpMessage (MessageToSend, Duration);
		}
	}

	public void clearItemList(){
		if (!readyToClearList) {
			//Turn off main buttons
			mainButtons.SetActive (false);
			//Turn on validation buttons
			ValidationButtons.SetActive (true);
			validationMessage = "Are you sure you want to clear list?";
			validationText.text = validationMessage;

			//Validating clear list
			checkClearList = true;

		} else if (readyToClearList) {
			//Clear list
			//myItemNames.Clear ();
			//myItemCosts.Clear ();
			myListofItems.Clear ();

			//Save lists
			/*
			DataControl.control.setItemNames (myItemNames);
			DataControl.control.setItemCosts (myItemCosts);
			*/
			DataControl.control.setListOfItems (myListofItems);

			//Save Data
			DataControl.control.Save ();

			//Tell user list was cleared
			string MessageToSend = "List was cleared!";
			float PopUpDuration = 3.0f;
			DisplayPopUpMessage (MessageToSend, PopUpDuration);

			//Reset everything
			readyToClearList = false;
			checkClearList = false;
		}
	}

	public void yesClearList(){
		if (checkClearList) {
			readyToClearList = true;
			clearItemList ();

			//Reset things
			checkClearList = false;
		}
		else if(checkContinueToShowList){
			readyToShowList = true;
			checkContinueToShowList = false;
			showList ();
		}
	}

	public void noClearList(){

		if (checkClearList) {
			//Turn on main buttons
			mainButtons.SetActive (true);
			//Turn of validation buttons
			ValidationButtons.SetActive (false);

			//Reset everything
			checkClearList = false;
		}
		else if(checkContinueToShowList){
			//Turn on main buttons
			mainButtons.SetActive (true);
			//Turn of validation buttons
			ValidationButtons.SetActive (false);

			//Reset everything
			checkContinueToShowList = false;
		}
	}

	public void showList(){
		//Check to see if text is empty
		string itemNameCheck = itemName.text;
		string itemCostCheck = itemCost.text;

		itemNameCheck = itemNameCheck.Replace (" ", "");
		itemCostCheck = itemCostCheck.Replace (" ", "");

		if (!itemNameCheck.Equals ("") && !itemCostCheck.Equals ("") && !itemCostCheck.Equals (".")) {
			//There is currenly an entry that is not saved

			//Let user know that the item was not saved
			string MessageToSend = "Current item not saved";
			float Duration = 3.0f;
			DisplayPopUpMessage (MessageToSend, Duration);

			if(!readyToShowList){
				//Turn off main buttons
				mainButtons.SetActive (false);
				//Turn on validation buttons
				ValidationButtons.SetActive (true);
				validationMessage = "Item not saved, item will be lost. Continue?";
				validationText.text = validationMessage;

				//Validating clear list
				checkContinueToShowList = true;
			}
			else if(readyToShowList){
				readyToShowList = false;
				//This will send user to list screen
				SceneManager.LoadScene ("itemList");
			}
		}
		else{
			//This will send user to list screen
			SceneManager.LoadScene ("itemList");
		}
	}

	private void DisplayPopUpMessage(string Message, float Duration){
		PopUpMessage = Message;
		PopUpMessageTimer = Duration;
		showPopUpMessage = true;
	}
}
