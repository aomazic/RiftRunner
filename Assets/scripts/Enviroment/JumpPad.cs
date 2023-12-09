using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float jumpForce = 10f;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] float soundVolume = 1f;
    private bool playerOnPad = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerOnPad)
        {
            playerOnPad = true;
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;

              
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                if (jumpSound != null)
                {
                    AudioSource.PlayClipAtPoint(jumpSound, transform.position, soundVolume);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPad = false;
        }
    }
}
