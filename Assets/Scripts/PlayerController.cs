using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Acest script Unity, gestionează controlul jucătorului. 
 */

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    // Variabile pentru setarea proprietăților jucătorului
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpImpulse = 10f;
    public float airWalkSpeed = 3f;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCoolDown = 1f;

    private bool doubleJump = false;
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private TrailRenderer tr;

    // Componente asociate
    TouchingDirections touchingDirections;
    Damageable damageable;

    // Proprietăți care returnează viteza curentă de deplasare a jucătorului
    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airWalkSpeed;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    Vector2 moveInput;
    [SerializeField] private bool _isMoving = false;

    // Proprietate care indică dacă jucătorul se mișcă
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField] private bool _isRunning = false;

    // Proprietate care indică dacă jucătorul rulează
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    // Proprietate care indică dacă jucătorul se uită la dreapta
    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public bool _isFacingRight = true;

    Rigidbody2D rb;
    Animator animator;

    // Proprietate care indică dacă jucătorul poate să se miște
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    // Proprietate care indică dacă jucătorul este în viață
    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (!damageable.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    // Metoda care gestionează input-ul pentru deplasare
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    // Metoda care setează direcția în care se uită jucătorul
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    // Metoda care gestionează input-ul pentru a alerga
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    // Metoda care gestionează input-ul pentru a sari
    public void OnJump(InputAction.CallbackContext context)
    {
        // TODO: Verificați dacă este în viață
        if (IsAlive)
        {
            if (context.started && CanMove)
            {
                if (touchingDirections.IsGrounded)
                {
                    animator.SetTrigger(AnimationStrings.jump);
                    rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
                    doubleJump = false;
                }
                else if (!doubleJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
                    doubleJump = true;
                }
            }
        }
    }

    // Metoda care gestionează input-ul pentru a ataca
    public void onAtack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attack);
        }
    }

    // Metoda care gestionează input-ul pentru a ataca la distanță
    public void onRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    // Metoda care gestionează efectele atunci când jucătorul este lovit
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    // Coroutine pentru efectul de dash
    private IEnumerator Dash()
    {
        canDash = false;// Setează flag-ul canDash pe false pentru a preveni activarea unui nou dash în timpul executării dash-ului curent.
        isDashing = true;// Marchează că jucătorul este în stare de dash.
        float originalGravity = rb.gravityScale;// Salvează gravitatea originală a jucătorului într-o variabilă locală.
        rb.gravityScale = 0f;// Desactivează gravitatea jucătorului în timpul dash-ului, permițându-i să se deplaseze orizontal fără influența gravitației.

        /*
         * Setează viteza jucătorului pe axa orizontală pentru a realiza dash-ul.
            Direcția dash-ului este determinată de scala locală a jucătorului, astfel încât dacă jucătorul
            se uită la dreapta (transform.localScale.x > 0),
            dash-ul va fi în direcția pozitivă a axei x; altfel, dash-ul va fi în direcția negativă a axei x.
         */
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);

        tr.emitting = true;//Activează emisia pentru TrailRenderer asociat pentru a vizualiza efectul de urmă a dash-ului.
        yield return new WaitForSeconds(dashingTime);//Așteaptă pentru o perioadă specificată dashingTime, reprezentând durata dash-ului.
        tr.emitting = false;//Dezactivează emisia pentru TrailRenderer pentru a opri urmărirea dash-ului după expirarea dashingTime.
        rb.gravityScale = originalGravity;// Restabilește gravitatea la valoarea originală pentru a reveni la comportamentul normal sub influența gravitației.
        isDashing = false;// Marchează că jucătorul nu mai este în stare de dash.
        yield return new WaitForSeconds(dashingCoolDown);//Așteaptă pentru o perioadă specificată dashingCoolDown înainte de a activa posibilitatea de a executa un alt dash.
        canDash = true;// Setează flag-ul canDash pe true, permițând jucătorului să execute un nou dash după ce cooldown-ul a expirat.
    }
    /*
     * yield return este folosit pentru a suspenda temporar execuția funcției, salvând starea acesteia, și pentru a permite unei alte părți a codului să fie executată 
     * (cum ar fi actualizarea frame-ului sau așteptarea unui anumit interval de timp) înainte de a reveni la execuția funcției.
     */


    // Metoda care gestionează input-ul pentru a executa dash-ul
    public void Dash(InputAction.CallbackContext context)
    {
        if (isDashing)
        {
            return;
        }
        if (context.started && touchingDirections.IsGrounded && CanMove && canDash && IsMoving)
        {
            /*
             * StartCoroutine(Dash()) este o instrucțiune folosită în Unity pentru a începe execuția unei coroutine.
             * Coroutine-urile sunt utilizate pentru a gestiona sarcini care necesită o execuție împărțită în mai multe frame-uri și pot implica 
             * așteptarea unui interval de timp, animații, sau alte operațiuni care necesită o execuție treptată.
             */
            StartCoroutine(Dash());
        }
    }
}
