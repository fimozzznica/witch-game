using UnityEngine;
using UnityEngine.AI;

public class GoToTarget : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMashAgent;
    [SerializeField] private Transform player;
    private void Update()
    {
        navMashAgent.destination = player.position;
    }
}
