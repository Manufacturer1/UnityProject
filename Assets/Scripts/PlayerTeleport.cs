using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTeleport : MonoBehaviour
{

    private GameObject TeleportCurent;

     void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (TeleportCurent != null)
            {
                transform.position = TeleportCurent.GetComponent<Teleporter>().GetDestination().position;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            TeleportCurent = collision.gameObject;
        }    
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        { 
        if(collision.gameObject ==  TeleportCurent)
            {
                TeleportCurent = null;
            }
        }
    }

}
