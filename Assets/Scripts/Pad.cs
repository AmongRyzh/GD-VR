using UnityEngine;

public class Pad : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.up * 24;
        }
    }
}
