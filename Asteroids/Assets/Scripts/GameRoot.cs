using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private InputSystem _inputSystem;
    [Space]
    [SerializeField] private ShipView _shipView;

    private ShipController _shipController;

    private void Start()
    {
        _shipController = new ShipController(_shipView, _inputSystem);
    }

    private void Update()
    {
        _shipController.Update();
    }
}
