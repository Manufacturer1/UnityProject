using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

/*
 *Acest script Unity, este responsabil pentru gestionarea barei de sănătate a jucătorului în joc.  
 */

public class HealthBar : MonoBehaviour
{
    // Referință la componenta Slider pentru bara de sănătate
    public Slider healthSlider;

    // Referință la componenta TextMeshPro pentru afișarea textului barei de sănătate
    public TMP_Text healthBarText;

    // Referință la componenta Damageable a jucătorului
    Damageable playerDamageable;

    // Se execută la inițializarea obiectului
    private void Awake()
    {
        // Găsește obiectul jucătorului
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Verifică dacă a fost găsit un obiect jucător
        if (player == null)
        {
            Debug.Log("No player found");
        }
        else
        {
            // Obține componenta Damageable a jucătorului
            playerDamageable = player.GetComponent<Damageable>();

            // Verifică dacă componenta Damageable a fost găsită
            if (playerDamageable == null)
            {
                Debug.Log("Damageable component not found on the player GameObject");
            }
        }
    }

    // Se execută la începutul jocului
    void Start()
    {
        // Inițializează valorile barei de sănătate și textului
        healthSlider.value = CalculateSliderPercentage(playerDamageable.Health, playerDamageable.MaxHealh);
        healthBarText.text = "HP " + playerDamageable.Health + " / " + playerDamageable.MaxHealh;
    }

    // Calculează procentajul pentru bara de sănătate
    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    // Se execută atunci când obiectul devine activ
    private void OnEnable()
    {
        // Adaugă metoda OnPlayerHealthChanged la evenimentul healthChanged al jucătorului
        playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    // Se execută atunci când obiectul devine inactiv
    private void OnDisable()
    {
        // Elimină metoda OnPlayerHealthChanged de la evenimentul healthChanged al jucătorului
        playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    // Se execută atunci când sănătatea jucătorului se schimbă
    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        // Actualizează valorile barei de sănătate și textului
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP " + newHealth + " / " + maxHealth;
    }
}
