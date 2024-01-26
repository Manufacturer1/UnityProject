using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    public Color gizmoColor = Color.red;

    // Lista de vectori ce reprezinta calea catre tinta
    public List<Vector2> PathToTarget;

    // Set de noduri verificate
    public HashSet<Node> CheckedNodes = new HashSet<Node>();

    // Lista de noduri libere
    public List<Node> FreeNodes = new List<Node>();

    // Coada de prioritati pentru noduri in asteptare
    PriorityQueue<Node> WaitingNodes = new PriorityQueue<Node>();

    // Tinta la care se doreste ajungerea
    public GameObject Target;

    // Masca de coliziune pentru a determina daca un nod este solid
    public LayerMask SolidLayer;

    // Raza pentru cautarea caii
    public float pathfindingRadius = 10f;

    // Numarul maxim de iteratii pentru cautarea caii
    public int maxIterations = 1000; // Se seteaza un numar maxim rezonabil de iteratii

    // Distanta maxima pentru desenarea gizmo-urilor
    public float maxGizmoDistance = 50f; // Se seteaza o limita de distanta pentru desenarea gizmo

    // Lista de vecini predefiniti pentru un nod
    //Lista Neighbours este declarată ca fiind statică pentru a asigura că aceasta este partajată între toate instanțele de obiect ale clasei 
    private static readonly List<Node> Neighbours = new List<Node>
    {
        new Node(1, new Vector2(-1, 0), Vector2.zero, null),
        new Node(1, new Vector2(1, 0), Vector2.zero, null),
        new Node(1, new Vector2(0, -1), Vector2.zero, null),
        new Node(1, new Vector2(0, 1), Vector2.zero, null),
        new Node(1, new Vector2(-1, -1), Vector2.zero, null),
        new Node(1, new Vector2(-1, 1), Vector2.zero, null),
        new Node(1, new Vector2(1, -1), Vector2.zero, null),
        new Node(1, new Vector2(1, 1), Vector2.zero, null)
    };

    // Metoda apelata la fiecare frame
    void Update()
    {
        // Pozitia tinte
        Vector2 targetPosition = Target.transform.position;

        // Pozitia curenta
        Vector2 currentPosition = transform.position;

        // Distanta pana la tinta
        float distanceToTarget = Vector2.Distance(currentPosition, targetPosition);

        // Verifica daca distanta este in raza de cautare a caii
        if (distanceToTarget <= pathfindingRadius)
        {
            // Obține calea către țintă și afiseaza numarul de puncte in consola
            //PathToTarget = GetPath(targetPosition);
            Debug.Log("PathToTarget Count: " + PathToTarget.Count);
        }
        else
        {
            // Daca nu este in raza de cautare, curata calea
            PathToTarget.Clear();
        }
    }

    // Metoda pentru obtinerea caii catre tinta
    public List<Vector2> GetPath(Vector2 target)
    {
        // Initializare variabile si structuri de date
        PathToTarget = new List<Vector2>();
        CheckedNodes.Clear();
        WaitingNodes.Clear();
        Vector2 startPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 targetPosition = new Vector2(Mathf.Round(target.x), Mathf.Round(target.y));

        // Daca pozitia de start si cea de tinta sunt aproape, returneaza calea goala
        if (Vector2.Distance(startPosition, targetPosition) < 0.5f)
            return PathToTarget;

        // Creaza nodul de start
        Node startNode = new Node(0, startPosition, targetPosition, null);
        CheckedNodes.Add(startNode);
        List<Node> neighbours = GetNeighbourNodes(startNode);
        WaitingNodes.Enqueue(startNode, startNode.F);
        foreach (var neighbour in neighbours)
        {
            WaitingNodes.Enqueue(neighbour, neighbour.F);
        }
        int iterations = 0;
        while (WaitingNodes.Count > 0 && iterations < maxIterations)
        {
            Node nodeToCheck = WaitingNodes.Dequeue();

            // Verifica daca nodul curent coincide cu pozitia tinte
            if (nodeToCheck.Position == targetPosition)
            {
                // Calculeaza calea de la nodul gasit pana la nodul de start
                return CalculatePathFromNode(nodeToCheck);
            }

            // Verifica daca pozitia este traversabila (nu este solida)
            var walkable = !Physics2D.OverlapCircle(nodeToCheck.Position, 0.8f, SolidLayer);

            // Trateaza cazurile de noduri traversabile si noduri solide
            if (!walkable)
            {
                CheckedNodes.Add(nodeToCheck);
            }
            else if (walkable)
            {
                WaitingNodes.Remove(nodeToCheck);
                if (!CheckedNodes.Where(x => x.Position == nodeToCheck.Position).Any())
                {
                    CheckedNodes.Add(nodeToCheck);
                    neighbours = GetNeighbourNodes(nodeToCheck);
                    foreach (var neighbour in neighbours)
                    {
                        WaitingNodes.Enqueue(neighbour, neighbour.F);
                    }
                }
                iterations++;
            }
        }

        // Actualizeaza lista de noduri libere si returneaza calea gasita sau goala
        FreeNodes = new List<Node>(CheckedNodes);
        return PathToTarget;
    }

    // Calculeaza calea de la un nod dat catre nodul de start
    List<Vector2> CalculatePathFromNode(Node node)
    {
        var path = new List<Vector2>();
        Node currentNode = node;

        // Construieste calea prin parcurgerea nodurilor anterioare
        while (currentNode.PreviousNode != null)
        {
            path.Add(new Vector2(currentNode.Position.x, currentNode.Position.y));
            currentNode = currentNode.PreviousNode;
        }
        return path;
    }

    // Obtine nodurile vecine pentru un nod dat
    List<Node> GetNeighbourNodes(Node node)
    {
        var neighbours = new List<Node>();

        // Itereaza prin vecinii predefiniti si creeaza noduri cu pozitii actualizate
        foreach (var neighbour in Neighbours)
        {
            Node newNeighbour = new Node(neighbour.G + node.G,
                                         new Vector2(Mathf.Round(node.Position.x + neighbour.Position.x),
                                                     Mathf.Round(node.Position.y + neighbour.Position.y)),
                                         node.TargetPosition, node);

            neighbours.Add(newNeighbour);
        }

        return neighbours;
    }

    // Metoda pentru desenarea gizmo-urilor in editor
    void OnDrawGizmos()
    {
        // Deseneaza nodurile verificate cu culoare galbena
        foreach (var item in CheckedNodes)
        {
            if (Vector2.Distance(item.Position, transform.position) < maxGizmoDistance)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(new Vector2(item.Position.x, item.Position.y), 0.1f);
            }
        }

        // Deseneaza calea catre tinta cu culoare rosie
        foreach (var item in PathToTarget)
        {
            if (Vector2.Distance(item, transform.position) < maxGizmoDistance)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawSphere(new Vector2(item.x, item.y), 0.2f);
            }
        }
    }
}
