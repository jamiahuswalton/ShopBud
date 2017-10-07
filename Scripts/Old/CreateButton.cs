﻿/*
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
using System;

public class CreateButton : MonoBehaviour {

	public GameObject Button_itemName;
	public Canvas MainCanvas;

	private float canvasWidth;
	private float canvasHeight;

	// Use this for initialization
	void Start () {
		RectTransform canvasRectTransform = MainCanvas.GetComponent <RectTransform> ();
		Text myText = Button_itemName.GetComponentInChildren <Text>();
		myText.text = "Width: " + canvasRectTransform.rect.width.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}