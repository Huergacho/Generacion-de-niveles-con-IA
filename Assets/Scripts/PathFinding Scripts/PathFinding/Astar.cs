using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Astar<T>
{
    public List<T> GetPath(T startPoint, Func<T, bool> condition, Func<T, List<T>> getNeighbours, Func<T, T, float> getConectionCost,Func<T,float> heuristic ,int watchDog = 500)
    {
        PriorityQueue<T> pending = new PriorityQueue<T>();

        HashSet<T> visited = new HashSet<T>();

        Dictionary<T, T> parents = new Dictionary<T, T>();

        Dictionary<T, float> cost = new Dictionary<T, float>();

        pending.Enqueue(startPoint, 0);
        cost[startPoint] = 0;
        //cost.Add(startPoint, 0);
        while (!pending.IsEmpty || watchDog <= 0)
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
                foreach (var neight in neightbours)
                {
                    if (visited.Contains(neight))
                    {
                        continue;
                    }
                    var neightCost = cost[curr] + getConectionCost(curr, neight);
                    if (cost.ContainsKey(neight) && cost[neight] <= neightCost) { continue; }
                    cost[neight] = neightCost;

                    parents[neight] = curr;
                    pending.Enqueue(neight, neightCost + heuristic(neight));
                }
            }
        }
        return new List<T>();
    }
    List<T> GeneratePath(T endPoint, Dictionary<T, T> parents)
    {
        List<T> path = new List<T>();
        path.Add(endPoint);
        while (parents.ContainsKey(path[path.Count - 1]))
        {
            path.Add(parents[path[path.Count - 1]]);
        }
        path.Reverse();
        return path;
    }
}
