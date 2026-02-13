using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTime;
    public float maxGameTime;

    public PoolManager poolManager;
    public Player player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        maxGameTime = 5 * 10f;
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
}
