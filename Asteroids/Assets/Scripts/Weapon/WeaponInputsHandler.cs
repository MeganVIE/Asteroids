namespace Weapon
{
    public class WeaponInputsHandler : IClearable
    {
        private IWeaponUseInputData _inputSystem;
        private BulletController _bulletController;
        private LaserController _laserController;

        public WeaponInputsHandler(BulletController bulletController, LaserController laserController, IWeaponUseInputData inputs)
        {
            _bulletController = bulletController;
            _laserController = laserController;

            _inputSystem = inputs;
            _inputSystem.onGunUse += OnDefaultGunUse;
            _inputSystem.onLaserUse += OnLaserGunUse;
        }

        void IClearable.Clear()
        {
            _inputSystem.onGunUse -= OnDefaultGunUse;
            _inputSystem.onLaserUse -= OnLaserGunUse;
        }

        private void OnDefaultGunUse()
        {
            _bulletController.CreateBullet();
        }

        private void OnLaserGunUse()
        {
            if (_laserController.CanCreateLaser)
                _laserController.CreateBullet();
        }
    }
}
