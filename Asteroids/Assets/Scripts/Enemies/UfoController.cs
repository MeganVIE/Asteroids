using Ship;

namespace Enemies
{
    public class UfoController : EnemyPeriodicSpawnController
    {
        private ShipModel _shipModel;
        public UfoController(UfoConfig ufoConfig, CollisionHandler collisionHandler, ShipModel shipModel, CameraData cameraData) :
            base(ufoConfig, collisionHandler, cameraData)
        {
            _shipModel = shipModel;
        }

        protected override void UpdateModelViewData(Enemy model)
        {
            var newDirection = (_shipModel.Position - model.Position).normalized;
            model.ChangeDirection(newDirection);
            base.UpdateModelViewData(model);
        }
    }
}
