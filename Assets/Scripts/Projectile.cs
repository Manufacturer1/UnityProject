/*
 * Scriptul Projectile controlează comportamentul proiectilelor în joc.
 * Proiectilele sunt obiecte care se mișcă într-o direcție dată și pot cauza daune entităților cu componente Damageable.
 * 
 * Proprietăți și variabile cheie:
 * - moveSpeed: Viteza inițială a proiectilului în unități pe secundă.
 * - damage: Dauna pe care proiectilul o cauzează entităților cu componente Damageable.
 * - knockback: Forța cu care proiectilul împinge înapoi entitățile lovitte.
 * - maxDistance: Distanța maximă pe care proiectilul o poate parcurge înainte de a se distruge.
 * - rb: Componenta Rigidbody2D a proiectilului pentru manipularea fizicii.
 * - initialPosition: Poziția inițială a proiectilului la momentul instantierii.
 * 
 * Funcții:
 * - Start(): Inițializează viteza proiectilului și direcția sa în momentul instantierii.
 * - OnTriggerEnter2D(Collider2D collision): Detectează coliziunea cu alte obiecte și aplică dauna și knockback-ul entităților Damageable.
 * - Update(): Verifică dacă proiectilul a parcurs o distanță maximă și îl distruge în consecință.
 * 
 * Acest script trebuie atașat la obiectul reprezentând proiectilul.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Viteza proiectilului în unități pe secundă
    public Vector2 moveSpeed = new Vector2(3f, 0);

    // Dauna cauzată de proiectil entităților cu componente Damageable
    public int damage = 10;

    // Forța de împingere înapoi aplicată entităților lovitte
    public Vector2 knockback = new Vector2(0, 0);

    // Distanța maximă pe care proiectilul o poate parcurge înainte de a se distruge
    public float maxDistance = 10f;

    // Componenta Rigidbody2D a proiectilului
    Rigidbody2D rb;

    // Poziția inițială a proiectilului la momentul instantierii
    Vector2 initialPosition;

    // Inițializarea componentelor
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Salvarea poziției inițiale
        initialPosition = transform.position;

        // Inițializarea vitezei proiectilului în funcție de direcția sa
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    // Detectarea coliziunii cu alte obiecte
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Obținerea componentei Damageable a obiectului lovit
        Damageable damageable = collision.GetComponent<Damageable>();

        // Verificarea dacă obiectul lovit are componenta Damageable
        if (damageable != null)
        {
            // Calcularea forței de knockback în funcție de direcția proiectilului
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            // Aplicarea daunei și knockback-ului entității lovitte
            bool gotHit = damageable.Hit(damage, deliveredKnockback);

            // Verificarea dacă entitatea a fost lovită și distrugerea proiectilului în consecință
            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + damage);
                Destroy(gameObject);
            }
        }
    }

    // Verificarea distanței parcurse de proiectil față de poziția inițială
    private void Update()
    {
        // Calcularea distanței parcurse
        float distanceTraveled = Vector2.Distance(initialPosition, transform.position);

        // Verificarea dacă proiectilul a parcurs distanța maximă și distrugerea în consecință
        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
