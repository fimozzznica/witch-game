using UnityEngine;
using UnityEngine.UI;

public class Back : MonoBehaviour
{
    [SerializeField] Button backButton;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject thisCanvas;
    public void DoButtonThing()
    { 
        thisCanvas.SetActive(mainMenu.activeInHierarchy);
        mainMenu.SetActive(!mainMenu.activeInHierarchy);
        
    }

    private void Awake()
    {
        backButton.onClick.AddListener(DoButtonThing);
    }
}
