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
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class EditHandlerItemName : MonoBehaviour {

	public GameObject EditDeleteButton;
	public GameObject Row;

	private GameObject Content;
	private string EditItemSceneName;
	private GameObject EditDelete;
	private GameObject rowObject;
	//private List<string> myItemNames;
	//private List<string> myItemCosts;
	private List<Item> myListOfItems;

	// Use this for initialization
	void Start () {
		EditItemSceneName = DataControl.control.getEditItemSceneName ();
		DataControl.control.Load ();
		Content = GameObject.FindGameObjectWithTag ("content");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void makeEdit(){

		bool changesAlreadyBeingMade = DataControl.control.itemIsBeingChanged;
		if (!changesAlreadyBeingMade) {
			//Debug.Log ("Changes not being made");
			//Get parent sibling index number. The parent should be the row object
			rowObject = this.transform.parent.gameObject;
			int IndexOfRow = rowObject.transform.GetSiblingIndex ();
			//Debug.Log ("Index: " + IndexOfRow);

			//Send the index of the item that needs to be changed
			DataControl.control.itemToEditIndexNumber = IndexOfRow;

			//Disable Row
			Destroy (rowObject);

			//Display EditDelete Row
			EditDelete = (GameObject)Instantiate (EditDeleteButton, Vector3.zero, Quaternion.identity);


			EditDelete.transform.SetParent (Content.transform);
			EditDelete.transform.SetSiblingIndex (DataControl.control.itemToEditIndexNumber);
			EditDelete.transform.localScale = new Vector3 (1,1,1);

			DataControl.control.Save ();
			DataControl.control.itemIsBeingChanged = true;
		}
	}


	public void cancelEverything(){

		Content = GameObject.FindGameObjectWithTag ("content");
		/*
		myItemNames = DataControl.control.getItemNames ();
		myItemCosts = DataControl.control.getItemCosts ();
		*/
		myListOfItems = DataControl.control.getListOfItems ();
		//Item Index
		int Index = DataControl.control.itemToEditIndexNumber;

		//This Destroy EditDelete buttons
		GameObject EditButtonToDestroy = Content.transform.GetChild (Index).gameObject;
		Destroy (EditButtonToDestroy);

		//Display Rows
		GameObject newRow = (GameObject)Instantiate (Row, Vector3.zero, Quaternion.identity);


		//---- Get the "Button-ItemName" and then get the "Text" of that button
		Text firstText =  newRow.transform.GetChild (0).transform.GetChild (0).GetComponent <Text>();
		//----Set the text for item name
		//firstText.text = myItemNames[Index];
		firstText.text = myListOfItems[Index].getItemName();

		//---- Get the "Button-ItemCost" and then get the "Text" of that button
		Text secondText =  newRow.transform.GetChild (1).transform.GetChild (0).GetComponent <Text>();
		//----Set the text for item name
		//secondText.text = myItemCosts[Index];
		secondText.text = myListOfItems [Index].getItemCost ().ToString ();

		newRow.transform.SetParent (Content.transform);
		newRow.transform.SetSiblingIndex (Index);
		newRow.transform.localScale = new Vector3 (1,1,1);

		//Allow other changes to be made
		DataControl.control.itemIsBeingChanged = false;
	}


	public void startEditScene(){		
		SceneManager.LoadScene (EditItemSceneName);
		DataControl.control.itemIsBeingChanged = false;
	}

	public void deleteItem(){
		Content = GameObject.FindGameObjectWithTag ("content");
		//myItemNames = DataControl.control.getItemNames();
		//myItemCosts = DataControl.control.getItemCosts();
		myListOfItems = DataControl.control.getListOfItems ();
		int Index = DataControl.control.itemToEditIndexNumber;
		//Get the row to delete item
		GameObject RowToDelete = Content.transform.GetChild (Index).gameObject;

		//remove items
		/*
		myItemCosts.RemoveAt (Index);
		myItemNames.RemoveAt (Index);
		*/
		myListOfItems.RemoveAt (Index);

		//Destroy the current row
		Destroy (RowToDelete);

		DataControl.control.itemIsBeingChanged = false;
	}
}
