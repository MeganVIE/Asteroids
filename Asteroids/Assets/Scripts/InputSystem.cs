using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public interface IMoveRotateInputData
{
    float RotationValue { get;}
    InputActionPhase MoveForwardPhase { get;}
}

public interface IWeaponUseInputData
{
    UnityEvent onGunUse { get; set; }
    UnityEvent onLaserUse { get; set; }
}

public class InputSystem : MonoBehaviour, IMoveRotateInputData, IWeaponUseInputData
{
    #region Activation
    [SerializeField] private InputActionAsset _actionAsset;    

    private void Awake()
    {
        if (_actionAsset != null)
            _actionAsset.Enable();
    }
    #endregion

    [Space]
    [SerializeField] private InputActionReference _moveForward;
    [SerializeField] private InputActionReference _rotate;
    [SerializeField] private InputActionReference _gunUse;
    [SerializeField] private InputActionReference _laserUse;

    float IMoveRotateInputData.RotationValue => _rotate.action.ReadValue<float>();
    InputActionPhase IMoveRotateInputData.MoveForwardPhase => _moveForward.action.phase;

    public UnityEvent onGunUse { get; set; }
    public UnityEvent onLaserUse { get; set; }

    private void Start()
    {
        _gunUse.action.performed += GunUsePerformed;
        _laserUse.action.performed += LaserUsePerformed;
    }

    private void OnDestroy()
    {
        _gunUse.action.performed -= GunUsePerformed;
        _laserUse.action.performed -= LaserUsePerformed;
        onGunUse?.RemoveAllListeners();
        onLaserUse?.RemoveAllListeners();
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
