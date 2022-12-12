using System;
using UnityEngine;

namespace Panels
{
    public class PanelsController : IUpdatable, IRestartable
    {
        private InformationPanel _view;
        private InformationPanelModel _model;

        private GameOverPanel _gameOverPanel;


        public PanelsController(InformationPanel view, GameOverPanel gameOverPanel)
        {
            _model = new InformationPanelModel();
            _view = view;

            _gameOverPanel = gameOverPanel;
        }

        public void ShowGameOverPanel(int score) => _gameOverPanel.Show(score);
        public void HideGameOverPanel() => _gameOverPanel.gameObject.SetActive(false);
        public void SubscribeToRestart(Action callback) => _gameOverPanel.onRestartClick += callback;
        public void UnsubscribeToRestart(Action callback) => _gameOverPanel.onRestartClick -= callback;

        public void UpdateCoordinatesData(Vector2 data) => _model.UpdateCoordinates(data);
        public void UpdateRotationData(float data) => _model.UpdateRotation(data);
        public void UpdateSpeedData(float data) => _model.UpdateSpeed(data);
        public void UpdateLaserAmountData(byte data) => _model.UpdateAmount(data);
        public void UpdateRechargeTimeData(float data) => _model.UpdateRechargeTime(data);

        void IUpdatable.Update()
        {
            _view.UpdateFields(_model.Coordinates, _model.Angle, _model.Speed, _model.Amount, _model.RechargeTime);
        }

        void IRestartable.Restart()
        {
            HideGameOverPanel();
            UpdateRechargeTimeData(0);
        }
    }
}