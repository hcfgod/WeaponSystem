using UnityEngine;

public class RangedWeapon : BaseWeapon
{
	public IFiringMode FiringMode { get; set; }

    private void Start()
    {
        // Initialize with SingleShot as default
        FiringMode = GetComponent<SingleShot>();
    }

    public override void Attack()
    {
        FiringMode?.ExecuteFiringSequence();
    }
}
