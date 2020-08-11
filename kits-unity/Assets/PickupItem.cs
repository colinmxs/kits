using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement player;

        if (collision.gameObject.TryGetComponent(out player))
        {
            player.LoseFocus(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player;
        if (collision.gameObject.TryGetComponent(out player))
        {
            player.Focus(gameObject);
        }
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerMovement player;
        
        if (collision.gameObject.TryGetComponent(out player))
        {
            player.LoseFocus(gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement player;
        if (collision.gameObject.TryGetComponent(out player))
        {
            player.Focus(gameObject);
        }
    }
}
