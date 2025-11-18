using UnityEngine;

public class DifficultyMenuUI : MonoBehaviour
{
    public GameObject menuUI; 
    public GameObject gameSystems; 

    public void SelectDifficulty(int difficultyValue)
    {
        DifficultyManager.Instance.SetDifficulty(difficultyValue);
        menuUI.SetActive(false);
        gameSystems.SetActive(true); 
    }
}
