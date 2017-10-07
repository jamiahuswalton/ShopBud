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
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class mainScript : MonoBehaviour {

	public static mainScript Control;
	public GameObject ContentFromScrollView;
	public GameObject ScrollView;
	public GameObject Canvas;
	public GameObject Rows;
	public GameObject RowListButton;
	public GameObject clearListButton;
	public GameObject addNewItemButton;
	public GameObject CostButton;
	public GameObject ValidationButtons;

	private int TotalNumberOfCells;
	private RectTransform ContentRectTransform;
	private RectTransform CanvasRectTransform;
	//private float ContentInitialPositionY;
	private ScrollRect ContentRectTransformScrollRect;
	private int VisibalCellNumber;
	private float CellSizeY;
	//private List<GameObject> RowsList = new List<GameObject> ();
	//private List<String> myItemNames = new List<string>();
	//private List<String> myItemCosts = new List<string>();
	private List<Item> myListofItems;
	private int siblingNumberForSelectedRow;
	private bool readyToClearList = false; //True = List is ready to clear, False = Need to make sure before clearing list.

	// Use this for initialization
	void Start () {
		//Disable Validation buttons
		ValidationButtons.SetActive (false);

		//Ad Data
		//AdBuddizBinding.SetAndroidPublisherKey ()

		//Load Data
		DataControl.control.Load ();
		/*
		myItemNames = DataControl.control.getItemNames ();
		myItemCosts = DataControl.control.getItemCosts ();
		*/
		myListofItems = DataControl.control.getListOfItems ();

		//TotalNumberOfCells = Canvas.transform.childCount;
		//Preset the Names and Cost

		//Destroys a  all rows (Except Last row, should be the button

		for(int t = 0; t < ContentFromScrollView.transform.childCount - 1; t++){
			Destroy(ContentFromScrollView.transform.GetChild (t).gameObject);
		}


		//Create and display the rows
		//for(int y = 0; y < myItemNames.Count; y++){
		for(int y = 0; y < myListofItems.Count; y++){
			//Display Rows
			GameObject newRow = (GameObject)Instantiate (Rows, Vector3.zero, Quaternion.identity);

			//---- Get the "Button-ItemName" and then get the "Text" of that button
			Text firstText =  newRow.transform.GetChild (0).transform.GetChild (0).GetComponent <Text>();
			//----Set the text for item name
			//firstText.text = myItemNames[y];
			firstText.text = myListofItems[y].getItemName();

			//---- Get the "Button-ItemCost" and then get the "Text" of that button
			Text secondText =  newRow.transform.GetChild (1).transform.GetChild (0).GetComponent <Text>();
			//----Set the text for item name

			//float formattedCost = float.Parse (myItemCosts[y]); // This is done to make sure there are at least 2 numbers after the decimals 
			float formattedCost = myListofItems[y].getItemCost();
			secondText.text = "$ " + formattedCost.ToString ("F2");

			newRow.transform.SetParent (ContentFromScrollView.transform);
			newRow.transform.localScale = new Vector3 (1,1,1);
		}

		//Set the Add new button to the bottom
		RowListButton.transform.SetAsLastSibling ();

		//Set the bounderies for top and bottom scrolling 
		ContentRectTransform = ContentFromScrollView.GetComponent <RectTransform> ();
		CanvasRectTransform = Canvas.GetComponent <RectTransform> ();

		ContentRectTransform.localPosition = new Vector3(0, 0, 0);

		//ContentInitialPositionY = ContentRectTransform.localPosition.y;

		GridLayoutGroup ContentGridlayout =  ContentFromScrollView.GetComponent <GridLayoutGroup>();
		CellSizeY = ContentGridlayout.cellSize.y;
		Debug.Log ("Cell Size Y: " + CellSizeY);

		float CanvasHeight = CanvasRectTransform.rect.height;
		float numOfVisibalCells = CanvasHeight / CellSizeY;

		VisibalCellNumber = int.Parse (Math.Round (numOfVisibalCells,0).ToString ()) - 3;
		Debug.Log ("Cells on screen: " + (VisibalCellNumber));


		//Set the total number of cells
		//TotalNumberOfCells = myItemNames.Count;
		TotalNumberOfCells = myListofItems.Count;
		Debug.Log ("Total Number of Cells: " + TotalNumberOfCells);
	}

	// Update is called once per frame
	void Update () {

		float current = ContentRectTransform.localPosition.y;
		//float diffrence = current - ContentInitialPositionY;

		/*
		Debug.Log ("Initial y: " + ContentInitialPositionY);
		Debug.Log ("Current: " + current);
		Debug.Log ( " Dif: " + diffrence.ToString ());
		*/

		//Sett scroll bounderies
		if(current < 0){
			//Reset the position of the content zero because you have hit the top bound 
			ContentRectTransform.localPosition = new Vector3(0, 0, 0);
			//Debug.Log ("Hit top ================================================================================================");
		}
		else if(current > (TotalNumberOfCells - VisibalCellNumber) * CellSizeY ){
			ContentRectTransform.localPosition = new Vector3(0, (TotalNumberOfCells - VisibalCellNumber) * CellSizeY, 0);
			//Debug.Log ("Hit bottom ================================================================================================");
		}

		//Lock the position because all cells should be visiable
		if (TotalNumberOfCells < VisibalCellNumber) {
			ContentRectTransform.localPosition = new Vector3(0, 0, 0);
		}

	}

	public void saveItem(){
		//This will start the scene to save an item
		SceneManager.LoadScene ("itemAdd");
	}

	public void clearItemList(){
		//Find the last row (AKA "ListButtons")
		//int ContentChildCount = clearListButton.transform.childCount;


		if(!readyToClearList){ //Need to send to validation

			//Turn off main list buttons
			addNewItemButton.SetActive (false);
			clearListButton.SetActive (false);
			CostButton.SetActive (false);

			//Turn on validation buttons
			ValidationButtons.SetActive (true);
		}
		else if(readyToClearList){
			//Clear list
			/*
			myItemNames.Clear ();
			myItemCosts.Clear ();
			*/
			myListofItems.Clear ();

			//Save lists
			/*
			DataControl.control.setItemNames (myItemNames);
			DataControl.control.setItemCosts (myItemCosts);
			*/
			DataControl.control.setListOfItems (myListofItems);
			//DataControl.control.itemNames = myItemNames;
			//DataControl.control.itemCosts = myItemCosts;

			//Save Data
			DataControl.control.Save ();

			//Tell user list was cleared
			/*
			string MessageToSend = "List was cleared!";
			float PopUpDuration = 3.0f;
			*/
			readyToClearList = false;
		}
	}

	public void yesClearItemList(){
		//Yes, the user is sure. Really clear the lists
		readyToClearList = true;
		clearItemList ();

		//Delete the rows
		//Destroys a  all rows (Except Last row, should be the button

		for(int t = 0; t < ContentFromScrollView.transform.childCount - 1; t++){
			Destroy(ContentFromScrollView.transform.GetChild (t).gameObject);
		}
	}

	public void noClearItemList(){
		//No, the user does not want to clear the list
		//Turn on main list buttons
		addNewItemButton.SetActive (true);
		clearListButton.SetActive (true);
		CostButton.SetActive (true);

		//Turn on validation buttons
		ValidationButtons.SetActive (false);
	}

	public void showResults(){
		//Show the results page
		SceneManager.LoadScene ("itemResults");
	}
}
