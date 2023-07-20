using UnityEngine;

public class Orb : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>() && other.GetComponent<Player>().jumpAction.action.WasPressedThisFrame())
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.up * 18;
        }
    }
}
