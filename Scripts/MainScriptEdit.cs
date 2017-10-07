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

public class MainScriptEdit : MonoBehaviour {
	public static MainScriptEdit control;
	public InputField itemName;
	public InputField itemCost;

	//private List<string> myItemNames;
	//private List<string> myItemCosts;
	private List<Item> myListofItems;

	public GameObject userMessageObject;

	private string listSceneName; 

	private int itemToEditIndex;

	private Text userMessageText;

	private bool delayStartShowList = false;

	private float timeForStartDelay = 3.0f;

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

		//Get current item lists
		//myItemNames = DataControl.control.getItemNames ();
		//myItemCosts = DataControl.control.getItemCosts ();
		myListofItems = DataControl.control.getListOfItems();


		//Get index number of item to change
		itemToEditIndex = DataControl.control.itemToEditIndexNumber;

		//Populate input field for Item name and cost
		Item currentItem = myListofItems[itemToEditIndex];
		itemName.text = currentItem.getItemName();
		itemCost.text = currentItem.getItemCost ().ToString();
		//itemName.text = myItemNames[itemToEditIndex];
		//itemCost.text = myItemCosts [itemToEditIndex];

		//Get the error message component
		userMessageText = userMessageObject.GetComponent <Text>();
	}
	
	// Update is called once per frame
	void Update () {

		//This will add a delay before showing list
		if (delayStartShowList) {
			timeForStartDelay -= Time.deltaTime;
			//Debug.Log (timeForStartDelay);
			if (timeForStartDelay <= 0) {
				SceneManager.LoadScene (listSceneName);
			}
		}
	}

	//Update Item
	public void updateCurrentItemInformation(){
		//Check to see if text is empty
		string itemNameCheck = itemName.text;
		string itemCostCheck = itemCost.text;

		//Remove any extra spaces from name and cost
		itemNameCheck = itemNameCheck.Replace (" ", "");
		itemCostCheck = itemCostCheck.Replace (" ", "");

		//Validation - Making sure the input is valid
		if (!itemNameCheck.Equals ("") && !itemCostCheck.Equals ("") && !itemCostCheck.Equals (".")) { //This check for valid entry
			
			//Update current Item
			myListofItems[itemToEditIndex].setItemName(itemName.text);
			myListofItems [itemToEditIndex].setItemCost (itemCost.text);
			//myItemNames[itemToEditIndex] = itemName.text;
			//myItemCosts [itemToEditIndex] = itemCost.text;

			//DataControl.control.itemNames = myItemNames;
			//DataControl.control.itemCosts = myItemCosts;
			//DataControl.control.setItemNames (myItemNames);
			//DataControl.control.setItemCosts (myItemCosts);
			DataControl.control.setListOfItems (myListofItems);

			//Save Data
			DataControl.control.Save ();

			//Turn off intput fields
			itemName.interactable = false;
			itemCost.interactable = false;

			//Tell user data was saved
			userMessageText.text = "Item was Updated!";
			delayStartShowList = true;

		} else if (itemNameCheck.Equals ("")) { // This checks to make sure there is an actual name
			//A valid item name is needed
			userMessageText.text = "Please enter valid user name!";

		} else if (itemCostCheck.Equals ("")) { //This check to make sure the cost is not empty
			userMessageText.text = "Please cost of item!";
		
		}else if(itemCostCheck.Equals (".")){ //This check to make sure there is more than one decimal
			userMessageText.text = "Please enter a valid item cost!";

		}
	}
}
