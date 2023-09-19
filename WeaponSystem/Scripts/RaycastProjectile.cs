using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastProjectile : MonoBehaviour, IProjectileType
{
	[SerializeField] private Camera _camera;
	
	[SerializeField] private LayerMask _hitLayers; // Layers that the raycast can hit

	public void Fire(WeaponData weaponData)
	{
		RaycastHit hit;
		Vector3 start = _camera.transform.position;
		Vector3 direction = _camera.transform.forward;

		if (Physics.Raycast(start, direction, out hit, weaponData.Range, _hitLayers))
		{
			if(hit.transform.CompareTag("Player"))
				return;
				
			// We hit something
			Debug.Log("Hit: " + hit.collider.name);
		}
	}
}
