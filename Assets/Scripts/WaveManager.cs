using UnityEngine;
using System;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Serializable]
    public class Wave
    {
        [Tooltip("Ведьмы, которые спавнятся в этой волне")]
        public List<GameObject> witches;
    }

    [Header("Настройка волн")]
    [Tooltip("Добавьте столько элементов, сколько волн будет, и в каждом — любую коллекцию ведьм")]
    [SerializeField] private List<Wave> waves = new List<Wave>();

    [Header("UI и игрок")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject position;

    private int currentWave = 0;
    private int remainingInWave;
    private PlayerHealth playerHealth;

    public int damageDealt = 0;
    public int killsCount = 0;
    public int damageTaken = 0;

    private void Awake()
    {
        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();

        damageDealt = 0;
        killsCount = 0;
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath += OnPlayerDeath;
            playerHealth.OnDamage += OnPlayerDamage;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnDeath -= OnPlayerDeath;
        CleanupAllWitches();
    }

    private void Start()
    {
        gameOverCanvas?.SetActive(false);
        winCanvas?.SetActive(false);

        foreach (var wave in waves)
            foreach (var w in wave.witches)
                if (w != null) w.SetActive(false);

        SpawnWave(0);
    }

    private void SpawnWave(int index)
    {
        if (index >= waves.Count)
        {
            Win();
            return;
        }

        currentWave = index;
        remainingInWave = 0;

        foreach (var witch in waves[index].witches)
        {
            if (witch == null) continue;
            witch.SetActive(true);

            var health = witch.GetComponent<WitchHealth>();
            if (health != null)
            {
                health.OnDeath += OnWitchDeath;
                health.OnDamage += OnWitchDamage;
                remainingInWave++;
            }
        }
    }

    private void OnWitchDeath()
    {
        remainingInWave--;
        killsCount++;
        playerHealth.Heal(50);
        if (remainingInWave <= 0)
            SpawnWave(currentWave + 1);
    }

    private void OnWitchDamage(int amount)
    {
        damageDealt += amount;
    }

    private void OnPlayerDamage(int amount)
    {
        damageTaken += amount;
    }

    private void OnPlayerDeath()
    {
        player.transform.position = position.transform.position;
        gameOverCanvas?.SetActive(true);
        gameOverCanvas?.GetComponent<TextEditor>().UpdateText($"Количество убитых ведьм:{killsCount}\nКоличество нанесенного урона: {damageDealt}\nКоличество полученного урона: {damageTaken}");
        CleanupAllWitches();
    }

    private void Win()
    {
        player.transform.position = position.transform.position;
        winCanvas?.SetActive(true);
        winCanvas?.GetComponent<TextEditor>().UpdateText($"Количество убитых ведьм:{killsCount}\nКоличество нанесенного урона: {damageDealt}\nКоличество полученного урона: {damageTaken}");
        CleanupAllWitches();
    }

    private void CleanupAllWitches()
    {
        foreach (var wave in waves)
        {
            foreach (var w in wave.witches)
            {
                if (w == null) continue;
                var health = w.GetComponent<WitchHealth>();
                if (health != null)
                {
                    health.OnDeath -= OnWitchDeath;
                    health.OnDamage -= OnWitchDamage;
                }
                w.SetActive(false);
            }
        }
    }
}