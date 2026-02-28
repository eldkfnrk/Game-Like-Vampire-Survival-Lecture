using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Control")]
    public bool isGameStop;  // 시간 정지 여부를 저장하는 변수
    public float gameTime;
    public float maxGameTime;

    [Header("Player Info")]
    public int HP;
    public int maxHP;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 17, 23, 36 };

    [Header("Public GameObject")]
    public PoolManager poolManager;
    public Player player;
    public LevelUp levelUpUI;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        maxGameTime *= 60f;
        maxHP = 100;
    }

    private void Start()
    {
        HP = maxHP;

        // 임시 스크립트(게임 시작 시 플레이어가 근접 무기를 들고 있을 수 있도록 설정)
        levelUpUI.Select(0);
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
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
}
