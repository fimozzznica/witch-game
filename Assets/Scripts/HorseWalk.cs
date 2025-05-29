using UnityEngine;
using UnityEngine.AI;


public class HorseWalk : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private Vector3 moveDirection;
    //[SerializeField] private NavMeshAgent HorseAgent;
    [SerializeField] private Animator animator;

    private void Start()
    {
        moveDirection = transform.forward;
        //animator.SetBool("isWalk", false);
    }

    private void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        {
            TurnLeft();
        }
    }

    private void TurnLeft()
    {
        transform.Rotate(0f, -90f, 0f);
        moveDirection = transform.forward;
    }
}
