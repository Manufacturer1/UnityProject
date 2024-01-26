using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/*
 * Acest script Unity, gestionează comportamentul unei entități care poate suferi daune. 
 */

public class Damageable : MonoBehaviour
{
    // Eveniment invocat atunci când entitatea suferă o lovitură, transmitând daune și vectorul de knockback
    public UnityEvent<int, Vector2> damageableHit;

    // Eveniment invocat atunci când entitatea moare
    public UnityEvent damageableDeath;

    // Eveniment invocat atunci când sănătatea entității este modificată, transmitând noua valoare a sănătății și sănătatea maximă
    public UnityEvent<int, int> healthChanged;

    // Sănătatea maximă a entității
    [SerializeField] private int _maxHealth = 100;

    // Referință la animatorul entității
    Animator animator;

    private void Awake()
    {
        // Obține componenta Animator la începutul jocului
        animator = GetComponent<Animator>();
    }

    // Starea de a fi în viață
    [SerializeField] private bool _isAlive = true;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            // Actualizează starea de a fi în viață și activează/dezactivează animația corespunzătoare
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);

            // Dacă entitatea nu mai este în viață, invocă evenimentul de moarte
            if (value == false)
            {
                damageableDeath.Invoke();
            }
        }
    }

    // Sănătatea maximă expusă public
    public int MaxHealh
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    // Blocarea vitezei în animator
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    // Sănătatea curentă și setarea invincibilității
    [SerializeField] private int _health = 100;
    [SerializeField] private bool isInvincible = true;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    // Proprietate pentru accesul și modificarea sănătății
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            // Actualizează sănătatea și invocă evenimentul de modificare a sănătății
            _health = value;
            healthChanged?.Invoke(_health, MaxHealh);

            // Dacă sănătatea ajunge la sau sub 0, entitatea nu mai este în viață
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    // Se execută în fiecare cadru
    private void Update()
    {
        // Gestionează timpul de invincibilitate
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    // Funcție pentru ca entitatea să primească o lovitură
    public bool Hit(int damage, Vector2 knockback)
    {
        // Verifică dacă entitatea este în viață și nu este invincibilă
        if (IsAlive && !isInvincible)
        {
            // Aplică daunele, activează invincibilitatea și declanșează animația de lovitură
            Health -= damage;
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hit);
            LockVelocity = true;

            // Invocă evenimentele corespunzătoare
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        return false;
    }

    // Funcție pentru vindecarea entității
    public bool Heal(int healthRestore)
    {
        // Verifică dacă entitatea este în viață și sănătatea nu a atins maximul
        if (IsAlive && Health < MaxHealh)
        {
            // Calculează cantitatea reală de vindecare și actualizează sănătatea
            int maxHeal = Mathf.Max(MaxHealh - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;

            // Invocă evenimentul de vindecare
            CharacterEvents.characterHealed.Invoke(gameObject, actualHeal);
            return true;
        }
        return false;
    }
}