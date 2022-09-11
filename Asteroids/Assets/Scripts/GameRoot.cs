using System.Collections.Generic;
using UnityEngine;

using Ship;
using Weapon;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private InputSystem _inputSystem;
    [Space]
    [SerializeField] private ShipView _shipView;
    [SerializeField] private ShipConfig _shipConfigData;
    [SerializeField] private WeaponConfig _weaponConfig;

    private WeaponHandler _weaponhandler;

    private List<IController> _controllers;

    public static GameRoot Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        Instance = this;
    }

    private void Start()
    {
        var shipController = new ShipController(_shipConfigData, _shipView, _inputSystem);
        var bulletController = new BulletController(_weaponConfig, shipController.GetShipTransform());
        _weaponhandler = new WeaponHandler(bulletController, _inputSystem);

        _controllers = new List<IController> { shipController, bulletController };
    }

    private void Update()
    {
        foreach (var controller in _controllers)
        {
            controller.Update();
        }
    }
}
