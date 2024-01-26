using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Acest script Unity, gestionează atacurile entității atunci când un obiect intră în contact cu trigger-ul asociat.
 */

public class Attack : MonoBehaviour
{
    // Daunele cauzate de atac
    public int attackDamage = 10;

    // Vectorul de knockback asociat atacului
    public Vector2 knockback = Vector2.zero;

    // Se execută atunci când un alt collider intră în collider-ul acestui obiect
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Obține componente Damageable ale obiectului lovit (dacă există)
        Damageable damageable = collision.GetComponent<Damageable>();

        // Verifică dacă obiectul lovit are componente Damageable
        if (damageable != null)
        {
            // Calculează knockback-ul în funcție de direcția în care este orientat părintele atacului
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            // Aplică daunele și knockback-ul la obiectul lovit
            damageable.Hit(attackDamage, deliveredKnockback);
        }
    }
}
