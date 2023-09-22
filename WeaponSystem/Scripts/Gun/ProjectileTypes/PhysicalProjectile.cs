using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalProjectile : MonoBehaviour, IProjectileType
{	
	public UnityEvent2 OnTargetHitUnityEvent;
	
	[SerializeField] private GameObject playerRoot;
	
	public GameObject bulletPrefab;
	public Transform bulletSpawnPoint;
	public float bulletSpeed = 30f;
	public int poolSize = 20; // Size of the bullet pool
	
	private Queue<GameObject> bulletPool;
	
	private List<Transform> _ignoreTransforms;
	
	private void Awake()
	{
		// Initialize the list of transforms to ignore
		_ignoreTransforms = ComponentUtils.CollectAllTransforms(playerRoot.transform);
		
		// Check if bulletPrefab and bulletSpawnPoint are set
		if (bulletPrefab == null || bulletSpawnPoint == null)
		{
			Debug.LogError("Bullet Prefab or Bullet Spawn Point is not set.");
			return;
		}
		
		// Initialize the bullet pool
		bulletPool = new Queue<GameObject>();

		for (int i = 0; i < poolSize; i++)
		{
			GameObject bulletObject = Instantiate(bulletPrefab, transform);
			Bullet bullet = bulletObject.AddComponent<Bullet>();
			bullet.OnTargetHit += TargetHit;
			bullet.IgnoreTransforms = _ignoreTransforms;
			bulletObject.SetActive(false);
			bulletPool.Enqueue(bulletObject);
		}
	}
	
	public void Fire(WeaponData weaponData)
	{
		// Check if weaponData is null or contains invalid data
		if (weaponData == null)
		{
			Debug.LogError("Weapon Data is null.");
			return;
		}
		
		if (bulletPool.Count == 0) return; // No bullets available in the pool

		// Dequeue a bullet from the pool
		GameObject bullet = bulletPool.Dequeue();
		bullet.transform.position = bulletSpawnPoint.position;
		bullet.transform.rotation = bulletSpawnPoint.rotation;
		bullet.SetActive(true);

		Rigidbody rb = bullet.GetComponent<Rigidbody>();
		
		if (rb == null)
		{
			Debug.LogError("Rigidbody not found on bullet prefab.");
			return;
		}
		
		rb.velocity = bulletSpeed * bulletSpawnPoint.forward;
		StartCoroutine(DeactivateAndEnqueueBullet(bullet, 2.0f));
	}
	
	private IEnumerator DeactivateAndEnqueueBullet(GameObject bullet, float delay)
	{
		yield return new WaitForSeconds(delay);
		bullet.SetActive(false);
		bulletPool.Enqueue(bullet);
	}
	
	private void TargetHit(GameObject gameobject)
	{
		OnTargetHitUnityEvent?.Invoke();
	}
}
