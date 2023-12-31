﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentUtils : MonoBehaviour
{
	public static T AddOrGetComponent<T>(GameObject gameObject) where T : MonoBehaviour
	{
		T component = gameObject.GetComponent<T>();
    
		if (component == null)
		{
			component = gameObject.AddComponent<T>();
		}
    
		return component;
	}
	
	public static void RemoveDuplicateComponents<T>(GameObject gameObject) where T : class
	{
		List<MonoBehaviour> componentsToRemove = new List<MonoBehaviour>();
		T foundComponent = null;

		foreach (MonoBehaviour component in gameObject.GetComponents<MonoBehaviour>())
		{
			if (component is T)
			{
				if (foundComponent == null)
				{
					foundComponent = component as T;
				}
				else
				{
					componentsToRemove.Add(component);
				}
			}
		}

		foreach (MonoBehaviour component in componentsToRemove)
		{	
            #if UNITY_EDITOR
			DestroyImmediate(component);
            #else
			Destroy(component);
            #endif
		}
	}
	
	public static List<Transform> CollectAllTransforms(Transform root)
	{
		List<Transform> _ignoreTransforms = new List<Transform>();
		CollectAllTransformsRecursive(root, _ignoreTransforms);
		return _ignoreTransforms;
	}

	private static void CollectAllTransformsRecursive(Transform root, List<Transform> list)
	{
		list.Add(root);
		foreach (Transform child in root)
		{
			CollectAllTransformsRecursive(child, list);
		}
	}
}
