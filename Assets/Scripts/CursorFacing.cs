using UnityEngine;

public class CursorFacing : MonoBehaviour
{
    private float moveInput; // Variabilă pentru stocarea inputului de mișcare pe axa orizontală

    private bool facingRight = true; // Variabilă pentru a ține evidența direcției în care se uită jucătorul (true - dreapta, false - stânga)
    private SpriteRenderer spriteRenderer; // Referință la componenta SpriteRenderer a jucătorului

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obține referința la componenta SpriteRenderer a jucătorului la începutul jocului
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the player!"); // Afișează un mesaj de eroare dacă nu este găsită componenta SpriteRenderer
        }
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal"); // Obține inputul de mișcare pe axa orizontală (A și D sau săgeți stânga/dreapta)

        FlipSprite(); // Verifică și inversează sprite-ul în funcție de inputul de mișcare
    }

    void FlipSprite()
    {
        if (moveInput > 0 && !facingRight || moveInput < 0 && facingRight)
        {
            // Verifică dacă jucătorul se mișcă spre dreapta, dar se uită spre stânga sau se mișcă spre stânga, dar se uită spre dreapta
            facingRight = !facingRight; // Inversează direcția în care se uită jucătorul

            // Inversează sprite-ul folosind componenta SpriteRenderer
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = !facingRight;
            }
        }
    }
}
