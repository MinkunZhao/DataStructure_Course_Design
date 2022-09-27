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

        /**
         * Function: Push the new element to the queue.
         * Input: The element of specified type.
         * Output: Empty.
         */
        public void Push(T v)
        {
            heap[Count] = v;
            SiftUp(Count++);
        }

        /**
         * Function: Check whether the queue is empty.
         * Input: Empty.
         * Output: bool.
         */
        public bool Empty()
        {
            return Count == 0;
        }

        /**
         * Function: Get the first element in the queue and delete it.
         * Input: Empty.
         * Output: The first element in the queue of specified type.
         */
        public T Pop()
        {
            var v = Top();
            heap[0] = heap[--Count];
            if (Count > 0) SiftDown(0);
            return v;
        }

        /**
         * Function: Get the first element in the queue.
         * Input: Empty.
         * Output: The first element in the queue of specified type.
         */
        public T Top()
        {
            if (Count > 0) return heap[0];
            throw new InvalidOperationException("The queue is empty!");
        }

        /**
         * Function: Construct the min heap so that the elements are arranged in order from bottom to top.
         * Input: The number of elements.
         * Output: Empty.
         */
        void SiftUp(int n)
        {
            var v = heap[n];
            for (var i = n / 2; n > 0 && comparer.Compare(v, heap[i]) > 0; n = i, i /= 2) heap[n] = heap[i];
            heap[n] = v;
        }

        /**
         * Function: Construct the min heap so that the elements are arranged in order from top to bottom.
         * Input: The number of elements.
         * Output: Empty.
         */
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

        /**
         * Function: Find the root of the tree to which the element belongs to.
         * Input: The index of the element.
         * Output: The root.
         */
        public int Find(int curr)
        {
            if (array[curr] == curr) return curr;
            return array[curr] = Find(array[curr]);
        }

        /**
         * Function: Determines whether two elements belong to the same tree.
         * Input: The indexs of the two elements.
         * Output: bool
         */
        public bool Differ(int a, int b)
        {
            return Find(a) != Find(b);
        }

        /**
         * Function: Add two elements to the same tree. 
         * Input: The indexs of the two elements.
         * Output: bool
         */
        public void Union(int a, int b)
        {
            int root1 = Find(a);
            int root2 = Find(b);
            if (root1 != root2) array[root2] = root1;
        }
    }

}
