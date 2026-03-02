using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] resultTitle;

    public void GameWin()
    {
        resultTitle[0].SetActive(true);
    }

    public void GameLose()
    {
        resultTitle[1].SetActive(true);
    }
}
