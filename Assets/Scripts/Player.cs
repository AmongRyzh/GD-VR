using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum GameModes
{
    Cube,
    Ship
}

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform deathCheck;
    [SerializeField] Transform cubeOnlyDeathCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask blockLayer;
    public InputActionProperty jumpAction;
    [HideInInspector] public bool shouldNotMove = false;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] AudioClip completedAudio;
    Rigidbody rb;
    [HideInInspector] public GameModes currentGameMode;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!shouldNotMove)
        {
            transform.position += Time.deltaTime * new Vector3(speed, 0);

            Invoke(currentGameMode.ToString(), 0);
        }
    }
    
    void Cube()
    {
        Physics.gravity = new Vector3(0, -50);

        if (rb.velocity.y < -20)
        {
            rb.velocity = new Vector3(rb.velocity.x, -20);
        }
        else if (rb.velocity.y > 20)
        {
            rb.velocity = new Vector3(rb.velocity.x, 20);
        }

        if (jumpAction.action.IsPressed() && IsGrounded())
            Jump();
    }

    void Ship()
    {
        if (jumpAction.action.IsPressed())
        {
            Physics.gravity = new Vector3(0, 25);
        }
        else
        {
            Physics.gravity = new Vector3(0, -25);
        }

        if (rb.velocity.y < -10)
        {
            rb.velocity = new Vector3(rb.velocity.x, -10);
        }
        else if (rb.velocity.y > 10)
        {
            rb.velocity = new Vector3(rb.velocity.x, 10);
        }
    }
    private void Jump()
    {
        rb.velocity = Vector3.up * jumpForce;
    }

    bool IsGrounded()
    {
        return Physics.OverlapBox(groundCheck.position, new Vector3(1, .1f, 1), 
            Quaternion.identity, groundLayer).Length != 0;
    }

    bool IsCollidingRespawn()
    {
        Collider[] colliders = Physics.OverlapBox(deathCheck.position, new Vector3(.1f, .8f, .1f),
            Quaternion.identity, blockLayer);
        print($"respawn cols: {colliders.Length}");
        return colliders.Length > 0;
    }

    bool IsCollidingCubeOnlyRespawn()
    {
        Collider[] colliders = Physics.OverlapBox(cubeOnlyDeathCheck.position, new Vector3(1, .1f, 1),
            Quaternion.identity, blockLayer);
        print($"cube only respawn cols: {colliders.Length}");
        return colliders.Length > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(1, .1f, 1));
        Gizmos.DrawWireCube(deathCheck.position, new Vector3(.1f, .8f, .1f));
        Gizmos.DrawWireCube(cubeOnlyDeathCheck.position, new Vector3(1, .1f, 1));
    }

    private IEnumerator OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Respawn") || IsCollidingRespawn())
        {
            print("collided respawn :)");
            StartCoroutine(Die());
            yield return null;
        }
        else if (IsCollidingCubeOnlyRespawn() && currentGameMode == GameModes.Cube)
        {
            print("collided cube only respawn :)");
            StartCoroutine(Die());
            yield return null;
        }
        else if (collision.collider.CompareTag("Finish"))
        {
            StartCoroutine(Finish());
            yield return null;
        }
    }

    private IEnumerator Die()
    {
        if (!shouldNotMove)
        {
            shouldNotMove = true;
            FindObjectOfType<AudioSource>().Stop();
            FindObjectOfType<AudioSource>().PlayOneShot(deathAudio);
            rb.isKinematic = true;

            yield return new WaitForSeconds(0.5f);

            FadeScreen fadeScreen = FindObjectOfType<FadeScreen>();
            fadeScreen.FadeOut();
            yield return new WaitForSeconds(fadeScreen.fadeDuration);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator Finish()
    {
        if (!shouldNotMove)
        {
            shouldNotMove = true;
            FindObjectOfType<AudioSource>().Stop();
            FindObjectOfType<AudioSource>().PlayOneShot(completedAudio);
            rb.isKinematic = true;
            FindObjectOfType<LevelSavingManager>().SaveCoinData();

            yield return new WaitForSeconds(0.5f);

            FadeScreen fadeScreen = FindObjectOfType<FadeScreen>();
            fadeScreen.FadeOut();
            yield return new WaitForSeconds(fadeScreen.fadeDuration);
            SceneManager.LoadScene(2);
        }
    }
}
