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
    private CollisionHandler _collisionHandler;
    private List<IController> _controllers;

    private bool _isGameOver;

    private void Start()
    {        
        _isGameOver = false;
        _collisionHandler = new CollisionHandler();

        var shipController = new ShipController(_shipConfigData, _shipView, _inputSystem, _collisionHandler);
        shipController.OnShipDestroy += GameOver;

        var bulletController = new BulletController(_weaponConfig, shipController.GetShipTransform(), _collisionHandler);
        _weaponhandler = new WeaponHandler(bulletController, _inputSystem);

        var asteroidController = new AsteroidController(_asteroidConfig, _collisionHandler);

        _controllers = new List<IController> { shipController, bulletController, asteroidController };
    }

    private void Update()
    {
        if (_isGameOver)
            return;

        foreach (var controller in _controllers)
        {
            controller.Update();
        }
        _collisionHandler.Update();
    }
    
    public void GameOver()
    {
        _isGameOver = true;
        Debug.Log("game over");
    }
}
