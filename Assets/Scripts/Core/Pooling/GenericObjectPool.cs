using System.Collections.Generic;
using UnityEngine;

namespace Jlabarca.OneBigMob.Core.Pooling
{
    public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component
    {
        private readonly Queue<T> _objects = new Queue<T>();
        private T _prefab;
        public void Prewarm(int count, T prefab)
        {
            _prefab = prefab;
            if (_objects.Count >= count)
            {
                Debug.LogWarning("Pool is already big enough!");
                return;
            }

            int numberToPrewarm = count - _objects.Count;
            AddObjects(numberToPrewarm, prefab);

        }

        public T Get(T prefab)
        {
            if (_objects.Count == 0)
                AddObjects(1, prefab);
            T objectFromPool = _objects.Dequeue();
            return objectFromPool;
        }

        public T Get()
        {
            if (_objects.Count == 0)
                AddObjects(1, _prefab);
            T objectFromPool = _objects.Dequeue();
            return objectFromPool;
        }

        private void AddObjects(int count, T prefab)
        {
            for (int i = 0; i < count; i++)
            {
                var newObject = Instantiate(prefab, transform, true);
                newObject.gameObject.SetActive(false);
                _objects.Enqueue(newObject);
            }
        }

        public virtual void ReturnToPool(T objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            _objects.Enqueue(objectToReturn);
        }
    }
}
