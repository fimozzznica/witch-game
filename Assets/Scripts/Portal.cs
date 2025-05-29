using UnityEngine;

public class Portal: MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject portal2;

        private void OnTriggerEnter(Collider other)
{
    player.transform.position = portal2.transform.position;
}
}
