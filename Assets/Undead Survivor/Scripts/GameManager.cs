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
    public int playerId;  // 플레이어 id(선택 창에서 선택한 플레이어 캐릭터의 정보를 적용하기 위한 캐릭터 구분)
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

    // 플레이어의 캐릭터 id를 받아서 플레이어 id를 저장
    public void GameStart(int id)
    {
        playerId = id;
        HP = maxHP;

        // 플레이어는 이제 캐릭터를 선택하기 전에는 보이지 않다가 선택하고 나서는 보이도록 수정
        player.gameObject.SetActive(true);
        levelUpUI.Select(playerId % 2);  // 플레이어 캐릭터에 알맞는 무기를 갖고 있도록 설정(혹시라도 플레이어 캐릭터 수보다 무기가 적어서 생기는 문제가 발생하는 것을 막기 위해 나머지 값으로 값을 제한)
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
