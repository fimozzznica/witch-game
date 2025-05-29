using UnityEngine;

public class RockGlow : MonoBehaviour
{
    private Light rockLight; // Ссылка на компонент света

    void Start()
    {
        // Получаем компонент Light у камня
        rockLight = GetComponentInChildren<Light>();

        // Выключаем свет в начале
        if (rockLight != null)
            rockLight.enabled = false;
    }

    // Вызывается при взятии камня в руки
    public void OnPickUp()
    {
        if (rockLight != null)
            rockLight.enabled = true;
    }

    // Вызывается при отпускании камня
    public void OnDrop()
    {
        if (rockLight != null)
            rockLight.enabled = false;
    }
}
