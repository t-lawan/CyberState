﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(ItemCodeDescriptionAttribute))]
public class ItemCodeDescriptionDrawer : PropertyDrawer
{

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		// Change the returned property height to be double to cater for the additional item code description that we will draw
		return EditorGUI.GetPropertyHeight(property) * 2;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		if (property.propertyType == SerializedPropertyType.Integer)
		{
			EditorGUI.BeginChangeCheck(); // Start of check for changed values

			// Draw item code
			var newValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height / 2), label, property.intValue);

			// Draw item description
			EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Item Description", GetItemDescription(property.intValue));


			// If item code value has changed, then set value to new value
			if (EditorGUI.EndChangeCheck())
			{
				property.intValue = newValue;
			}
		}

		EditorGUI.EndProperty();
	}

	private string GetItemDescription(int itemCode)
	{
		SOItemList sOItemList;

		sOItemList = AssetDatabase.LoadAssetAtPath("Assets/Scriptable Object Assets/Item/soItemList.asset", typeof(SOItemList)) as SOItemList;

		List<ItemDetails> itemDetailsList = sOItemList.itemDetails;

		ItemDetails itemDetail = itemDetailsList.Find(x => x.itemCode == itemCode);

		if (itemDetail != null)
		{
			return itemDetail.itemDescription;
		}
		else
		{
			return "";
		}

	}
}
