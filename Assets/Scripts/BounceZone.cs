using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BounceZone : MonoBehaviour
{
    [SerializeField] private float bounceForce = 18f;
    private void Reset() => GetComponent<Collider2D>().isTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) player.Bounce(bounceForce);
    }
}
