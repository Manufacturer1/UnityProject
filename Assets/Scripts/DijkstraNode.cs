using UnityEngine;

public class NodeDijkstra
{
    // Poziția nodului în spațiu
    public Vector2 PositionDijkstra;

    // Poziția țintei către care se face căutarea
    public Vector2 TargetPositionDijkstra;

    // Nodul anterior
    public NodeDijkstra PreviousNodeDijkstra;

    // Costul efectiv al călătoriei de la nodul de start la nodul curent
    public int GDijkstra;

    // Constructorul clasei NodeDijkstra
    public NodeDijkstra(int g, Vector2 nodePositionDijkstra, Vector2 targetPositionDijkstra, NodeDijkstra previousNodeDijkstra)
    {
        PositionDijkstra = nodePositionDijkstra;
        TargetPositionDijkstra = targetPositionDijkstra;
        PreviousNodeDijkstra = previousNodeDijkstra;
        GDijkstra = g;
    }

}
