using UnityEngine;
using TMPro;
using System;

public class InformationPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coordinatesValueText;
    [SerializeField] private TextMeshProUGUI _angleValueText;
    [SerializeField] private TextMeshProUGUI _speedValueText;
    [SerializeField] private TextMeshProUGUI _lasersAmountValueText;
    [SerializeField] private TextMeshProUGUI _laserRechargeValueText;
    [Space]
    [SerializeField] private byte _angleRoundDigits = 2;
    [SerializeField] private byte _speedRoundDigits = 3;
    [SerializeField] private byte _laserRechargeRoundDigits = 2;

    public void UpdateFields(Vector2 coordinates, float angle, float speed, byte amount, float recharge)
    {
        _coordinatesValueText.text = coordinates.ToString();
        _angleValueText.text = Math.Round(angle, _angleRoundDigits).ToString();
        _speedValueText.text = Math.Round(speed, _speedRoundDigits).ToString();
        _lasersAmountValueText.text = amount.ToString();
        _laserRechargeValueText.text = Math.Round(recharge, _laserRechargeRoundDigits).ToString();
    }
}
