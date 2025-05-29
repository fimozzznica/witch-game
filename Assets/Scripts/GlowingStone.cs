using UnityEngine;

public class GlowingStone : MonoBehaviour
{
    private Light stoneLight;
    private void Awake()
    {
        stoneLight = GetComponent<Light>();

        if (stoneLight != null)
            stoneLight.enabled = false;
    }

    public void OnPickup()
    {
        if (stoneLight != null)
            stoneLight.enabled = true;
    }

    public void OnDrop()
    {
        if (stoneLight != null)
            stoneLight.enabled = false;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("hand"))
        {
            OnPickup();
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("hand"))
        {
            OnDrop();
        }
    }
}
