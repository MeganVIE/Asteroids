using UnityEngine;

namespace Panels
{
    public class PanelsController : IUpdatable
    {
        private InformationPanel _view;
        private InformationPanelModel _model;

        public PanelsController(InformationPanel view)
        {
            _model = new InformationPanelModel();
            _view = view;
        }

        public void UpdateCoordinatesData(Vector2 data) => _model.UpdateCoordinates(data);
        public void UpdateRotationData(float data) => _model.UpdateRotation(data);
        public void UpdateSpeedData(float data) => _model.UpdateSpeed(data);
        public void UpdateLaserAmountData(byte data) => _model.UpdateAmount(data);
        public void UpdateRechargeTimeData(float data) => _model.UpdateRechargeTime(data);

        void IUpdatable.Update()
        {
            _view.UpdateFields(_model.Coordinates, _model.Angle, _model.Speed, _model.Amount, _model.RechargeTime);
        }
    }
}