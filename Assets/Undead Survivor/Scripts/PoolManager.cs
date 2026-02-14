using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] enemyPools;

    private void Awake()
    {
        enemyPools = new List<GameObject>[prefabs.Length];

        for(int index = 0; index < enemyPools.Length; index++)
        {
            enemyPools[index] = new List<GameObject>();
        }
    }

    public GameObject GetObject(int index)
    {
        GameObject selectObject = null;

        foreach(GameObject gameObject in enemyPools[index])
        {
            if (!gameObject.activeSelf)
            {
                selectObject = gameObject;
                selectObject.SetActive(true);
                break;
            }
        }

        if (!selectObject)
        {
            selectObject = Instantiate(prefabs[index], transform);
            enemyPools[index].Add(selectObject);
        }

        return selectObject;
    }
}
