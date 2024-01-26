using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/*
 * Acest script Unity, gestionează detecția coliziunilor într-o zonă specifică.
 */
public class DetectionZone : MonoBehaviour
{
    // Eveniment Unity declanșat atunci când nu există colizoare detectate
    public UnityEvent NoColliders;

    // Lista de colizoare detectate în zona de detecție
    public List<Collider2D> detectedColliders = new List<Collider2D>();

    // Se execută atunci când un obiect intră în zona de detecție
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Adaugă colizorul în lista de colizoare detectate
        detectedColliders.Add(collision);
    }

    // Se execută atunci când un obiect iese din zona de detecție
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Elimină colizorul din lista de colizoare detectate
        detectedColliders.Remove(collision);

        // Verifică dacă nu mai există colizoare detectate în lista
        if (detectedColliders.Count <= 0)
        {
            // Invocă evenimentul NoColliders
            NoColliders.Invoke();
        }
    }
}
