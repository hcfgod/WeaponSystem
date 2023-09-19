using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
	public UnityEvent2 OnTargetHitUnityEvent;
	public delegate void TargetHitEventHandler(GameObject target);
	
	public event TargetHitEventHandler OnTargetHit;
	
	private Rigidbody bulletRigidBody;
	
	public List<Transform> IgnoreTransforms { get; set; }
	
	private void Awake()
	{
		bulletRigidBody = GetComponent<Rigidbody>();
	}
	
	private void OnTriggerEnter(Collider other)
	{
		// Check if the hit object is in the ignore list
		if (IgnoreTransforms.Contains(other.transform))
		{
			return; // We hit ourselves or a child object, so return
		}
		
		// We hit something
		Debug.Log("Hit: " + other.name);
		
		bulletRigidBody.velocity = Vector3.zero;
		gameObject.SetActive(false);
		
		OnTargetHit?.DynamicInvoke();
		OnTargetHitUnityEvent?.Invoke();
	}
}
