using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class Knight : MonoBehaviour
{
    public float walkAcceleration = 3f; // Accelerarea în timpul mersului
    public float maxSpeed = 3f; // Viteza maximă de deplasare
    public float walkStopRate = 0.05f; // Rata la care personajul încetinește când nu se mișcă
    public DetectionZone attackZone; // Zona de detecție a atacului
    public DetectionZone cliffDetection; // Zona de detecție a stâncilor/clifurilor

    Rigidbody2D rb; // Referință la componenta Rigidbody2D
    TouchingDirections touchingDirections; // Referință la scriptul TouchingDirections
    Animator animator; // Referință la componenta Animator
    Damageable damageable; // Referință la scriptul Damageable

    public enum walkableDirection { Right, Left }; // Direcții în care poate merge personajul

    private walkableDirection _walkDirection; // Direcția în care se mișcă personajul
    private Vector2 walkDirectionVector = Vector2.right; // Vectorul care indică direcția în care se mișcă personajul

    public walkableDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == walkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == walkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float AttackCoolDown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCoolDown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.attackCoolDown, Mathf.Max(value, 0));
        }
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void FlipDirection()
    {
        if (WalkDirection == walkableDirection.Right)
        {
            WalkDirection = walkableDirection.Left;
        }
        else if (WalkDirection == walkableDirection.Left)
        {
            WalkDirection = walkableDirection.Right;
        }
        else
        {
            Debug.LogError("error");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCoolDown > 0)
        {
            AttackCoolDown -= Time.deltaTime;
        }

        if (CanMove && touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }

        // Resume normal movement
        if (!damageable.LockVelocity)
        {
            if (CanMove && touchingDirections.IsGrounded)
            {
                rb.velocity = new Vector2(
                    Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.deltaTime), -maxSpeed, maxSpeed), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }
    }
}
