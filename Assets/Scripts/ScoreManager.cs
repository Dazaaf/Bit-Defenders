using System;
using System.IO;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    private string folderPath;
    private string filePath;

    private void Awake()
    {
        
        Instance = this;

        folderPath = @"E:\jogo\Xd";

        try
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }
        catch (Exception e)
        {
            Debug.LogError(" Error al crear carpeta: " + e.Message);
        }

        filePath = Path.Combine(folderPath, "score.txt");
    }

    private void Start()
    {
        ResetScore();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        SaveScore();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void SaveScore()
    {
        try
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Score: {score}";
            File.AppendAllText(filePath, logEntry + Environment.NewLine);
            Debug.Log(" Score agregado al historial: " + logEntry);
        }
        catch (Exception e)
        {
            Debug.LogError(" Error al guardar el score: " + e.Message);
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
        Debug.Log(" Score reiniciado para nueva partida.");
    }
}
