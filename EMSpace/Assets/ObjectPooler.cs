using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//objectpooler is a dict where the elements point to a pool of a specific gameobject type
public class ObjectPooler : MonoBehaviour
{
    [System.Serializable] //making sure this will show up as a list in unity
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            //instantiate all ibjects we want and put them in the queue
            for (int i =0; i< pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); //they will be greyed out
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
