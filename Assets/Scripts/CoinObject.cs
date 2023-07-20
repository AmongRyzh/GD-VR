using UnityEngine;

public class CoinObject : MonoBehaviour
{
    public Coin coinScriptableObject;
    [SerializeField] Material collectedMaterial;
    [SerializeField] AudioClip collectedSound;

    private void Start()
    {
        if (coinScriptableObject != null && coinScriptableObject.isCollected)
        {
            GetComponent<MeshRenderer>().material = collectedMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() && !coinScriptableObject.isCollected)
        {
            coinScriptableObject.isCollected = true;
            GetComponent<MeshRenderer>().material = collectedMaterial;
            FindObjectOfType<AudioSource>().PlayOneShot(collectedSound);
        }
    }
}
