using UnityEngine.Events;

public interface IWeaponUseInputData
{
    UnityEvent onGunUse { get;}
    UnityEvent onLaserUse { get;}
}
