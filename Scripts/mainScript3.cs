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
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
//using ItemSerial;

public class mainScript3 : MonoBehaviour {

	public static mainScript3 control;
	public GameObject ValidationButtons;
	public GameObject contentMainParent;
	public GameObject itemRow;
	public GameObject addItemButton;
	public GameObject EditOrDeleteOrCancelRow;
	public GameObject showResultsRow;
	private GameObject newEditDeleteCancelRow;

	private int contentItemCount;
	private int itemNameChildLocationInNewRow = 0;
	private int itemCostChildLocationInNewRow = 1;
	private int currentItemIndex = 0;

	//private List<String> myItemNames = new List<string>();
	//private List<String> myItemCosts = new List<string>();
	private List<Item> myListOfItems = new List<Item> ();

	private string addItemSceneName;
	private string editItemSceneName;
	private string overviewSceneName;
	private string listItemSceneName;

	private bool isBeingModified = false;

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


		Debug.Log ("List Scene Name: " + DataControl.control.getListSceneName ());
		//DataControl.control.Load ();
		//Load Scene Names from data controls
		addItemSceneName = DataControl.control.getAddItemSceneName ();
		editItemSceneName = DataControl.control.getEditItemSceneName ();
		overviewSceneName = DataControl.control.getOverviewSceneName ();
		listItemSceneName = DataControl.control.getListSceneName ();
		//Disable Validation buttons
		//ValidationButtons.SetActive (false);


		buildItemList ();

