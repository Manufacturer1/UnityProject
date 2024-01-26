using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Acest script implementează un obiect de colectare a vieții (health pickup) care restaurează viața jucătorului sau a altor obiecte cu componente 
 * Damageable atunci când intră în contact cu acesta. Este adăugată o rotație de învârtire continuă pentru a oferi un aspect vizual plăcut.
 */


public class HealthPickUp : MonoBehaviour
{
    public int healthRestore = 20; // Câți puncte de viață să se restaureze la colectarea pickup-ului
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0); // Viteza de rotație a pickup-ului

    AudioSource pickupSource; // Referință la componenta AudioSource

    private void Awake()
    {
        pickupSource = GetComponent<AudioSource>(); // Obține componenta AudioSource atașată obiectului
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>(); // Obține componenta Damageable a obiectului care a intrat în trigger

        if (damageable && damageable.Health < damageable.MaxHealh)
        {
            bool wasHealed = damageable.Heal(healthRestore); // Încearcă să vindece obiectul dacă nu este la viața maximă

            if (wasHealed)
            {
                if (pickupSource)
                {
                    AudioSource.PlayClipAtPoint(pickupSource.clip, gameObject.transform.position, pickupSource.volume);
                    // Redă sunetul corespunzător pickup-ului la poziția sa și cu volumul său specificat
                }
                Destroy(gameObject); // Distruge obiectul după colectare
            }
        }
    }

    private void Update()
    {
        // Rotirea pickup-ului în jurul axei sale în timpul fiecărui cadru
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
