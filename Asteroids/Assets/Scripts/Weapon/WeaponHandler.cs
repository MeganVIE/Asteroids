namespace Weapon
{
    public class WeaponHandler
    {
        private IWeaponUseInputData _inputSystem;
        private BulletController _bulletController;

        public WeaponHandler(BulletController bulletController, IWeaponUseInputData iputs)
        {
            _bulletController = bulletController;

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

        }
    }
}
