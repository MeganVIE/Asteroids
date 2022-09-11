using Ship;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private InputSystem _inputSystem;
    [Space]
    [SerializeField] private ShipView _shipView;
    [SerializeField] private ShipConfig _shipConfigData;

    private ShipController _shipController;

    private void Start()
    {
        _shipController = new ShipController(_shipConfigData, _shipView, _inputSystem);
    }

    private void Update()
    {
        _shipController.Update();
    }
}
