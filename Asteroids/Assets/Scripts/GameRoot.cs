using System.Collections.Generic;
using UnityEngine;

using Ship;
using Weapon;
using Enemies;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private InputSystem _inputSystem;
    [Space]
    [SerializeField] private ShipView _shipView;
    [SerializeField] private ShipConfig _shipConfigData;
    [SerializeField] private WeaponConfig _weaponConfig;
    [Space]
    [SerializeField] private AsteroidConfig _asteroidConfig;

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
        var asteroidController = new AsteroidController(_asteroidConfig);

        _controllers = new List<IController> { shipController, bulletController, asteroidController };
    }

    private void Update()
    {
        foreach (var controller in _controllers)
        {
            controller.Update();
        }
    }
}
