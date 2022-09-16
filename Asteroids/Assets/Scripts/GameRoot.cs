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
    [SerializeField] private BigAsteroidConfig _bigAsteroidConfig;
    [SerializeField] private SmallAsteroidConfig _smallAsteroidConfig;
    [SerializeField] private UfoConfig _ufoConfig;

    private WeaponInputsHandler _weaponInputsHandler;
    private CameraData _cameraData;
    private List<IUpdatable> _updatables;

    private bool _isGameOver;

    private void Start()
    {
        _cameraData= new CameraData(Camera.main);
        _isGameOver = false;
        var collisionHandler = new CollisionHandler();

        var shipController = new ShipController(_shipConfigData, _shipView, _inputSystem, collisionHandler, _cameraData);
        shipController.OnShipDestroy += GameOver;
        var shipModel = shipController.GetShipModel();

        var bulletController = new BulletController(_weaponConfig, shipModel, collisionHandler, _cameraData);
        var laserController = new LaserController(_weaponConfig, shipModel, collisionHandler);
        _weaponInputsHandler = new WeaponInputsHandler(bulletController, laserController, _inputSystem);

        var asteroidhandler = new AsteroidHandler(_bigAsteroidConfig, _smallAsteroidConfig, collisionHandler, _cameraData);
        var ufoController = new UfoController(_ufoConfig, collisionHandler, shipModel, _cameraData);

        _updatables = new List<IUpdatable> { shipController, 
                                             bulletController,
                                             asteroidhandler,
                                             laserController, 
                                             collisionHandler, 
                                             ufoController };
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
