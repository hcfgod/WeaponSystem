using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastProjectile : MonoBehaviour, IProjectileType
{
	[SerializeField] private GameObject playerRoot;
	
	public UnityEvent2 OnTargetHitUnityEvent;
	public delegate void TargetHitEventHandler(GameObject target);
	
	public event TargetHitEventHandler OnTargetHit;
	
	[SerializeField] private Camera _camera;
	
	[SerializeField] private LayerMask _hitLayers; // Layers that the raycast can hit

	private List<Transform> _ignoreTransforms;
	
	private void Start()
	{
		// Initialize the list of transforms to ignore
		_ignoreTransforms = ComponentUtils.CollectAllTransforms(playerRoot.transform);
	}
	
	public void Fire(WeaponData weaponData)
	{
		RaycastHit hit;
		Vector3 start = _camera.transform.position;
		Vector3 direction = _camera.transform.forward;

		if (Physics.Raycast(start, direction, out hit, weaponData.Range, _hitLayers))
		{
			// Check if the hit object is in the ignore list
			if (_ignoreTransforms.Contains(hit.transform))
			{
				return; // We hit ourselves or a child object, so return
			}
			
			// We hit something
			Debug.Log("Hit: " + hit.collider.name);
			
			OnTargetHit?.DynamicInvoke();
			OnTargetHitUnityEvent?.Invoke();
		}
	}
}
