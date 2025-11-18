using UnityEngine;
using UnityEngine.SceneManagement;

public enum Difficulty
{
    Easy,
    Hard
}

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }
    public Difficulty SelectedDifficulty { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SetDifficulty(int value)
    {
        SelectedDifficulty = (Difficulty)value;
        Debug.Log("Dificultad seleccionada: " + SelectedDifficulty);

        SceneManager.LoadScene("SampleScene");
    }
}
