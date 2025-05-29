using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] Button startGameButton;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject waveManager;
    [SerializeField] GameObject playerCanvas;
    public void DoButtonThing()
    {
        mainMenu.SetActive(false);
        waveManager.SetActive(true);
        playerCanvas.SetActive(true);
    }

    private void Awake()
    {
        startGameButton.onClick.AddListener(DoButtonThing);
    }
}
