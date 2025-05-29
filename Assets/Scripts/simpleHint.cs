using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SimpleHint : MonoBehaviour
{
    public TextMesh hintText; // Перетащите сюда ваш TextMesh

    void Start()
    {
        // Получаем компонент для захвата
        GetComponent<XRGrabInteractable>().selectEntered.AddListener((args) => {
            hintText.GetComponent<MeshRenderer>().enabled = true;
        });

        GetComponent<XRGrabInteractable>().selectExited.AddListener((args) => {
            hintText.GetComponent<MeshRenderer>().enabled = false;
        });
    }
}
