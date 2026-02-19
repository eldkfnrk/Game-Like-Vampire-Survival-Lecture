using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Control")]
    public float gameTime;
    public float maxGameTime;

    [Header("Player Info")]
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 17, 23, 36 };

    [Header("Public GameObject")]
    public PoolManager poolManager;
    public Player player;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        maxGameTime *= 60f;
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

        if(exp >= nextExp[level])
        {
            level++;
            exp = 0;
        }
    }
}
