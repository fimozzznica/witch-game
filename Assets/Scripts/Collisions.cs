using UnityEngine;

public class Collisions : MonoBehaviour
{
    [SerializeField] GameObject witch;

private void OnCollisionStay(Collision collision)
{
    witch.SetActive(!witch.activeInHierarchy);
}
}