using System;

public interface IWeaponUseInputData
{
    Action onGunUse { get; set; }
    Action onLaserUse { get; set; }
}
