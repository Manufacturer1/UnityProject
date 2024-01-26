using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    public void Remove(T item)
    {
        elements.RemoveAll(tuple => EqualityComparer<T>.Default.Equals(tuple.Item1, item));
    }

    // Lista de tupluri care contine elementele si prioritatile acestora
    private List<Tuple<T, float>> elements = new List<Tuple<T, float>>();

    // Proprietate care returneaza numarul de elemente din coada
    public int Count => elements.Count;

    // Metoda pentru adaugarea unui element în coada cu o anumita prioritate
    public void Enqueue(T item, float priority)
    {
        // Se adauga un tuplu format din element si prioritate în lista de elemente
        elements.Add(Tuple.Create(item, priority));
    }

    // Metoda pentru extragerea elementului cu cea mai mare prioritate din coada
    public T Dequeue()
    {
        // Se initializeaza indexul celui mai bun element cu primul element din lista
        int bestIndex = 0;

        // Se parcurge lista pentru a gasi elementul cu cea mai mare prioritate
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                // Daca se gaseste un element cu prioritate mai mare(cu costul F mai mic), se actualizeaza indexul
                bestIndex = i;
            }
        }

        // Se extrage elementul cu cea mai mare prioritate
        T bestItem = elements[bestIndex].Item1;

        // Se elimina elementul extras din lista de elemente
        elements.RemoveAt(bestIndex);

        // Se returneaza elementul cu cea mai mare prioritate
        return bestItem;
    }

    internal void Clear()
    {
        elements.Clear();
    }
}
