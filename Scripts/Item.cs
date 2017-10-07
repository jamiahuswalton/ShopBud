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

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;

[Serializable]
public class Item {
	//[SerializeField]
	private string itemName;
	//[SerializeField]
	private float itemCost;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}



	public Item (string myItemName, float myItemCost){
		itemName = myItemName;
		itemCost = myItemCost;
	}

	public string getItemName (){
		return itemName;
	}

	public float getItemCost (){
		return itemCost;
	}

	public void setItemName(string newName){
		itemName = newName;
	}

	public void setItemCost(string newCost){
		itemCost = float.Parse (newCost);
	}
}