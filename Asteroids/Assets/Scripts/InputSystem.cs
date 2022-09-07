using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
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

    public float RotationValue => _rotate.action.ReadValue<float>();
    public InputActionPhase MoveForwardPhase => _moveForward.action.phase;

    public UnityEvent onGunUse;
    public UnityEvent onLaserUse;

    private void Start()
    {
        _gunUse.action.performed += GunUsePerformed;
        _laserUse.action.performed += LaserUsePerformed;
    }

    private void OnDestroy()
    {
        _gunUse.action.performed -= GunUsePerformed;
        _laserUse.action.performed -= LaserUsePerformed;
        onGunUse.RemoveAllListeners();
        onLaserUse.RemoveAllListeners();
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
