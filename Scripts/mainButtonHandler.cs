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
using System.Collections.Generic;

public class mainButtonHandler : MonoBehaviour {
	//private List<string> currentItemListNames = new List<string> ();
	//private List<string> currentItemListCosts = new List<string> ();
	private string costOverviewSceneName;

	// Use this for initialization
	void Start () {
		costOverviewSceneName = DataControl.control.getOverviewSceneName();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//This action will load the "Add Item" Scene
	public void startAddItemScene (){
		//SceneManager.LoadScene (addItemSceneName);
		//mainScript2.control.startAddItemScene ();
		mainScript3.control.startAddItemScene ();
	}

	public void clearItemsInList(){
		//mainScript2.control.clearAllItemsInList ();
		mainScript3.control.clearAllItemsInList ();
	}

	public void modifyCurrentItem(){
		//bool isAnotherItemBeingModified = mainScript2.control.getCurrentModificationSate ();
		bool isAnotherItemBeingModified = mainScript3.control.getCurrentModificationSate ();
		if (!isAnotherItemBeingModified) {//No other item is being modified
			//Get item ROW position
			GameObject currentitemRow = this.transform.parent.gameObject;
			int currentItemIndex = currentitemRow.transform.GetSiblingIndex ();
			//mainScript2.control.setCurrentItemIdexNum (currentItemIndex);
			mainScript3.control.setCurrentItemIdexNum (currentItemIndex);

			//Hide this item in the mean time
			currentitemRow.gameObject.SetActive (false);
		}

	}

	public void deleteCurrentItem(){
		//mainScript2.control.deleteCurrentItem ();
		mainScript3.control.deleteCurrentItem ();
	}

	//This lets the user cancel their modification action and do nothing.
	public void cancelItemModification(){
		//mainScript2.control.cancelDoNothingAndReturnListBackToOrignalDisplayState ();
		mainScript3.control.cancelDoNothingAndReturnListBackToOrignalDisplayState ();
	}

	public void startEditScene(){
		//mainScript2.control.startEditItemScene ();
		mainScript3.control.startEditItemScene ();
	}

	public void startResultsScene(){
		Scene currentScene=  SceneManager.GetActiveScene ();
		string currentSceneName = currentScene.name;
		//mainScript2.control.startTotalItemCostOverviewScene ();

		Debug.Log ("Current Scene name: " + currentSceneName);
		Debug.Log ("Compared to: " + DataControl.control.getAddItemSceneName());
		if(currentSceneName.Equals (DataControl.control.getOverviewSceneName ())){
			Debug.Log ("This is the Overview scene");
			mainScriptOverview.control.startOverviewScene ();
		} else{
			//Debug.Log ("Scene Name " + mainScript3.control.startTotalItemCostOverviewScene ());
			//mainScript3.control.startTotalItemCostOverviewScene ();
			SceneManager.LoadScene (costOverviewSceneName);
		}

	}

	public void startListScene(){
		Scene currentScene=  SceneManager.GetActiveScene ();
		string currentSceneName = currentScene.name;

		Debug.Log ("Current Scene name: " + currentSceneName);
		Debug.Log ("Compared to: " + DataControl.control.getAddItemSceneName());
		if (currentSceneName.Equals (DataControl.control.getAddItemSceneName ())) {
			Debug.Log ("This is the Add Item scene");
			mainScriptAdd.control.showList ();
		} else if(currentSceneName.Equals (DataControl.control.getOverviewSceneName ())){
			Debug.Log ("This is the Overview scene");
			mainScriptOverview.control.startListScene ();
		} 
		else if(currentSceneName.Equals (DataControl.control.getEditItemSceneName ())){
			Debug.Log ("This is the Edit Item scene");
		}
		else {
			mainScript3.control.startListScene ();
		}
	}
}