		//Test
		Item test = new Item("Test", 5.67f);
		Debug.Log (test.getItemName());
	}
	
	// Update is called once per frame
	void Update () {
		/*
		timerTest -= .1f;
		if (timerTest < 0) {
			buildItemList ();
		}
		*/
	}

	public void startAddItemScene(){
		SceneManager.LoadScene (addItemSceneName);
	}

	public void startEditItemScene(){
		DataControl.control.itemToEditIndexNumber = currentItemIndex;
		SceneManager.LoadScene (editItemSceneName);
	}

	public void startListScene(){
		SceneManager.LoadScene (listItemSceneName);
	}

	public void startTotalItemCostOverviewScene(){
		Debug.Log ("Test, Scene Name: " + overviewSceneName);
		SceneManager.LoadScene (overviewSceneName);
	}

	public void clearAllItemsInList(){
		//Clear list
		//myItemNames.Clear ();
		//myItemCosts.Clear ();
		myListOfItems.Clear ();

		//Save lists
		//DataControl.control.setItemNames (myItemNames);
		//DataControl.control.setItemCosts (myItemCosts);
		DataControl.control.setListOfItems (myListOfItems);

		//Save Data
		DataControl.control.Save ();

		//rebuild Item List
		buildItemList ();

		//Tell user list was cleared
		/*
			string MessageToSend = "List was cleared!";
			float PopUpDuration = 3.0f;
			*/
	}

	public void deleteCurrentItem(){
		//Delete the specific item from the list
		//myItemNames.RemoveAt (currentItemIndex);
		//myItemCosts.RemoveAt (currentItemIndex);
		myListOfItems.RemoveAt (currentItemIndex);

		//Update Lists
		//DataControl.control.setItemNames (myItemNames);
		//DataControl.control.setItemCosts (myItemCosts);
		DataControl.control.setListOfItems (myListOfItems);
		//Save
		DataControl.control.Save ();
		//Rebuild the list
		buildItemList ();

		//Done modifying item
		setModificationState (false);
	}
	/*
	public List<string> getMyItemNames(){
		return myItemNames;
	}
	*/
	/*
	public List<string> getMyItemCosts(){
		return myItemCosts;
	}
	*/
	//This will return true if another item is under modification. In other words, the user will only be allowed to modifiy one item as a time.
	public bool getCurrentModificationSate(){
		return isBeingModified;
	}

	private void setModificationState(bool state){
		isBeingModified = state;
	}

	public void cancelDoNothingAndReturnListBackToOrignalDisplayState(){
		//Destroy the modification row
		Destroy (newEditDeleteCancelRow.gameObject);

		//Find the item that is turned off and turn it back on
		for(int y=0; contentMainParent.transform.childCount > y; y++){
			bool stateOfChild = contentMainParent.transform.GetChild (y).gameObject.activeSelf;

			if (!stateOfChild) {
				Debug.Log ("Index: " + y.ToString ());
				contentMainParent.transform.GetChild (y).gameObject.SetActive (true);
				break;
			}
		}

		//Done Modifying item
		setModificationState (false);
		//Debug.Log ("Child Count: " + contentMainParent.transform.childCount.ToString ());
	}

	public void setCurrentItemIdexNum (int index){
		//It is being modified
		setModificationState (true);
		//isBeingModified = true;

		//Set the index of the item we want to change
		currentItemIndex = index;

		//Add the Edit-Delete-Cancel row
		newEditDeleteCancelRow = (GameObject)Instantiate (EditOrDeleteOrCancelRow, Vector3.zero, Quaternion.identity);
		newEditDeleteCancelRow.transform.SetParent (contentMainParent.transform);
		newEditDeleteCancelRow.transform.SetSiblingIndex (currentItemIndex);
		newEditDeleteCancelRow.transform.localScale = new Vector3 (1,1,1);
	}

	public void buildItemList(){
		//Load Data
		DataControl.control.Load ();
		//myItemNames = DataControl.control.getItemNames ();
		//myItemCosts = DataControl.control.getItemCosts ();
		myListOfItems = DataControl.control.getListOfItems ();

		//Get the number of item  (i.e., child count)
		contentItemCount = contentMainParent.transform.childCount;

		//Destroys a  all rows
		for(int i =0; i < contentItemCount; i++){
			Destroy (contentMainParent.transform.GetChild (i).gameObject);
		}

		//Create and display the rows
		for(int x = 0; x < myListOfItems.Count; x++){
			//Display Rows
			GameObject newRow = (GameObject)Instantiate (itemRow, Vector3.zero, Quaternion.identity);
			newRow.transform.SetParent (contentMainParent.transform);
			newRow.transform.localScale = new Vector3 (1,1,1);

			//---- Get the "Button-ItemName" and then get the "Text" of that button
			Text itemNameText = newRow.transform.GetChild (itemNameChildLocationInNewRow).GetChild (0).GetComponent<Text>();

			//---- Get the "Button-ItemCost" and then get the "Text" of that button
			Text itemCostText = newRow.transform.GetChild (itemCostChildLocationInNewRow).GetChild (0).GetComponent<Text>();

			Item currentItem = myListOfItems [x];
			//itemNameText.text = myItemNames [x];
			itemNameText.text = currentItem.getItemName();
			//float formattedCost = float.Parse (myItemCosts [x]); // This is done to make sure there are at least 2 numbers after the decimals
			float formattedCost = currentItem.getItemCost();
			itemCostText.text = "$ " + formattedCost.ToString ("F2");
		}

		//Finally, add the "Add Item" Button
		GameObject newAddItemButton = (GameObject)Instantiate (addItemButton, Vector3.zero, Quaternion.identity);
		newAddItemButton.transform.SetParent (contentMainParent.transform);
		newAddItemButton.transform.SetAsLastSibling ();
		newAddItemButton.transform.localScale = new Vector3 (1, 1, 1);

		//Add the "Show Results" button row
		/*
		GameObject newShowResultsButton = (GameObject)Instantiate (showResultsRow, Vector3.zero, Quaternion.identity);
		newShowResultsButton.transform.SetParent (contentMainParent.transform);
		newShowResultsButton.transform.SetAsLastSibling ();
		newShowResultsButton.transform.localScale = new Vector3 (1, 1, 1);
		*/
	}
}
