using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
	public UnityEvent2 OnBulletHit;
	private Rigidbody bulletRigidBody;
	
	private void Awake()
	{
		bulletRigidBody = GetComponent<Rigidbody>();
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Weapon"))
			return;
			
		bulletRigidBody.velocity = Vector3.zero;
		gameObject.SetActive(false);
		
		OnBulletHit?.Invoke();
	}
}
