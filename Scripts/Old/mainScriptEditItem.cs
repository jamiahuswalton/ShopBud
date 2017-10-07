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

public class mainScriptEditItem : MonoBehaviour {

	public InputField itemName;
	public InputField itemCost;
	public GameObject saveVerification;
	/*
	private List<string> myItemNames;
	private List<string> myItemCosts;
	*/
	private List<Item> myListofItems;

	private int IndexOfItemToChange;
	private bool showPopUpMessage = false; //True = Show message
	private Text PopUpMessageText;
	private string PopUpMessage;
	private float PopUpMessageTimer; //This tell how long the message will show

	// Use this for initialization
	void Start () {
		DataControl.control.Load ();

		//Get the list data
		/*
		myItemNames = DataControl.control.getItemNames ();
		myItemCosts = DataControl.control.getItemCosts ();
		*/
		myListofItems = DataControl.control.getListOfItems ();
		
		IndexOfItemToChange = DataControl.control.itemToEditIndexNumber;

		//Set the current text
		/*
		itemName.text = myItemNames[IndexOfItemToChange];
		itemCost.text = myItemCosts [IndexOfItemToChange];
		*/
		itemName.text = myListofItems [IndexOfItemToChange].getItemName ();
		itemCost.text = myListofItems [IndexOfItemToChange].getItemCost ().ToString();

		//Set the text Component
		PopUpMessageText = saveVerification.GetComponent <Text>();
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
				//Send back to list
				SceneManager.LoadScene ("itemList");
			}

		} else {
			saveVerification.SetActive (false);
		}
	}

	public void saveChanges(){
		
		//Similar to the save method in mainScriptAddItem
		Debug.Log ("Item Name: " + itemName.text);
		Debug.Log ("Item Cost: " + itemCost.text);
		/*
		myItemNames [IndexOfItemToChange] = itemName.text;
		myItemCosts [IndexOfItemToChange] = itemCost.text;
		*/
		myListofItems [IndexOfItemToChange].setItemName (itemName.text);
		myListofItems [IndexOfItemToChange].setItemCost (itemCost.text);

		//Debug.Log ("New Item: " + myItemNames[IndexOfItemToChange] + ", " + myItemCosts[IndexOfItemToChange]);
		/*
		DataControl.control.setItemNames (myItemNames) ;
		DataControl.control.setItemCosts (myItemCosts) ;
		*/
		DataControl.control.setListOfItems (myListofItems);

		//Show message verification
		string MessageToDisplay = "Changes Saved";
		float Duration = 2.0f;
		DisplayPopUpMessage (MessageToDisplay,Duration);

		//Save Data
		DataControl.control.Save ();

		itemName.interactable = false;
		itemCost.interactable = false;
	}

	private void DisplayPopUpMessage(string Message, float Duration){
		PopUpMessage = Message;
		PopUpMessageTimer = Duration;
		showPopUpMessage = true;
	}
}
