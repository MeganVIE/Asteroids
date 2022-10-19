using UnityEngine;

namespace Panels
{
    public class InformationPanelModel
    {
        private Vector2 _coordinates;
        private float _angle;
        private float _speed;
        private byte _amount;
        private float _recharge;

        public Vector2 Coordinates => _coordinates;
        public float Angle => _angle;
        public float Speed => _speed;
        public byte Amount => _amount;
        public float RechargeTime => _recharge;

        public void UpdateCoordinates(Vector2 position) => _coordinates = position;
        public void UpdateRotation(float angle) => _angle = angle;
        public void UpdateSpeed(float speed) => _speed = speed;
        public void UpdateAmount(byte amount) => _amount = amount;
        public void UpdateRechargeTime(float time) => _recharge = time;
    }
}
