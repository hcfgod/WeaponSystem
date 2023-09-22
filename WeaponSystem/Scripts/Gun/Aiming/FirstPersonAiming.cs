using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonAiming : MonoBehaviour, IAimingMode
{
	[SerializeField] private Camera playerCamera;
	
	[SerializeField] private float normalFOV = 60f;
	[SerializeField] private float aimingFOV = 40f;
	
	[SerializeField] private float aimingSpeed = 0.08f;
	
	[SerializeField] private Vector3 normalPosition;
	[SerializeField] private Vector3 aimingPosition;
	
	private bool isAiming = false;

	public bool IsAiming
	{
		get { return isAiming; }
	}

	private void Update()
	{
		// Smoothly interpolate FOV
		float targetFOV = isAiming ? aimingFOV : normalFOV;
		playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, aimingSpeed);

		// Smoothly interpolate weapon position
		Vector3 targetPosition = isAiming ? aimingPosition : normalPosition;
		transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, aimingSpeed);
	}

	public void Aim()
	{
		isAiming = true;
	}

	public void StopAiming()
	{
		isAiming = false;
	}
}