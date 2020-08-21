using UnityEngine;

public class InflictPainOnContact : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health health;
        if (collision.gameObject.TryGetComponent(out health))
        {
            health.remainingHealth -= damage;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Health health;
        if (collision.gameObject.TryGetComponent(out health))
        {
            health.remainingHealth -= damage;
        }
    }
}
