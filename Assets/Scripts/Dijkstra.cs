using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    public List<Vector2> PathToTarget; // Calea de la pozitia curenta la tinta
    public HashSet<NodeDijkstra> CheckedNodes = new HashSet<NodeDijkstra>(); // Nodurile verificate în timpul cautarii
    public PriorityQueue<NodeDijkstra> WaitingNodes = new PriorityQueue<NodeDijkstra>(); // Coada de prioritati pentru gestionarea nodurilor
    public int maxIterations = 8000; // Numarul maxim de iteratii permise pentru cautare
    public GameObject Target; // Obiectul tinta
    public LayerMask SolidLayer; // Stratul de obstacole
    private float maxGizmoDistance = 3000f; // Distanta maxima pentru afișarea gizmo-urilor
    public List<NodeDijkstra> FreeNodes = new List<NodeDijkstra>();
    private bool pathFound = false;

    private static readonly List<NodeDijkstra> Neighbours = new List<NodeDijkstra> // Lista de vecini predefiniti
    {
         // Vecini: stanga, dreapta, jos, sus, stanga-jos, stanga-sus, dreapta-jos, dreapta-sus
        new NodeDijkstra(1, new Vector2(-1, 0), Vector2.zero, null), 
        new NodeDijkstra(1, new Vector2(1, 0), Vector2.zero, null),  
        new NodeDijkstra(1, new Vector2(0, -1), Vector2.zero, null), 
        new NodeDijkstra(1, new Vector2(0, 1), Vector2.zero, null),  
        new NodeDijkstra(1, new Vector2(-1, -1), Vector2.zero, null), 
        new NodeDijkstra(1, new Vector2(-1, 1), Vector2.zero, null),  
        new NodeDijkstra(1, new Vector2(1, -1), Vector2.zero, null),  
        new NodeDijkstra(1, new Vector2(1, 1), Vector2.zero, null)   
    };

    private int maxNodesEncountered = 0;
    // Momentul de începere a calculului drumului
    void Update()
    {
        float startTime = Time.realtimeSinceStartup;

        // Obține calea către țintă și calculeaza timpul și numărul de noduri
        //PathToTarget = GetPath(Target.transform.position);

        float endTime = Time.realtimeSinceStartup; // Momentul de final al calculului drumului
        float elapsedTime = endTime - startTime; // Timpul total pentru a calcula drumul
        int numberOfNodes = CheckedNodes.Count; // Numarul total de noduri verificate

        // Actualizeaza numarul maxim de noduri intalnite
        maxNodesEncountered = Mathf.Max(maxNodesEncountered, numberOfNodes);

        // Afiseaza numarul de noduri in cale si timpul in consola
        Debug.Log($"Number of Nodes in Path: {PathToTarget.Count}, Time: {elapsedTime} seconds");
        Debug.Log($"Number of Nodes: {numberOfNodes}, Time: {elapsedTime} seconds");
    }

    // Afiseaza numarul maxim de noduri intalnite cand script-ul este distrus
    void OnDestroy()
    {
        Debug.Log($"Maximum Number of Nodes Encountered: {maxNodesEncountered}");
    }
    // Metoda pentru obtinerea caii catre tinta
    public List<Vector2> GetPath(Vector2 target)
    {
        CheckedNodes.Clear(); // Curata lista de noduri verificate
        WaitingNodes.Clear(); // Curata coada de noduri în asteptare
        PathToTarget = new List<Vector2>();

        // Pozitia initiala rotunjita la valori întregi
        Vector2 startPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        // Pozitia tinta rotunjita la valori întregi
        Vector2 targetPosition = new Vector2(Mathf.Round(target.x), Mathf.Round(target.y));

        // Daca pozitia initiala coincide cu cea a tinte, returneaza calea goala
        if (startPosition == targetPosition)
        {
            pathFound = true;
            return PathToTarget;
        }

        // Creaza nodul de start
        NodeDijkstra startNode = new NodeDijkstra(0, startPosition, targetPosition, null);
        List<NodeDijkstra> neighbours = GetNeighbourNodes(startNode); // Obtine vecinii nodului de start
        CheckedNodes.Add(startNode);
        WaitingNodes.Enqueue(startNode, startNode.GDijkstra);
        foreach (var neighbour in neighbours)
        {
            WaitingNodes.Enqueue(neighbour, neighbour.GDijkstra); // Adauga vecinii in coada
        }
        int iterations = 0;
        while (WaitingNodes.Count > 0 && iterations < maxIterations)
        {
            NodeDijkstra nodeToCheck = WaitingNodes.Dequeue();

            // Verifica daca nodul curent coincide cu pozitia tinte
            if (nodeToCheck.PositionDijkstra == targetPosition)
            {
                pathFound = true;
                return CalculatePathFromNode(nodeToCheck);
            }

            // Verifica daca nodul este walkable
            var walkable = !Physics2D.OverlapCircle(nodeToCheck.PositionDijkstra, 0.8f, SolidLayer);

            // Trateaza cazurile de noduri traversabile si noduri solide
            if (!walkable)
            {
                CheckedNodes.Add(nodeToCheck); // Adauga nodul la nodurile verificate
            }
            else if (walkable)
            {
                WaitingNodes.Remove(nodeToCheck); // Elimina nodul din coada
                // Verifica daca nodul nu a fost verificat anterior
                if (!CheckedNodes.Where(x => x.PositionDijkstra == nodeToCheck.PositionDijkstra).Any())
                {
                    CheckedNodes.Add(nodeToCheck); // Adauga nodul la nodurile verificate
                    neighbours = GetNeighbourNodes(nodeToCheck); // Obtine vecinii nodului
                    foreach (var neighbour in neighbours)
                    {
                        WaitingNodes.Enqueue(neighbour, neighbour.GDijkstra); // Adauga vecinii in coada
                    }
                }
                iterations++; // Incrementare numar de iteratii
            }
        }
        // Actualizeaza lista de noduri libere si returneaza calea gasita sau goala
        FreeNodes = new List<NodeDijkstra>(CheckedNodes);
        return PathToTarget;
    }

    // Calculeaza calea de la un nod dat catre nodul de start
    public List<Vector2> CalculatePathFromNode(NodeDijkstra node)
    {
        var path = new List<Vector2>();
        NodeDijkstra currentNode = node;

        // Construieste calea prin parcurgerea nodurilor anterioare
        while (currentNode.PreviousNodeDijkstra != null)
        {
            path.Add(currentNode.PositionDijkstra);
            currentNode = currentNode.PreviousNodeDijkstra;
        }

        return path;
    }

    // Obtine vecinii unui nod dat
    List<NodeDijkstra> GetNeighbourNodes(NodeDijkstra node)
    {
        var neighbours = new List<NodeDijkstra>();

        // Itereaza prin vecinii predefiniti si creeaza noduri cu pozitii actualizate
        foreach (var neighbour in Neighbours)
        {
            NodeDijkstra newNeighbour = new NodeDijkstra(neighbour.GDijkstra + node.GDijkstra,
                                         new Vector2(Mathf.Round(node.PositionDijkstra.x + neighbour.PositionDijkstra.x),
                                                     Mathf.Round(node.PositionDijkstra.y + neighbour.PositionDijkstra.y)),
                                         node.TargetPositionDijkstra, node);

            neighbours.Add(newNeighbour);
        }

        return neighbours;
    }

    // Afiseaza gizmo-urile pentru nodurile verificate si calea gasita
    void OnDrawGizmos()
    {
        foreach (var item in CheckedNodes)
        {
            if (Vector2.Distance(item.PositionDijkstra, transform.position) < maxGizmoDistance)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(new Vector3(item.PositionDijkstra.x, item.PositionDijkstra.y, 0), 0.1f);
            }
        }

        foreach (var item in PathToTarget)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(new Vector3(item.x, item.y, 0), 0.2f);
        }
    }
}
