using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Level Settings")]
    [SerializeField] private int lives = 10;

    [SerializeField] private int totalWaves = 10; 

    public int TotalLives { get; private set; }
    public int CurrentWave { get; set; }
    public int TotalWaves => totalWaves;

    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        TotalLives = lives;
        CurrentWave = 1;
    }

    private void ReduceLives(Enemy enemy)
    {
        TotalLives--;
        if (TotalLives <= 0)
        {
            TotalLives = 0;
            GameOver();
        }
    }

    private void GameOver()
    {
        UIManager.Instance.ShowGameOverPanel();
    }

    private void WaveCompleted()
    {
        CurrentWave++;

        AchievementManager.Instance.AddProgress("Waves10", 1);
        AchievementManager.Instance.AddProgress("Waves20", 1);
        AchievementManager.Instance.AddProgress("Waves50", 1);
        AchievementManager.Instance.AddProgress("Waves100", 1);

       
        if (CurrentWave > totalWaves)
        {
            Debug.Log("Todas las oleadas terminadas — cambiando al siguiente nivel...");
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadSceneWithDelay(nextSceneIndex));
        }
        else
        {
            Debug.Log("🎉 Se completaron todos los niveles");
            SceneManager.LoadScene("MainMenu");
        }
    }

    private IEnumerator LoadSceneWithDelay(int sceneIndex)
    {
        yield return new WaitForSeconds(1.5f); 
        SceneManager.LoadScene(sceneIndex);
    }

    private void OnEnable()
    {
        Enemy.OnEndReached += ReduceLives;
        Spawner.OnWaveCompleted += WaveCompleted;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= ReduceLives;
        Spawner.OnWaveCompleted -= WaveCompleted;
    }
}


