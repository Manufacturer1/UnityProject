using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * "estomparea" se referă la procesul de a face treptat un obiect sau o imagine să devină mai transparent, 
 * până când nu mai este vizibil. 
 * Acest efect poate fi realizat prin ajustarea componentei alpha a culorii unui obiect grafic.
 */
public class FadeRemoveBehaviour : StateMachineBehaviour
{
    public float fadeTime = 0.5f;//Timpul în secunde în care estompare ar trebui să aibă loc.
    private float timeElapsed = 0;//Timpul petrecut de la începerea estomparii.
    public float fadeDelay = 0.0f;// O întârziere înainte ca estomparea să înceapă
    private float fadeDelayElapsed = 0f;//Timpul petrecut înainte de începerea estomparii.
    SpriteRenderer spriteRenderer;// Referință la componenta SpriteRenderer a obiectului.
    GameObject objToRemove;//Referință la obiectul care va fi eliminat.
    Color startColor;//Culoarea de pornire a SpriteRenderer

   
    //Este apelată când starea începe să fie evaluată.
    //Inițializează variabilele și salvează culoarea de pornire a SpriteRenderer.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        objToRemove = animator.gameObject;
        startColor = spriteRenderer.color;
    }

    //Este apelată în fiecare cadru între OnStateEnter și OnStateExit.
    //Verifică dacă a trecut intervalul de întârziere(fadeDelay). Dacă nu, crește fadeDelayElapsed.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (fadeDelay > fadeDelayElapsed)
        {
            fadeDelayElapsed += Time.deltaTime;
        }
        //Dacă intervalul de întârziere a trecut, începe procesul de estompare.
        else
        {
            timeElapsed += Time.deltaTime;

            //Ajustează alpha (transparența) în funcție de timpul scurs și actualizează culoarea SpriteRenderer.

            float newAlpha = startColor.a * (1 - timeElapsed / fadeTime);

            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            //După ce a trecut intervalul de estompare (fadeTime), distruge obiectul (Destroy(objToRemove)).

            if (timeElapsed > fadeTime)
            {
                Destroy(objToRemove);
            }
        }

        
    }
}
