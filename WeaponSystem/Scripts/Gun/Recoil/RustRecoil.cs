using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lightbug.CharacterControllerPro.Implementation;

public class RustRecoil : BaseRecoil
{
	[SerializeField] Transform cameraTransform;
	
	private Vector2 currentRecoilOffset; // The current offset applied to the weapon's aim or camera
	private int currentRecoilIndex; // The current index in the recoil pattern array
	
	// Initialize the recoil system
	private void Start()
	{
		currentRecoilOffset = Vector2.zero;
		currentRecoilIndex = 0;
	}

	// Apply the recoil when firing
	public override void ApplyRecoil()
	{
		// Get the next step in the recoil pattern
		Vector2 recoilStep = recoilData.recoilPattern[currentRecoilIndex];

		// Apply randomness and intensity to the recoil step
		recoilStep *= (1 + Random.Range(-recoilData.randomnessFactor, recoilData.randomnessFactor));
		recoilStep *= recoilData.intensity;

		// Update the current recoil offset
		currentRecoilOffset += recoilStep;

		// Apply the recoil offset to the weapon's aim or camera
		ApplyOffsetToAim();

		// Update the current index for the next shot, looping back to the start if necessary
		currentRecoilIndex = (currentRecoilIndex + 1) % recoilData.recoilPattern.Length;
	}

	// Reset the recoil over time
	public override void ResetRecoil()
	{
		currentRecoilOffset = Vector2.zero;
		
		// Apply the updated recoil offset to reset the weapon's aim or camera
		ApplyOffsetToAim();
	}
	
	private void ApplyOffsetToAim()
	{
		// Target rotation based on the recoil offset
		Vector3 targetRotation = cameraTransform.localEulerAngles;
		targetRotation.x -= currentRecoilOffset.y; // Pitch
		targetRotation.y += currentRecoilOffset.x; // Yaw

		// Current rotation
		Vector3 currentRotation = cameraTransform.localEulerAngles;

		// Smoothly interpolate between the current and target rotations
		Vector3 smoothRotation = Vector3.Lerp(currentRotation, targetRotation, Time.deltaTime * recoilData.recoveryRate);

		// Apply the smooth rotation to the camera
		cameraTransform.localEulerAngles = smoothRotation;
	}
}
