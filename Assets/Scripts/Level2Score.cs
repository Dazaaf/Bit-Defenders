using System;
using System.IO;
using TMPro;
using UnityEngine;

public class Level2Score : MonoBehaviour
{
    public static Level2Score Instance;

    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;

    
    
    
    public class Node
    {
        public string data;
        public Node next;

        public Node(string data)
        {
            this.data = data;
            this.next = null;
        }
    }

    private Node head = null;   
    private Node tail = null;   
    

    private string folderPath;
    private string filePath;

    private void Awake()
    {
        Instance = this;

        folderPath = @"E:\jogo\Xd";
        filePath = Path.Combine(folderPath, "score_level2.txt");

        try
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }
        catch (Exception e)
        {
            Debug.LogError(" Error al crear carpeta: " + e.Message);
        }

        score = 0;
        UpdateScoreText();

        
        File.WriteAllText(filePath, "Score Level 2 - Registro\n\n");
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();

        
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Score: {score}";

        
        AddToLinkedList(logEntry);

        
        SaveLinkedListToFile();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }


    private void AddToLinkedList(string data)
    {
        Node newNode = new Node(data);

        if (head == null)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            tail.next = newNode;
            tail = newNode;
        }
    }

    private void SaveLinkedListToFile()
    {
        try
        {
            Node current = head;

            
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine("Score Level 2 - Registro\n");

                while (current != null)
                {
                    writer.WriteLine(current.data);
                    current = current.next;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(" Error al guardar score en lista enlazada: " + e.Message);
        }
    }
    
}

