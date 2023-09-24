using UnityEngine;

public abstract class BaseRecoil : MonoBehaviour, IRecoilBehavior
{
	public RecoilData recoilData;

	public abstract void ApplyRecoil();
	public abstract void ResetRecoil();
}