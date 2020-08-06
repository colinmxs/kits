using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement player;
        if (collision.gameObject.TryGetComponent(out player))
        {
            Debug.Log("PickUp");
            var pickedUp = player.PickUp(gameObject);
            if (pickedUp)
            {
                var rigidBody = GetComponent<Rigidbody2D>();
                rigidBody.simulated = false;
            }
            
        }
    }
}
