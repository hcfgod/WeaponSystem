public interface IAimingMode
{
	void Aim();
	void StopAiming();
	bool IsAiming { get; }
}