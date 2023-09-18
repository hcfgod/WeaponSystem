using UnityEngine;

public class SingleShot : MonoBehaviour, IFiringMode
{
	public void ExecuteFiringSequence(IProjectileType projectileType)
	{
		projectileType.Fire();
	}
}
