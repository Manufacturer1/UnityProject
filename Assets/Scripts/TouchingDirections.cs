using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Acest script Unity, numit TouchingDirections, gestionează detecția contactului cu diferite suprafețe, cum ar fi solul, pereții și tavanul
 */

public class TouchingDirections : MonoBehaviour
{
    // Referință la collider-ul capsulei folosit pentru detecție
    CapsuleCollider2D touchingCol;

    // Referință la animatorul obiectului pentru a controla animațiile
    Animator animator;

    // Filtrele de contact pentru detecția solului, pereților și tavanului
    public ContactFilter2D castFilter;

    // Rezultatele detecției pentru sol, pereți și tavan
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    // Distanța de detecție pentru sol, pereți și tavan
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;

    // Starea de a fi pe sol
    [SerializeField]
    private bool _isGrounded = true;

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    // Starea de a fi pe perete
    [SerializeField]
    private bool _isOnWall = true;

    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    // Starea de a fi pe tavan
    [SerializeField]
    private bool _isOnCeiling = true;

    // Direcția de verificare pentru pereți
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    // Se execută la începutul jocului
    private void Awake()
    {
        // Obține componente la începutul jocului
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Se execută în fiecare cadru fizic
    void FixedUpdate()
    {
        // Verifică dacă obiectul este pe sol
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;

        // Verifică dacă obiectul este pe perete
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;

        // Verifică dacă obiectul este pe tavan
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}
