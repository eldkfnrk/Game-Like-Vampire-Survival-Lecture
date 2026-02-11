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

        if (!selectObject)  //변수에 데이터가 없으면 false 있으면 true를 반환해준다.(객체 같은 null 값을 가지는 기본 자료형이 아닌 변수)
        {
            selectObject = Instantiate(enemyPrefabs[index], transform);
            enemyPools[index].Add(selectObject);
        }

        return selectObject;
    }
}
