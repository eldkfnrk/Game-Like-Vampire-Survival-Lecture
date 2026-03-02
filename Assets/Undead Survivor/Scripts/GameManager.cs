using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Control")]
    public bool isGameStop;  // 시간 정지 여부를 저장하는 변수
    public float gameTime;
    public float maxGameTime;

    [Header("Player Info")]
    public float HP;
    public float maxHP;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 17, 23, 36 };

    [Header("Public GameObject")]
    public PoolManager poolManager;
    public Player player;
    public LevelUp levelUpUI;
    public Result resultUI;
    public GameObject enemyCleaner;  // 게임 시간이 지나 승리하게 되면 모든 적을 죽이도록 하는 오브젝트
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        isGameStop = true;
        maxGameTime *= 60f;
        maxHP = 100;
    }

    public void GameStart()
    {
        HP = maxHP;
        levelUpUI.Select(0);  // 임시 스크립트(게임 시작 시 플레이어가 근접 무기를 들고 있을 수 있도록 설정)
        GameResume();
    }

    private void Update()
    {
        if (isGameStop)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (isGameStop)
            return;

        exp++;

        // 무한 레벨 업을 위해서 인덱스 값을 레벨이 배열 크기보다 크면 배열의 마지막 값을 계속 호출할 수 있도록 하는 것이다.
        if(exp >= nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++;
            exp = 0;
            levelUpUI.Show();
        }
    }

    public void GameStop()
    {
        isGameStop = true;
        Time.timeScale = 0f;
    }

    public void GameResume()
    {
        isGameStop = false;
        Time.timeScale = 1f;
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);  // 씬을 다시 호출하여서 씬을 초기화하는 방식으로 재시작 구현
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isGameStop = true;

        yield return new WaitForSeconds(0.3f);
        resultUI.gameObject.SetActive(true);
        resultUI.GameLose();
        GameStop();
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isGameStop = true;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        resultUI.gameObject.SetActive(true);
        resultUI.GameWin();
        GameStop();
    }
}
