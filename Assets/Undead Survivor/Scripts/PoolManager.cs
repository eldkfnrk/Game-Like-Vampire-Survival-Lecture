using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    List<GameObject>[] enemyPools;

    private void Awake()
    {
        enemyPools = new List<GameObject>[enemyPrefabs.Length];

        for(int index = 0; index < enemyPools.Length; index++)
        {
            enemyPools[index] = new List<GameObject>();
        }

        string log = string.Format("{0}, {1}", enemyPrefabs.Length, enemyPools.Length);
        Debug.Log(log);
    }
}
