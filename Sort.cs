namespace DVD_System
{
    class Sort
    {
        public static void HeapSort(Movie[] movies, string type)
        {
            int len = movies.Length;

            if (len <= 1)
            {
                return;
            }

            int left = 0;
            int right = len - 1;
            int middle = (len - 1) / 2;
            
            if (type == "viewcount")
            {
                for (int i = middle; i >= left; i--)
                {
                    IntHeapify(movies, len, i);
                }

                for (int i = right; i >= left; i--)
                {
                    Swap(movies, left, i);
                    IntHeapify(movies, i, left);
                }
            }
            else // type "title"
            {
                for (int i = middle; i >= left; i--)
                {
                    StringHeapify(movies, len, i);
                }

                for (int i = right; i >= left; i--)
                {
                    Swap(movies, left, i);
                    StringHeapify(movies, i, left);
                }
            }
            
        }

        private static void IntHeapify(Movie[] movies, int size, int root)
        {
            int largestNodeIndex = root;
            int leftNodeIndex = 2 * root + 1;
            int rightNodeIndex = 2 * root + 2;

            int largestNodeView = movies[root].GetViewCount();

            if (leftNodeIndex < size)
            {
                int leftNodeView = movies[leftNodeIndex].GetViewCount();

                if (leftNodeView > largestNodeView)
                {
                    largestNodeIndex = leftNodeIndex;
                    largestNodeView = movies[largestNodeIndex].GetViewCount();
                }
            }

            if (rightNodeIndex < size)
            {
                int rightNodeView = movies[rightNodeIndex].GetViewCount();

                if (rightNodeView > largestNodeView)
                {
                    largestNodeIndex = rightNodeIndex;
                }
            }

            if (largestNodeIndex != root)
            {
                Swap(movies, root, largestNodeIndex);
                IntHeapify(movies, size, largestNodeIndex);
            }
        }

        private static void StringHeapify(Movie[] movies, int size, int root)
        {
            int largestNodeIndex = root;
            int leftNodeIndex = 2 * root + 1;
            int rightNodeIndex = 2 * root + 2;

            string rootNodeTitle = movies[largestNodeIndex].GetTitle();

            if (leftNodeIndex < size)
            {
                string leftNodeTitle = movies[leftNodeIndex].GetTitle();
                if (string.Compare(leftNodeTitle, rootNodeTitle) > 0)
                {
                    largestNodeIndex = leftNodeIndex;
                    rootNodeTitle = movies[largestNodeIndex].GetTitle();
                }
            }

            if (rightNodeIndex < size)
            {
                string rightNodeTitle = movies[rightNodeIndex].GetTitle();
                if (string.Compare(rightNodeTitle, rootNodeTitle) > 0)
                {
                    largestNodeIndex = rightNodeIndex;
                }
                    
            }

            if (largestNodeIndex != root)
            {
                Swap(movies, root, largestNodeIndex);
                StringHeapify(movies, size, largestNodeIndex);
            }
        }

        private static void Swap(Movie[] movies, int i, int j)
        {
            (movies[j], movies[i]) = (movies[i], movies[j]);
        }
    }
}
