using UnityEngine;
using UnityEngine.UI;

public class AboutGame : MonoBehaviour
{
    [SerializeField] Button aboutGameButton;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject aboutGame;
    public void DoButtonThing()
    {
        mainMenu.SetActive(aboutGame.activeInHierarchy);
        aboutGame.SetActive(!aboutGame.activeInHierarchy);
    }

    private void Awake()
    {
        aboutGameButton.onClick.AddListener(DoButtonThing);
    }
}
