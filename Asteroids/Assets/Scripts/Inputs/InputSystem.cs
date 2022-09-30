using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour, IMoveRotateInputData, IWeaponUseInputData
{
    #region Activation
    [SerializeField] private InputActionAsset _actionAsset;    

    private void Awake()
    {
        if (_actionAsset != null)
            _actionAsset.Enable();

        Init();
    }
    #endregion

    [Space]
    [SerializeField] private InputActionReference _moveForward;
    [SerializeField] private InputActionReference _rotate;
    [SerializeField] private InputActionReference _gunUse;
    [SerializeField] private InputActionReference _laserUse;

    float IMoveRotateInputData.RotationValue => _rotate.action.ReadValue<float>();
    InputActionPhase IMoveRotateInputData.MoveForwardPhase => _moveForward.action.phase;

    public Action onGunUse { get; set; }
    public Action onLaserUse { get; set; }

    private void Init()
    {
        _gunUse.action.performed += GunUsePerformed;
        _laserUse.action.performed += LaserUsePerformed;
    }

    private void OnDestroy()
    {
        _gunUse.action.performed -= GunUsePerformed;
        _laserUse.action.performed -= LaserUsePerformed;
    }

    private void LaserUsePerformed(InputAction.CallbackContext obj)
    {
        onLaserUse?.Invoke();
    }

    private void GunUsePerformed(InputAction.CallbackContext obj)
    {
        onGunUse?.Invoke();
    }
}
