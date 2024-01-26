using System.Collections.Generic;
using UnityEngine;

/*
 * Acest script implementeaza comportamentul unui obiect (in acest caz, un ochi zburator) care urmareste jucatorul 
 * si incearca sa se apropie de acesta. Scriptul foloseste un algoritm simplu de cautare A* pentru a determina un traseu catre pozitia jucatorului.
 */


public class flyEye : MonoBehaviour
{
    // Lista de puncte reprezentand traseul catre jucator si un traseu random
    private List<Vector2> PathToTarget = new List<Vector2>();
    private List<Vector2> RandomPath = new List<Vector2>();
    private List<Vector2> CurrentPath = new List<Vector2>();



    // Referinta la componenta PathFinder pentru calcularea traseelor
    private PathFinder PathFinder;

    //Variablica care tine cont de raza de cautare
    public float detectionRadius = 10f;
 

    // Starea de vizibilitate a jucatorului
    private bool SeeTarget;

    // Starea de miscare a ochiului
    private bool isMoving;

    // Referinta la obiectul jucătorului
    public GameObject Player;

    // Variabilă pentru viteza de zbor
    public float flightSpeed = 15f;

    // Componente necesare pentru functionarea obiectului
    public Collider2D deathCollider;
    public DetectionZone byteDetectionZone;

    // Referinte catre componente
    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

    // Variabila pentru verificarea daca exista tinta
    public bool _hasTarget = false;

    // Proprietate pentru a obtine si seta starea de tinta
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    // Proprietate pentru a obtine starea de miscare
    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    // Metoda apelata la initializarea obiectului
    void Awake()
    {
        // Obtine referinte catre componente
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();

    }






    // Metoda apelata la inceputul jocului
    void Start()
    {
        // Verifica daca exista un jucator asignat
        if (Player != null)
        {
            // Obtine referinta catre componenta PathFinder
            PathFinder = GetComponent<PathFinder>();

            ReCalculatePath();
            // Seteaza starea de miscare la adevarat
            isMoving = true;
        }
    }
    public void ReCalculatePath()
    {

        // Verifică dacă jucătorul este în raza specificată
        if (Vector2.Distance(transform.position, Player.transform.position) <= detectionRadius)
        {
            // Obține un traseu către poziția jucătorului
            PathToTarget = PathFinder.GetPath(Player.transform.position);

            // Verifică dacă nu există un traseu către jucător
            if (PathToTarget.Count == -1)
            {
                // Setează starea de vizibilitate la fals
                SeeTarget = false;

                // Dacă nu vede jucătorul, alege un nod random și obține un traseu
                if (!SeeTarget)
                {
                    var r = UnityEngine.Random.Range(0, PathFinder.FreeNodes.Count);
                    RandomPath = PathFinder.GetPath(PathFinder.FreeNodes[r].Position);
                    CurrentPath = RandomPath;
                    print(CurrentPath.Count);
                }
            }
            else
            {
                // Dacă există un traseu către jucător, setează traseul curent
                CurrentPath = PathToTarget;
                SeeTarget = true;
            }

            // Setează starea de mișcare la adevărat pentru a urmări jucătorul
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }




    // Metoda pentru actualizarea directiei de privire
    private void UpdateDirection()
    {
        Vector3 currentLocalScale = transform.localScale;

        // Verifica daca exista puncte in traseu
        if (CurrentPath.Count > 0)
        {
            // Calculeaza directia de miscare pe baza ultimului punct din traseu
            Vector2 moveDirection = (CurrentPath[CurrentPath.Count - 1] - (Vector2)transform.position).normalized;

            // Obtine componenta x a directiei
            float moveX = moveDirection.x;

            // Flip-uieste sprite-ul in functie de directie
            if (moveX > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(currentLocalScale.x), currentLocalScale.y, currentLocalScale.z);
            }
            else if (moveX < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(currentLocalScale.x), currentLocalScale.y, currentLocalScale.z);
            }
        }
    }


    // Metoda apelata in fiecare cadru
    void Update()
    {
        // Verifica daca exista o tinta in zona de detectie
        HasTarget = byteDetectionZone.detectedColliders.Count > 0;

        // Actualizeaza directia de privire
        UpdateDirection();


        // Verifica daca jucatorul exista
        if (Player == null) return;

        // Verifica daca obiectul este viu
        if (damageable.IsAlive)
        {
            // Verifica daca obiectul poate sa se miste
            if (CanMove)
            {
                // Verifica daca traseul curent este gol si distanta pâna la jucator este mai mare decât o valoare mica
                if (CurrentPath.Count == 0 && Vector2.Distance(transform.position, Player.transform.position) > 0.5f)
                {

                    // Recalculeaza traseul
                    ReCalculatePath();

                    // Seteaza starea de miscare la adevarat
                    isMoving = true;
                }

                // Verifica daca traseul curent este gol
                if (CurrentPath.Count == 0)
                {

                    return;
                }

                // Verifica daca obiectul este in miscare
                if (isMoving)
                {
                    // Verifica daca distanta pâna la ultimul punct din traseu este mai mare decât o valoare mica
                    if (Vector2.Distance(transform.position, CurrentPath[CurrentPath.Count - 1]) > 0.1f)
                    {
                        // Misca obiectul catre ultimul punct din traseu
                        transform.position = Vector2.MoveTowards(transform.position, CurrentPath[CurrentPath.Count - 1], flightSpeed * Time.deltaTime);
                    }
                    else
                    {
                        // Opreste miscarea când ajunge la destinatie
                        rb.velocity = Vector3.zero;
                    }

                    // Verifica daca obiectul a ajuns la destinatie
                    if (Vector2.Distance(transform.position, CurrentPath[CurrentPath.Count - 1]) <= 0.1f)
                    {
                        // Seteaza starea de miscare la fals
                        isMoving = false;
                    }
                }
                else
                {
                    // Daca obiectul nu este in miscare, recalculeaza traseul
                    ReCalculatePath();

                    // Seteaza starea de miscare la adevarat
                    isMoving = true;
                }
            }
        }
    }
}
