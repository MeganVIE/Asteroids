using System.Collections.Generic;
using UnityEngine;

using Ship;
using Weapon;
using Enemies;
using Panels;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private InputSystem _inputSystem;
    [Space]
    [SerializeField] private ShipConfig _shipConfig;
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
    private BulletController _bulletController;
    private WeaponInputsHandler _weaponInputsHandler;

    private AsteroidHandler _asteroidhandler;
    private UfoController _ufoController;

    private PanelsController _panelsController;

    private CameraData _cameraData;

    private List<IUpdatable> _updatables;
    private List<IClearable> _clearables;
    private List<IRestartable> _restartables;

    private bool _isGameOver;
    private ScoreData _scoreData;

    private void Start()
    {
        _cameraData = new CameraData(Camera.main);
        var collisionHandler = new CollisionHandler();
        ControllersInits(collisionHandler);

        _scoreData = new ScoreData();

        _updatables = new List<IUpdatable> { _shipController, 
                                             _bulletController,
                                             _asteroidhandler,
                                             _laserController, 
                                             collisionHandler,
                                             _ufoController,
                                             _panelsController};

        _clearables = new List<IClearable> { _shipController,
                                             _bulletController,
                                             _asteroidhandler,
                                             _laserController,
                                             collisionHandler,
                                             _ufoController,
                                            _weaponInputsHandler};

        _restartables = new List<IRestartable> { _shipController,
                                                  _bulletController,
                                                 _laserController,
                                                 _asteroidhandler,
                                                 _ufoController,
                                                 _panelsController};

        Restart();
        Subscribes();
    }

    private void ControllersInits(CollisionHandler collisionHandler)
    {
        _shipController = new ShipController(_shipConfig, _inputSystem, collisionHandler, _cameraData);
        var shipModel = _shipController.GetShipModel();

        _bulletController = new BulletController(_weaponConfig, shipModel, collisionHandler, _cameraData);
        _laserController = new LaserController(_weaponConfig, shipModel, collisionHandler);
        _weaponInputsHandler = new WeaponInputsHandler(_bulletController, _laserController, _inputSystem);

        _asteroidhandler = new AsteroidHandler(_bigAsteroidConfig, _smallAsteroidConfig, collisionHandler, _cameraData);
        _ufoController = new UfoController(_ufoConfig, collisionHandler, shipModel, _cameraData);

        _panelsController = new PanelsController(_informationPanel, _gameOverPanel);
    }

    private void Subscribes()
    {
        _panelsController.SubscribeToRestart(Restart);
        _shipController.OnShipDestroy += GameOver;
        _asteroidhandler.OnEnemyDestroyed += AddPoints;
        _ufoController.OnEnemyDestroyed += AddPoints;

        _shipController.OnCoordinatesChange += _panelsController.UpdateCoordinatesData;
        _shipController.OnRotationChange += _panelsController.UpdateRotationData;
        _shipController.OnSpeedChange += _panelsController.UpdateSpeedData;
        _laserController.OnCurrentAmountChange += _panelsController.UpdateLaserAmountData;
        _laserController.OnRechargeTimeChange += _panelsController.UpdateRechargeTimeData;
    }
    private void Unsubscribes()
    {
        _panelsController.UnsubscribeToRestart(Restart);
        _shipController.OnShipDestroy -= GameOver;
        _asteroidhandler.OnEnemyDestroyed -= AddPoints;
        _ufoController.OnEnemyDestroyed -= AddPoints;

        _shipController.OnCoordinatesChange -= _panelsController.UpdateCoordinatesData;
        _shipController.OnRotationChange -= _panelsController.UpdateRotationData;
        _shipController.OnSpeedChange -= _panelsController.UpdateSpeedData;
        _laserController.OnCurrentAmountChange -= _panelsController.UpdateLaserAmountData;
        _laserController.OnRechargeTimeChange -= _panelsController.UpdateRechargeTimeData;
    }

    private void Restart()
    {
        _isGameOver = false;
        _scoreData.Clear();

        foreach (var restartable in _restartables)
            restartable.Restart();

        _panelsController.UpdateLaserAmountData(_laserController.LasersAmount);
    }

    private void Update()
    {
        if (_isGameOver || _updatables == null)
            return;

        foreach (var updatable in _updatables)
            updatable.Update();
    }

    private void OnDestroy()
    {
        if (_updatables == null)
            return;

        foreach (var clearable in _clearables)
            clearable.Clear();

        Unsubscribes();
    }

    private void AddPoints(Enemy model) => _scoreData.Increase(model.Points);
    
    private void GameOver()
    {
        _isGameOver = true;
        _panelsController.ShowGameOverPanel(_scoreData.Value);
    }
}
