using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Acest script gestionează afișarea și animarea unui text care se mișcă în sus și se estompează treptat după o anumită perioadă de timp (timeToFade).
 * Textul este reprezentat de componenta TextMeshProUGUI. 
 * Proprietatea moveSpeed controlează viteza de deplasare a textului, iar timeToFade determină cât timp durează procesul de fading.
 */

public class HealthText : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0, 80, 0); // Viteza de deplasare a textului
    RectTransform textTransform; // Referință la componenta RectTransform pentru manipularea poziției textului
    public float timeToFade = 1f; // Timpul în secunde pentru a face textul să dispară
    private float timeElapsed = 0f; // Timpul scurs de la începutul afișării textului
    TextMeshProUGUI textMeshPro; // Referință la componenta TextMeshProUGUI pentru manipularea textului afișat

    private Color startColor; // Culoarea de început a textului

    private void Awake()
    {
        textTransform = GetComponent<RectTransform>(); // Obține componenta RectTransform atașată obiectului
        textMeshPro = GetComponent<TextMeshProUGUI>(); // Obține componenta TextMeshProUGUI atașată obiectului
        startColor = textMeshPro.color; // Salvează culoarea de început a textului
    }

    private void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime; // Deplasează textul în funcție de viteza specificată și timpul scurs

        timeElapsed += Time.deltaTime; // Actualizează timpul scurs

        if (timeElapsed < timeToFade)
        {
            // Calculează opacitatea în funcție de timpul scurs pentru a face textul să dispară treptat
            float fadeAlpha = startColor.a * (1 - timeElapsed / timeToFade);
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
        else
        {
            Destroy(gameObject); // Distruge obiectul (textul) după ce a trecut timpul de fading
        }
    }
}
