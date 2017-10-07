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
using UnityEngine.Advertisements;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class mainScriptOverview : MonoBehaviour {
	public static mainScriptOverview control;
	public Text totalCostNoTaxDisplay;
	public Text totalSalesTaxDisplay;
	public Text totalCostWithTaxDisplay;
	public InputField salesTax;

	private float salesTaxPercentageFloat;
	private float totalCostNoTax = 0;
	private float totalSalesTax = 0;
	private float totalCostWithTax = 0;
	private float delayTimeBeforeAdShows = 3;
	private float delayTimer;

	private string listSceneName;
	private string addItemSceneName;
	private string overviewListSceneName;

	private bool runTimer = false;

	//private List<string> myItemNames = new List<string>();
	//private List<string> myItemCosts;
	private List<Item> myListofItems;

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
		listSceneName = DataControl.control.getListSceneName ();
		addItemSceneName = DataControl.control.getAddItemSceneName ();
		overviewListSceneName = DataControl.control.getOverviewSceneName ();

		//Load Data
		DataControl.control.Load ();

		//myItemCosts = DataControl.control.getItemCosts ();
		myListofItems = DataControl.control.getListOfItems ();
		Debug.Log ("Sales Tax: " + DataControl.control.getPreviousSalesTax ());
		if (DataControl.control.getPreviousSalesTax () != null) {
			salesTax.text = DataControl.control.getPreviousSalesTax ();
		} else {
			DataControl.control.setPreviousSalesTax ("0");
		}
		//salesTax.text = DataControl.control.getPreviousSalesTax ();

		//Show add
		//startVideoAd();

		//Delay timer
		delayTimer = delayTimeBeforeAdShows;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(runTimer){
			Debug.Log (delayTimer);
			delayTimer -= Time.deltaTime;

			if (delayTimer <= 0) {
				delayTimer = delayTimeBeforeAdShows;
				runTimer = false;
				Advertisement.Show ("video");
			}
		}
	}

	public void startListScene(){
		SceneManager.LoadScene (listSceneName);
	}

	public void startAddNewItemScene(){
		SceneManager.LoadScene (addItemSceneName);
	}

	public void startOverviewScene(){
		SceneManager.LoadScene (overviewListSceneName);
	}

	public void updateOverviewInformation(){
		string percentString = salesTax.text;
		DataControl.control.setPreviousSalesTax (percentString);
		DataControl.control.Save ();

		salesTaxPercentageFloat = float.Parse (percentString);
		//Debug.Log ("Sales Tax: " + salesTaxPercentageFloat);

		//Reset the total cost amount
		totalCostNoTax = 0;

		//Caculate the total cost of the current items'
		//for (int i = 0; i < myItemCosts.Count; i++) {
		for (int i = 0; i < myListofItems.Count; i++) {
			//Add total cost
			//float currentItemCost = float.Parse (myItemCosts[i]);
			float currentItemCost = myListofItems[i].getItemCost();

			totalCostNoTax += currentItemCost;
		}

		//Find the sales tax
		totalSalesTax = totalCostNoTax * (salesTaxPercentageFloat / 100);

		//Find the total cost when you include the tax
		totalCostWithTax = totalCostNoTax + totalSalesTax;

		//Display Info
		totalCostNoTaxDisplay.text = "$ " + totalCostNoTax.ToString ("F2");
		totalSalesTaxDisplay.text = "$ " + totalSalesTax.ToString ("F2");
		totalCostWithTaxDisplay.text = "$ " + totalCostWithTax.ToString ("F2");
	}

	public void startVideoAd(){
		Debug.Log ("Start Video?");
		/*
		while (!Advertisement.IsReady("video")) {
		}
		*/
		while (!Advertisement.isInitialized) {
		}

		if(Advertisement.IsReady ("video")){
			//Advertisement.Show ("video");
			runTimer = true;
		}

		/*
		if (Advertisement.IsReady ("video")) {
			Advertisement.Show ("video");
		}
		*/
	}
}
