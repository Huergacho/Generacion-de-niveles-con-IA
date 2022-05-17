using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class BFS<T>
{

    public List<T> GetPath(T startPoint, Func<T, bool> condition, Func<T, List<T>> getNeighbours, int watchDog = 500)
    {
        Queue<T> pending = new Queue<T>();

        HashSet<T> visited = new HashSet<T>();

        Dictionary<T, T> parents = new Dictionary<T, T>();

        pending.Enqueue(startPoint);
        while (pending.Count != 0 || watchDog <= 0)
        {
            T curr = pending.Dequeue();
            watchDog--;
            if (condition(curr))
            {
                return GeneratePath(curr, parents);
            }
            else
            {
                visited.Add(curr);
                List<T> neightbours = getNeighbours(curr);
                foreach (var item in neightbours)
                {
                    if (visited.Contains(item))
                    {
                        continue;
                    }
                    if (pending.Contains(item))
                    {
                        continue;
                    }
                    parents[item] = curr;
                    pending.Enqueue(item);
                }
            }
        }
        return new List<T>();
    }
    List<T> GeneratePath(T endPoint, Dictionary<T,T> parents)
    {
        List<T> path = new List<T>();
        path.Add(endPoint);
        while(parents.ContainsKey(path[path.Count - 1]))
        {
            path.Add(parents[path[path.Count - 1]]);
        }
        path.Reverse();
        return path;
    }
}
