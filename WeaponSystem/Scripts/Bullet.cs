using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	Rigidbody bulletRigidBody;
	
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
	}
}
