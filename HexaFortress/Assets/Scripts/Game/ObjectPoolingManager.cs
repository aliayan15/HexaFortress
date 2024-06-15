using System.Collections.Generic;
using System.Linq;
using MyUtilities;
using UnityEngine;

namespace HexaFortress.Game
{
    public class ObjectPoolingManager : SingletonMono<ObjectPoolingManager>
    {
        private readonly List<PooledObjectInfo> _pools = new();

        public GameObject SpawnObject(string id, GameObject objToSpawn, Vector3 position, Quaternion rotation)
        {
            var pool = _pools.Find(p => p.ID == id);
            if (pool == null)
            {
                pool = new PooledObjectInfo() { ID = id };
                _pools.Add(pool);
            }

            GameObject pooledObj = pool.InactiveObjects.FirstOrDefault();
            if (pooledObj == null)
            {
                pooledObj = Instantiate(objToSpawn, position, rotation);
            }
            else
            {
                pool.InactiveObjects.Remove(pooledObj);
                pooledObj.transform.SetPositionAndRotation(position, rotation);
                pooledObj.SetActive(true);
            }

            return pooledObj;
        }

        public void ReturnObject(string id, GameObject obj)
        {
            var pool = _pools.Find(p => p.ID == id);
            if (pool == null)
            {
                Debug.LogError("Returning object to null pool!!", this);
                return;
            }

            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }

    public class PooledObjectInfo
    {
        public readonly HashSet<GameObject> InactiveObjects = new();
        public string ID = "";
    }
}