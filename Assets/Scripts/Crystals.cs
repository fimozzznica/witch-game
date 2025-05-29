using UnityEngine;

public class Crystals : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private PlayerHealth playerHealth;
    private void Awake()
    {
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"{other.name} (tag:{other.tag}) collided with {gameObject.name}!");
        if (collision.gameObject.CompareTag("weapon"))
        {
            playerHealth.Heal(100);
            gameObject.SetActive(false);
        }
    }
}