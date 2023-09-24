using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponClippingHandler : MonoBehaviour
{
	public UnityEvent2 OnWeaponClipped;
	
	[SerializeField] private PlayerData _playerData; 
	
	[SerializeField] private Transform cameraTransform; // Reference to the camera transform
	[SerializeField] private Transform weaponTransform; // Reference to the weapon transform
	[SerializeField] private float weaponLength = 1.0f; // Length of the weapon from the camera to its tip
	
	[SerializeField] private float sphereRadius = 0.1f; // Radius of the sphere for SphereCast
	[SerializeField] private float lerpSpeed = 5.0f; // Speed of the smooth transition

	private Vector3 originalLocalPosition; // Original local position of the weapon
	private Vector3 originalLocalRotation; // Original local position of the weapon

	[SerializeField] private Vector3 collisionLocalPosition;
	[SerializeField] private Vector3 collisionLocalRotation;
	
	[SerializeField] private LayerMask environmentLayerMask; // Layer mask for the environment

	// Initialize the system
	private void Start()
	{
		originalLocalPosition = weaponTransform.localPosition;
		originalLocalRotation = weaponTransform.localEulerAngles;
	}

	// Update is called once per frame
	private void Update()
	{
		HandleWeaponClipping();
	}

	// Handle weapon clipping through walls
	
	private void HandleWeaponClipping()
	{
		RaycastHit hit;
  
		if (Physics.SphereCast(cameraTransform.position, sphereRadius, cameraTransform.forward, out hit, weaponLength, environmentLayerMask))
		{
			OnWeaponClipped?.Invoke();
			
			_playerData.isTooCloseToWall = true;
			
			// Smoothly move weapon to the collision position and rotation
			weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition, collisionLocalPosition, Time.deltaTime * lerpSpeed);
			weaponTransform.localRotation = Quaternion.Slerp(weaponTransform.localRotation, Quaternion.Euler(collisionLocalRotation), Time.deltaTime * lerpSpeed);
		}
		else
		{
			_playerData.isTooCloseToWall = false;
			
			// Smoothly reset weapon to its original local position and rotation
			weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition, originalLocalPosition, Time.deltaTime * lerpSpeed);
			weaponTransform.localRotation = Quaternion.Slerp(weaponTransform.localRotation, Quaternion.Euler(originalLocalRotation), Time.deltaTime * lerpSpeed);
		}
	}
}
