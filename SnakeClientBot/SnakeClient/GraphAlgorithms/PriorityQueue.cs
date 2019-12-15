using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeClient.GraphAlgorithms
{
    public class PriorityQueue<T>
    {
        private List<(T, double)> priorityQueue;

        public PriorityQueue()
        {
            this.priorityQueue = new List<(T, double)>();
        }

        public void Enqueue(T item, double priority)
        {
            priorityQueue.Add((item, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < priorityQueue.Count; i++)
            {
                if (priorityQueue[i].Item2 < priorityQueue[bestIndex].Item2)
                    bestIndex = i;
            }

            T bestItem = priorityQueue[bestIndex].Item1;
            priorityQueue.RemoveAt(bestIndex);
            return bestItem;
        }

        public int Count => priorityQueue.Count;
    }
}
