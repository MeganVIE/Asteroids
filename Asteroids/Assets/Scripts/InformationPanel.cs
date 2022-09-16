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

    public void UpdateFields(Vector2 coordinates, float angle, float speed, byte amount, float recharge)
    {
        _coordinatesValueText.text = coordinates.ToString();
        _angleValueText.text = Math.Round(angle, 2).ToString();
        _speedValueText.text = Math.Round(speed, 3).ToString();
        _lasersAmountValueText.text = amount.ToString();
        _laserRechargeValueText.text = Math.Round(recharge, 2).ToString();
    }
}
