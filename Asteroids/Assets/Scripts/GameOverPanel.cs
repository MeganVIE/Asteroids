using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _scoreValueText;

    public void Show(int score)
    {
        gameObject.SetActive(true);
        _scoreValueText.text = score.ToString();
    }
}
