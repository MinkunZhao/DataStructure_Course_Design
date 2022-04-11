using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecosystem.datastructure
{
    //define the struct edge used for classifying the entities into groups
    //The struct contains its two vertexs(Location) and the distance between them. 
    public struct Edge
    {
        public int from;
        public int to;
        public double distance;
    }

    //This class implements the ICpmparer interface and is used for the sorting of edges.
    public class EdgeComparer : IComparer<Edge>
    {
        public int Compare(Edge x, Edge y)
        {
            if (x.distance <= y.distance) { return 1; }
            else { return -1; }
        }
    }

    //priorityqueue data structure(min heap)
    public class PriorityQueue<T>
    {
        private IComparer<T> comparer;
        private T[] heap;

        public int Count { get; private set; }
        public PriorityQueue(int capacity, IComparer<T> comparer)
        {
            this.comparer = comparer;
            this.heap = new T[capacity];
        }

        public void Push(T v)
        {
            heap[Count] = v;
            SiftUp(Count++);
        }

        public bool Empty()
        {
            return Count == 0;
        }

        public T Pop()
        {
            var v = Top();
            heap[0] = heap[--Count];
            if (Count > 0) SiftDown(0);
            return v;
        }

        public T Top()
        {
            if (Count > 0) return heap[0];
            throw new InvalidOperationException("The queue is empty!");
        }

        void SiftUp(int n)
        {
            var v = heap[n];
            for (var i = n / 2; n > 0 && comparer.Compare(v, heap[i]) > 0; n = i, i /= 2) heap[n] = heap[i];
            heap[n] = v;
        }

        void SiftDown(int n)
        {
            var v = heap[n];
            for (var i = n * 2; i < Count; n = i, i *= 2)
            {
                if (i + 1 < Count && comparer.Compare(heap[i + 1], heap[i]) > 0) i++;
                if (comparer.Compare(v, heap[i]) >= 0) break;
                heap[n] = heap[i];
            }
            heap[n] = v;
        }
    }

    //This data structure is like Disjoint Set Union
    public class GenTree
    {
        private int[] array;
        private int Count;

        public GenTree(int size)
        {
            Count = size;
            array = new int[Count];
            for (int i = 0; i < size; i++)
            {
                array[i] = i;
            }
        }

        public int Find(int curr)
        {
            if (array[curr] == curr) return curr;
            return array[curr] = Find(array[curr]);
        }

        public bool Differ(int a, int b)
        {
            return Find(a) != Find(b);
        }

        public void Union(int a, int b)
        {
            int root1 = Find(a);
            int root2 = Find(b);
            if (root1 != root2) array[root2] = root1;
        }
    }

}
