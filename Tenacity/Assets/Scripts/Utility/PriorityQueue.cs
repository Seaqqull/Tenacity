using System.Collections.Generic;
using System.Collections;
using System.Linq;


namespace Tenacity.Utility
{
    public class PriorityQueue<TObject>  : IEnumerable<(double Priority, TObject Object)>
    {
        private List<(double Priority, TObject Object)> _queue =
            new List<(double Priority, TObject Object)>();

        public int Count { get => _queue.Count; }
        public IEnumerable<TObject> RawObjects
        {
            get => _queue.Select(queueObject => queueObject.Object);
        }


        public void Clear()
        {
            _queue.Clear();
        }

        public TObject Peek()
        {
            var item = _queue[_queue.Count - 1];
            return item.Object;
        }
        
        public TObject Dequeue()
        {
            var item = _queue[_queue.Count - 1];
            _queue.RemoveAt(_queue.Count - 1);
            return item.Object;
        }

        public bool Exist(TObject item)
        {
            var itemIndex = _queue.FindIndex((queueItem) => { return Equals(queueItem.Object, item); });
            return itemIndex != -1;
        }

        public void Dequeue(TObject item)
        {
            var itemIndex = _queue.FindIndex(queueItem => Equals(item, queueItem.Object));
            _queue.RemoveAt(itemIndex);
        }
        
        public void Enqueue(double priority, TObject item)
        {
            var insertIndex = _queue.Count;
            for (int i = 0; i < _queue.Count; i++)
            {
                if(_queue[i].Priority > priority)
                {
                    insertIndex = i;
                    break;
                }
            }
            
            _queue.Insert(insertIndex, (priority, item));
        }

        
        public IEnumerator<(double Priority, TObject Object)> GetEnumerator()
        {
            foreach (var item in _queue)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}