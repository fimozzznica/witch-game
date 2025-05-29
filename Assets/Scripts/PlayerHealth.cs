using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public event Action OnDeath;
    public event Action<int> OnDamage;
    public int health = 100;
    [SerializeField] private GameObject playerCanvas;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{other.gameObject.name} - tag: {other.gameObject.tag}");
        if (other.gameObject.CompareTag("witch"))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int amount)
    {

        health -= amount;
        playerCanvas?.GetComponent<TextEditor>().UpdateText($"Уровень здоровья:{health}");
        OnDamage?.Invoke(amount);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDeath?.Invoke();
        //gameObject.SetActive(false);
    }

    public void Heal(int amount)
    {
        health += amount;
        playerCanvas?.GetComponent<TextEditor>().UpdateText($"Уровень здоровья:{health}");
    }
}