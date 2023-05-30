using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PriorityQueue<T>
{
    private SortedDictionary<float, Queue<T>> dict = new SortedDictionary<float, Queue<T>>();

    public int Count()
    {
        return dict.Values.Sum(queue => queue.Count);

    }
    public void Enqueue(float priority, T item)
    {
        if (dict.TryGetValue(priority, out var queue))
        {
            queue.Enqueue(item);
        }
        else
        {
            var newQueue = new Queue<T>();
            newQueue.Enqueue(item);
            dict.Add(priority, newQueue);
        }
    }

    public T Dequeue()
    {
        if (dict.Count == 0)
            throw new InvalidOperationException("우선순위 큐가 비어 있습니다.");

        var pair = dict.First();
        var v = pair.Value.Dequeue();

        if (pair.Value.Count == 0)
            dict.Remove(pair.Key);

        return v;
    }

    public bool IsEmpty() => !dict.Any();

    public bool Contains(T item)
    {
        return dict.Values.Any(queue => queue.Contains(item));
    }
}
