using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class NewProjectCode : MonoBehaviour
{
    public static class AddressablesLoader
    {
        public static async Task<T> Load<T>(AssetReference keyAsset)
        {
            var handle = keyAsset.LoadAssetAsync<T>();
            await handle.Task;
            if (handle.Status.Equals(AsyncOperationStatus.Succeeded))
            {
                return handle.Task.Result;
            }
            else
            {
                throw new NullReferenceException($"Addressables Load Error: {typeof(T)}/{keyAsset} is null");
            }
        }

        public static async Task<T> LoadInstantiate<T>(AssetReferenceGameObject keyAsset)
        {
            var handle = keyAsset.InstantiateAsync();
            await handle.Task;
            if (handle.Result.TryGetComponent(out T loadAsset) == false)
            {
                throw new NullReferenceException($"Addressables Load Instantiate Error: {typeof(T)}/{keyAsset} is null");
            }

            return loadAsset;
        }

        public static void UnloadGameobject(GameObject unloadGameObject)
        {
            if (unloadGameObject == null)
            {
                throw new NullReferenceException($"Addressables Unload Instance Error: {unloadGameObject.name} is null");
            }

            unloadGameObject.SetActive(false);
            Addressables.ReleaseInstance(unloadGameObject);
            unloadGameObject = null;
        }
    }

    public class Timer
    {
        private readonly float _duration;
        private float _timeEnd;

        public bool IsTimerEnd => Time.time >= _timeEnd;

        public Timer(float duration, bool startDelay = false)
        {
            _duration = duration;
            if (startDelay)
            {
                Start();
            }
            else
            {
                _timeEnd = Time.time;
            }

        }

        public void Start()
        {
            _timeEnd = Time.time + _duration;
        }
    }

    public class PoolObject<T> where T : MonoBehaviour
    {
        protected readonly List<T> _pool;
        protected readonly SpawnFactory<T> _factory;

        public PoolObject(SpawnFactory<T> factory, int pollRange = 1)
        {
            _factory = factory;
            _pool = new List<T>(pollRange);
            CreateStartPoll(pollRange);
        }

        public T[] GetAll()
        {
            return _pool.ToArray();
        }

        public T Get()
        {
            foreach (T poolObject in _pool)
            {
                if (poolObject.gameObject.activeInHierarchy == false)
                {
                    poolObject.gameObject.SetActive(true);
                    return poolObject;
                }
            }

            return Create();
        }

        public T Create(bool activaState = true)
        {
            T newObject = _factory.Create();
            newObject.gameObject.SetActive(activaState);
            _pool.Add(newObject);
            return newObject;
        }

        private void CreateStartPoll(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Create(false);
            }
        }

    }
}

public class SpawnFactory<T> where T : MonoBehaviour
{
    internal T Create()
    {
        throw new NotImplementedException();
    }
}