using UnityEngine;

[CreateAssetMenu(fileName = "RecoilData", menuName = "Weapon System/RecoilData", order = 1)]
public class RecoilData : ScriptableObject
{
	[Header("Pattern")]
	public Vector2[] recoilPattern; // An array of Vector2 to represent the x and y offsets for each shot in the pattern

	[Header("Randomness")]
	[Range(0, 1)]
	public float randomnessFactor; // A value between 0 and 1 to introduce randomness to the recoil pattern

	[Header("Intensity")]
	public float intensity; // A multiplier to scale the entire recoil pattern

	[Header("Recovery Rate")]
	public float recoveryRate; // The speed at which the aim returns to the original position

	[Header("Visual Effects")]
	public float cameraShakeAmount; // The amount of camera shake to apply when firing
}
