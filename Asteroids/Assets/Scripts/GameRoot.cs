using System.Collections.Generic;
using UnityEngine;

using Ship;
using Weapon;
using Enemies;

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
    private WeaponInputsHandler _weaponInputsHandler;

    private AsteroidHandler _asteroidhandler;
    private UfoController _ufoController;

    private CameraData _cameraData;

    private List<IUpdatable> _updatables;
    private List<IClearable> _clearables;
    private List<IRestartable> _restartables;

    private bool _isGameOver;
    private int _score;

    private void Start()
    {
        _cameraData = new CameraData(Camera.main);
        var collisionHandler = new CollisionHandler();

        _shipController = new ShipController(_shipConfig, _inputSystem, collisionHandler, _cameraData);
        var shipModel = _shipController.GetShipModel();

        var bulletController = new BulletController(_weaponConfig, shipModel, collisionHandler, _cameraData);
        _laserController = new LaserController(_weaponConfig, shipModel, collisionHandler);
        _weaponInputsHandler = new WeaponInputsHandler(bulletController, _laserController, _inputSystem);

        _asteroidhandler = new AsteroidHandler(_bigAsteroidConfig, _smallAsteroidConfig, collisionHandler, _cameraData);
        _ufoController = new UfoController(_ufoConfig, collisionHandler, shipModel, _cameraData);

        _updatables = new List<IUpdatable> { _shipController, 
                                             bulletController,
                                             _asteroidhandler,
                                             _laserController, 
                                             collisionHandler,
                                             _ufoController };

        _clearables = new List<IClearable> { _shipController,
                                             bulletController,
                                             _asteroidhandler,
                                             _laserController,
                                             collisionHandler,
                                             _ufoController,
                                            _weaponInputsHandler};

        _restartables = new List<IRestartable> { _shipController,
                                                  bulletController,
                                                 _laserController,
                                                 _asteroidhandler,
                                                 _ufoController};
        Restart();
        _gameOverPanel.onRestartClick += Restart;
        _shipController.OnShipDestroy += GameOver;
        _asteroidhandler.OnEnemyDestroyed += AddPoints;
        _ufoController.OnEnemyDestroyed += AddPoints;
    }

    private void Restart()
    {
        _gameOverPanel.gameObject.SetActive(false);
        _isGameOver = false;
        _score = 0;

        foreach (var restartable in _restartables)
            restartable.Restart();
    }

    private void Update()
    {
        if (_isGameOver || _updatables == null)
            return;

        foreach (var updatable in _updatables)
            updatable.Update();

        _informationPanel.UpdateFields(_shipController.GetShipModel().Position, 
                                       _shipController.GetShipModel().Rotation,
                                       _shipController.GetCurrentSpeed(),
                                       _laserController.CurrentAmount,
                                       _laserController.RechargeTime);
    }

    private void OnDestroy()
    {
        if (_updatables == null)
            return;

        foreach (var clearable in _clearables)
            clearable.Clear();

        _gameOverPanel.onRestartClick -= Restart;
        _shipController.OnShipDestroy -= GameOver;
        _asteroidhandler.OnEnemyDestroyed -= AddPoints;
        _ufoController.OnEnemyDestroyed -= AddPoints;
    }

    private void AddPoints(Enemy model) => _score += model.Points;
    
    private void GameOver()
    {
        _isGameOver = true;
        _gameOverPanel.Show(_score);
    }
}
