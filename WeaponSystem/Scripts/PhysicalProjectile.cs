using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalProjectile : MonoBehaviour, IProjectileType
{
	public GameObject bulletPrefab;
	public Transform bulletSpawnPoint;
	public float bulletSpeed = 30f;
	public int poolSize = 20; // Size of the bullet pool
	
	private Queue<GameObject> bulletPool;

	private void Awake()
	{
		// Initialize the bullet pool
		bulletPool = new Queue<GameObject>();

		for (int i = 0; i < poolSize; i++)
		{
			GameObject bullet = Instantiate(bulletPrefab, transform);
			bullet.SetActive(false);
			bulletPool.Enqueue(bullet);
		}
	}
	
	public void Fire()
	{
		if (bulletPool.Count == 0) return; // No bullets available in the pool

		// Dequeue a bullet from the pool
		GameObject bullet = bulletPool.Dequeue();
		bullet.transform.position = bulletSpawnPoint.position;
		bullet.transform.rotation = bulletSpawnPoint.rotation;
		bullet.SetActive(true);

		// Apply force to the bullet
		Rigidbody rb = bullet.GetComponent<Rigidbody>();
		rb.velocity = bulletSpeed * bulletSpawnPoint.forward;

		// Enqueue the bullet back into the pool after some time
		StartCoroutine(DeactivateAndEnqueueBullet(bullet, 2.0f));
	}
	
	private IEnumerator DeactivateAndEnqueueBullet(GameObject bullet, float delay)
	{
		yield return new WaitForSeconds(delay);
		bullet.SetActive(false);
		bulletPool.Enqueue(bullet);
	}
}
