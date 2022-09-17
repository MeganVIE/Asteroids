using UnityEngine;
using UnityEngine.Events;
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

    private UnityEvent _onGunUse;
    private UnityEvent _onLaserUse;

    public UnityEvent onGunUse => _onGunUse;
    public UnityEvent onLaserUse => _onLaserUse;

    private void Init()
    {
        _gunUse.action.performed += GunUsePerformed;
        _laserUse.action.performed += LaserUsePerformed;
        _onGunUse = new UnityEvent();
        _onLaserUse = new UnityEvent();
    }

    private void OnDestroy()
    {
        _gunUse.action.performed -= GunUsePerformed;
        _laserUse.action.performed -= LaserUsePerformed;
        _onGunUse?.RemoveAllListeners();
        _onLaserUse?.RemoveAllListeners();
    }

    private void LaserUsePerformed(InputAction.CallbackContext obj)
    {
        _onLaserUse?.Invoke();
    }

    private void GunUsePerformed(InputAction.CallbackContext obj)
    {
        _onGunUse?.Invoke();
    }
}
