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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections.Generic;
//using ItemSerial;

public class DataControl : MonoBehaviour {

	public static DataControl control;

	private List<string> itemNames;
	private List<string> itemCosts;
	private List<Item> listOfItems;

	public int itemToEditIndexNumber;

	public bool itemIsBeingChanged = false; // True = Item is being changed so dont allow any more edits

	//public string listSceneName = "itemList";
	private string listSceneName = "itemList_v2";
	private string addItemSceneName = "itemAdd_v2";
	private string editItemSceneName = "itemEdit_v2";
	private string overviewSceneName = "itemOverview_v2";
	private string DataFilePath = "/listInfo.dat";
	private string previousSalesTax = "0";

	// Use this for initialization
	void Awake () {
		if(control == null){
			DontDestroyOnLoad (gameObject);
			control = this;
		}
		else if(control != this){
			Destroy (gameObject);
		}
	}

	void Start(){
		
	}

	// Update is called once per frame
	void Update () {

	}

	void OnGUI(){
		//GUI.Label (new Rect(10,10,100,30), "High Score:  " + highScore);
	}

	public void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + DataFilePath);

		listData data = new listData ();
		//Save all data items
		data.itemNames = itemNames;
		data.itemCosts = itemCosts;
		data.listOfItems = listOfItems;
		data.previousSalesTax = previousSalesTax;
		//data.HighScore = highScore;

		bf.Serialize (file, data);

		file.Close ();
	}

	public void Load(){
		if(File.Exists (Application.persistentDataPath + DataFilePath)){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + DataFilePath, FileMode.Open);
			listData data = (listData)bf.Deserialize (file);
			file.Close ();

			//Load all data items
			itemNames = data.itemNames;
			itemCosts = data.itemCosts;
			listOfItems = data.listOfItems;
			previousSalesTax = data.previousSalesTax;
		}
	}

	public string getListSceneName(){
		return listSceneName;
	}

	public string getAddItemSceneName(){
		return addItemSceneName;
	}

	public string getEditItemSceneName(){
		return editItemSceneName;
	}

	public string getOverviewSceneName(){
		return overviewSceneName;
	}

	//Need to remove these methods eventually
	/*
	public List<string> getItemNames(){
		if (itemNames == null) {
			itemNames = new List<string>();
		}
		return itemNames;
	}

	public List<string> getItemCosts(){
		if (itemCosts == null) {
			itemCosts = new List<string>();
		}
		return itemCosts;
	}
	*/
	//=====================================

	public List<Item> getListOfItems(){
		if (listOfItems == null) {
			listOfItems = new List<Item> ();
		}
		return listOfItems;
	}

	public string getPreviousSalesTax(){
		return previousSalesTax;
	}

	public void setItemNames(List<string> newItemNameList){
		itemNames = newItemNameList;
	}

	public void setItemCosts(List<string> newItemCostsList){
		itemCosts = newItemCostsList;
	}

	public void setListOfItems(List<Item> newListOfItems){
		listOfItems = newListOfItems;
	}

	public void setPreviousSalesTax(string newPreviousTax){
		previousSalesTax = newPreviousTax;
	}
}



//This class is serialzed when instatiated
[Serializable]
class listData{
	public List<Item> listOfItems;
	public List<string> itemNames;
	public List<string> itemCosts;
	public string previousSalesTax;
}