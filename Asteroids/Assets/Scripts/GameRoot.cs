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
    [SerializeField] private UfoConfig _ufoConfig;

    private WeaponInputsHandler _weaponInputsHandler;
    private CollisionHandler _collisionHandler;
    private List<IUpdatable> _updatables;

    private bool _isGameOver;

    private void Start()
    {
        CameraData.Init(Camera.main);
        _isGameOver = false;
        _collisionHandler = new CollisionHandler();

        var shipController = new ShipController(_shipConfigData, _shipView, _inputSystem, _collisionHandler);
        shipController.OnShipDestroy += GameOver;
        var shipModel = shipController.GetShipModel();

        var bulletController = new BulletController(_weaponConfig, shipModel, _collisionHandler);
        var laserController = new LaserController(_weaponConfig, shipModel, _collisionHandler);
        _weaponInputsHandler = new WeaponInputsHandler(bulletController, laserController, _inputSystem);

        var asteroidController = new AsteroidController(_asteroidConfig, _collisionHandler);
        var ufoController = new UfoController(_ufoConfig, _collisionHandler, shipModel);

        _updatables = new List<IUpdatable> { shipController, bulletController, asteroidController, laserController, _collisionHandler, ufoController };
    }

    private void Update()
    {
        if (_isGameOver)
            return;

        foreach (var controller in _updatables)
        {
            controller.Update();
        }
    }
    
    public void GameOver()
    {
        _isGameOver = true;
        Debug.Log("game over");
    }
}
