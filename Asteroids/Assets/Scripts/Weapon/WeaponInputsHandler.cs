namespace Weapon
{
    public class WeaponInputsHandler
    {
        private IWeaponUseInputData _inputSystem;
        private BulletController _bulletController;
        private LaserController _laserController;

        public WeaponInputsHandler(BulletController bulletController, LaserController laserController, IWeaponUseInputData iputs)
        {
            _bulletController = bulletController;
            _laserController = laserController;

            _inputSystem = iputs;
            _inputSystem.onGunUse.AddListener(OnDefaultGunUse);
            _inputSystem.onLaserUse.AddListener(OnLaserGunUse);
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
