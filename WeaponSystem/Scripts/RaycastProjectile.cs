using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastProjectile : MonoBehaviour, IProjectileType
{
	public float range = 100f;
	public int damage = 10;

	public void Fire()
	{
		// Raycasting logic here
	}
}
