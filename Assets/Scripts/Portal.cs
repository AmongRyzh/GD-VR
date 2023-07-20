using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] GameModes gameMode;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
            player.currentGameMode = gameMode;
    }
}
