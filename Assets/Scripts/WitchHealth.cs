using System;
using UnityEngine;

public class WitchHealth : MonoBehaviour
{
    public event Action OnDeath;
    public event Action<int> OnDamage;

    [SerializeField] public int health = 100;
    [SerializeField] private float damageCooldown = 0.5f;
    [SerializeField] private GameObject witchCanvas;
    [SerializeField] private GameObject player;


    private float lastDamageTime = -999f;
    private int maxHealth;
    private float DamageTime = 0f;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        DamageTime += Time.deltaTime;
        maxHealth = health;
    }

    public void TakeDamage(int amount)
    {
        if (Time.time - lastDamageTime < damageCooldown)
            return;

        lastDamageTime = Time.time;

        health -= amount;
        OnDamage?.Invoke(amount);
        //Debug.Log($"now witch health is {health}");

        if (health <= 0)
        {
            //Debug.Log("Witch Death trigger (health <= 0)");
            Die();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("weapon"))
        {
            TakeDamage(10);
            witchCanvas?.GetComponent<TextEditor>().UpdateText($"{health}");
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        //Debug.Log("Witch Died");
        gameObject.SetActive(false);
        health = maxHealth;
    }
}