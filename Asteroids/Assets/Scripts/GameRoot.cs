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
    [Space]
    [SerializeField] private InformationPanel _informationPanel;
    [SerializeField] private GameOverPanel _gameOverPanel;

    private ShipController _shipController;
    private LaserController _laserController;
    private WeaponInputsHandler _weaponInputsHandler;
    private CameraData _cameraData;
    private List<IUpdatable> _updatables;

    private bool _isGameOver;
    private int _score;
    private void Start()
    {
        _gameOverPanel.gameObject.SetActive(false);

        _cameraData = new CameraData(Camera.main);
        _isGameOver = false;
        _score = 0;

        var collisionHandler = new CollisionHandler();
        _shipController = new ShipController(_shipConfigData, _shipView, _inputSystem, collisionHandler, _cameraData);
        _shipController.OnShipDestroy += GameOver;
        var shipModel = _shipController.GetShipModel();

        var bulletController = new BulletController(_weaponConfig, shipModel, collisionHandler, _cameraData);
        _laserController = new LaserController(_weaponConfig, shipModel, collisionHandler);
        _weaponInputsHandler = new WeaponInputsHandler(bulletController, _laserController, _inputSystem);

        var asteroidhandler = new AsteroidHandler(_bigAsteroidConfig, _smallAsteroidConfig, collisionHandler, _cameraData);
        asteroidhandler.OnEnemyDestroyed += AddPoints;
        var ufoController = new UfoController(_ufoConfig, collisionHandler, shipModel, _cameraData);
        ufoController.OnEnemyDestroyed += AddPoints;

        _updatables = new List<IUpdatable> { _shipController, 
                                             bulletController,
                                             asteroidhandler,
                                             _laserController, 
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

        _informationPanel.UpdateFields(_shipController.GetShipModel().Position, 
                                       _shipController.GetShipModel().Rotation,
                                       _shipController.GetCurrentSpeed(),
                                       _laserController.CurrentAmount,
                                       _laserController.RechargeTime);
    }

    private void AddPoints(Enemy model) => _score += model.Points;
    
    private void GameOver()
    {
        _isGameOver = true;
        _gameOverPanel.Show(_score);
    }
}
