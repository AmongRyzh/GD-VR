using UnityEngine;

public class GetPlayerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player) && !player.shouldNotMove)
        {
            player.transform.position = new Vector3(other.transform.position.x, GetComponentInParent<Transform>().position.y + 1);
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
