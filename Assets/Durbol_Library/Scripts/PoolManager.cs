/*
 * Developer Name: Md. Imran Hossain
 * E-mail: sandsoftimer@gmail.com
 * FB: https://www.facebook.com/md.imran.hossain.902
 * in: https://www.linkedin.com/in/md-imran-hossain-69768826/
 * 
 * Features:
 * Object Pooling
 * Object Pushing
 * Resetting Manager
 * Pre Instantiate pooling objects 
 */

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Com.Durbol.Utility
{
    public class PoolManager : MonoBehaviour
    {
        Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

        public GameObject Instantiate(GameObject prefabObj, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            // Finally this object will be return
            GameObject obj;

            // Make sure type is not null
            CheckTypeExist(prefabObj.tag);

            // If don't have any item yet then create one & return
            if (poolDictionary[prefabObj.tag].Count == 0)
            {
                obj = GameObject.Instantiate(prefabObj, position, rotation, parent);
                return obj;
            }
            // Finally pool an object & return;
            obj = poolDictionary[prefabObj.tag].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.parent = parent;
            obj.SetActive(true);
            return obj;
        }

        public void Destroy(GameObject obj, float killTime = 0)
        {
            if (killTime == 0) Hide_N_Store(obj);
            else StartCoroutine(DelayDestroy(obj, killTime));
        }

        IEnumerator DelayDestroy(GameObject obj, float killTime)
        {
            yield return new WaitForSeconds(killTime);

            Hide_N_Store(obj);
        }

        private void Hide_N_Store(GameObject obj)
        {
            if (obj == null)
            {
                Debug.Log("A null object can't be stored inside PoolManager.");
                return;
            }

            // Make sure type is not null
            CheckTypeExist(obj.tag);

            if (obj == null)
                return;

            obj.SetActive(false);
            obj.transform.parent = transform;
            poolDictionary[obj.tag].Enqueue(obj);
        }

        void CheckTypeExist(string tagName)
        {
            // If this prefab type is not in dictionary,
            // then make a type by it's tag name.
            if (!poolDictionary.ContainsKey(tagName))
                poolDictionary[tagName] = new Queue<GameObject>();
        }

        public void ResetPoolManager()
        {
            //Transform[] children = gameObject.GetComponentsInChildren(typeof(Transform), true) as Transform[];

            for (int i = transform.childCount - 1; i > -1; i--)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }

            //foreach (Transform item in transform)
            //{
            //    Destroy(item.gameObject);
            //}

            poolDictionary = new Dictionary<string, Queue<GameObject>>();
        }

        public void PrePopulateItem(GameObject obj, int howMany)
        {
            // Make sure type is not null
            CheckTypeExist(obj.tag);

            for (int i = 0; i < howMany; i++)
            {
                Hide_N_Store(GameObject.Instantiate(obj));
            }
        }

    }
}