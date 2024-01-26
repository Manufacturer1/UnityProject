using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Acest script definește clasa Node, care reprezintă un nod în cadrul unui algoritm de căutare a căii în jocuri. 
 * Algoritmul A* este folosit pentru a găsi cea mai scurtă cale între două puncte într-un spațiu bidimensional.
 */

public class Node
{
    // Poziția nodului în spațiu
    public Vector2 Position;

    // Poziția țintei către care se face căutarea
    public Vector2 TargetPosition;

    // Nodul anterior, adică nodul de la care s-a ajuns la nodul curent
    public Node PreviousNode;

    // Costul total estimat al călătoriei de la nodul de start la nodul țintă
    // F = G + H
    public int F;

    // Costul efectiv al călătoriei de la nodul de start la nodul curent
    public int G;

    // Euristicul, adică estimarea costului de a ajunge de la nodul curent la nodul țintă
    public int H;

    // Constructorul clasei Node
    public Node(int g, Vector2 nodePosition, Vector2 targetPosition, Node previousNode)
    {
        // Inițializare poziția nodului curent
        Position = nodePosition;

        // Inițializare poziția țintei
        TargetPosition = targetPosition;

        // Inițializare nod anterior
        PreviousNode = previousNode;

        // Inițializare cost real al călătoriei de la nodul de start la nodul curent
        G = g;

        // Calculare și inițializare a distanței euristice folosind distanța Manhattan
        H = CalculateManhattanDistance(targetPosition, Position);

        // Calculare și inițializare a costului total estimat al călătoriei
        F = G + H;
    }

    // Metoda pentru calculul distanței Manhattan între două puncte
    /*
     * Exemplu simplu: A(0, 0) și B(3, 4)

        Manhattan Distance
        Manhattan Distance=∣3−0∣+∣4−0∣=3+4=7
     */
    private int CalculateManhattanDistance(Vector2 a, Vector2 b)
    {
        return (int)(Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y));
    }
}
