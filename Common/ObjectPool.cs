using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : SingletonMonoBehaviour<ObjectPool>
{
    [SerializeField] private GameObject[] _gameObjects;
    public class Pool
    {
        public GameObject PoolObject;
        public bool InUse;
    }
    private List<Pool> pools = new List<Pool>();

    public T Get<T>(Transform parent)
    {
        GameObject poolObject = null;
        foreach (var obj in pools)
        {
            if (obj.InUse == false)
            {
                poolObject = obj.PoolObject;
                obj.PoolObject.SetActive(true);
                obj.InUse = true;
                return obj.PoolObject.GetComponent<T>();
            }
        }
        return Create<T>(parent);
    }

    private T Create<T>(Transform parent)
    {
        Pool obj = new Pool();
        obj.PoolObject = Instantiate(_gameObjects[Random.Range(0, _gameObjects.Length)], parent);
        obj.InUse = true;
        pools.Add(obj);
        return obj.PoolObject.GetComponent<T>();
    }

    public void Release(GameObject obj)
    {
        foreach (var poolObj in pools)
        {
            if (poolObj.PoolObject.Equals(obj))
            {
                poolObj.InUse = false;
                return;
            }
        }
    }
}


