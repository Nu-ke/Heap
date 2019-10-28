     public class Heap<T>
    {
        private const int _max = 100_000;
        private readonly HeapItem<T>[] _items = new HeapItem<T>[_max];
        private readonly Dictionary<T, int> _indexes = new Dictionary<T, int>();
        private int _currentIndex;

        public bool IsEmpty => _currentIndex == 0;

        public void Add(T item, int priority)
        {
            var node = new HeapItem<T>(item, priority);
            _items[_currentIndex] = node;
            _indexes[_items[_currentIndex].Item] = _currentIndex;
            
            PercolateUp(_currentIndex);
            _currentIndex++;
        }

        public T Get()
        {
            var node = _items[0];
            _currentIndex--;
            _items[0] = _items[_currentIndex];
            PercolateDown(0);

            return node.Item;
        }

        public void Update(T item, int priority)
        {
            var currentIndex = _indexes[item];
            var heapItem = _items[currentIndex];
            var currentPriority = heapItem.Priority;
            
            if (priority == currentPriority)
                return;

            heapItem.Priority = priority;
            
            if (currentPriority > priority)
                PercolateDown(currentIndex);
            else
                PercolateUp(currentIndex);
        }

        private void PercolateUp(int currentIndex)
        {
            var parentIndex = (currentIndex - 1) / 2;
            var parent = _items[parentIndex];
            var son = _items[currentIndex];

            while (son.Priority > parent.Priority)
            {
                Swap(parentIndex, currentIndex);

                currentIndex = parentIndex;
                parentIndex = (currentIndex - 1) / 2;
                parent = _items[parentIndex];
                son = _items[currentIndex];
            }
        }

        private void PercolateDown(int currentIndex)
        {
            var lChildIndex = 2 * currentIndex + 1;
            var rChildIndex = 2 * currentIndex + 2;
            var lChild = lChildIndex < _currentIndex ? _items[lChildIndex] : null;
            var rChild = rChildIndex < _currentIndex ? _items[rChildIndex] : null;
            var currentNode = _items[currentIndex];
            var cont = true;

            while (cont)
            {
                cont = false;
                var max = currentNode.Priority;
                var index = (int?)null;
                if (rChild != null && rChild.Priority > max)
                {
                    cont = true;
                    max = rChild.Priority;
                    index = rChildIndex;
                }

                if (lChild != null && lChild.Priority > max)
                {
                    cont = true;
                    index = lChildIndex;
                }

                if (cont)
                {
                    Swap(index.Value, currentIndex);
                    currentIndex = index.Value;

                    lChildIndex = 2 * currentIndex + 1;
                    rChildIndex = 2 * currentIndex + 2;
                    lChild = lChildIndex < _currentIndex ? _items[lChildIndex] : null;
                    rChild = rChildIndex < _currentIndex ? _items[rChildIndex] : null;
                    currentNode = _items[currentIndex];
                }
            }
        }

        private void Swap(int i, int j)
        {
            var temp = _items[i];
            _items[i] = _items[j];
            _items[j] = temp;

            _indexes[_items[i].Item] = i;
            _indexes[_items[j].Item] = j;
        }
    }

    public class HeapItem<T>
    {
        public HeapItem(T item, int priority)
        {
            Item = item;
            Priority = priority;
        }

        public T Item { get; }
        public int Priority { get; set; }
    }
