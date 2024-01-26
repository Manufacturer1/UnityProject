using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    // Referințe la textele pentru daune și vindecare
    public GameObject damageText;
    public GameObject healthText;

    // Referință la canvas
    public Canvas canvas;

    // Metoda care se apelează la trezirea scriptului
    private void Awake()
    {
        // Găsește referința la Canvas în scenă
        canvas = FindObjectOfType<Canvas>();
    }

    // Metoda care se apelează când scriptul devine activ
    private void OnEnable()
    {
        // Abonează metodele la evenimentele de daună și vindecare
        CharacterEvents.characterDamaged += CharacterTookDamage;
        CharacterEvents.characterHealed += CharacterHealed;
    }

    // Metoda care se apelează când scriptul devine inactiv
    private void OnDisable()
    {
        // Dezabonează metodele de la evenimentele de daună și vindecare
        CharacterEvents.characterDamaged -= CharacterTookDamage;
        CharacterEvents.characterHealed -= CharacterHealed;
    }

    // Metoda apelată când un caracter primește daune
    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        // Converteste poziția caracterului în coordonate de ecran
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        // Instantierea unui nou text de daune
        TMP_Text tmpText = Instantiate(damageText, spawnPosition, Quaternion.identity, canvas.transform).GetComponent<TMP_Text>();

        // Setarea textului cu valoarea daunelor primite
        tmpText.text = damageReceived.ToString();
    }

    // Metoda apelată când un caracter este vindecat
    public void CharacterHealed(GameObject character, int healthRestored)
    {
        // Converteste poziția caracterului în coordonate de ecran
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        // Instantierea unui nou text de vindecare
        TMP_Text tmpText = Instantiate(healthText, spawnPosition, Quaternion.identity, canvas.transform).GetComponent<TMP_Text>();

        // Setarea textului cu valoarea vindecării primite
        tmpText.text = healthRestored.ToString();
    }

    // Metoda apelată la apăsarea butonului de ieșire din joc
    public void OnExitGame(InputAction.CallbackContext context)
    {
        // Verificarea dacă butonul a fost apăsat (început)
        if (context.started)
        {
            // Mesaj de debug pentru informarea în editor sau într-un build de dezvoltare
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif

            // Verificarea tipului de build și efectuarea acțiunilor corespunzătoare
#if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;  // Oprește rularea în editor
#elif (UNITY_STANDALONE)
                Application.Quit();  // Închide aplicația pe platformele standalone (de exemplu, PC)
#elif (UNITY_WEBGL)
                SceneManager.LoadScene("QuitScene");  // Încarcă o scenă specială pentru WebGL (închidere)
#endif
        }
    }
}
