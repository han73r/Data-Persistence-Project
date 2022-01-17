using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Used to work with menu scene buttons and save/load score system
/// </summary>


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string playerName;
    public string currentPlayerName;
    public int highScore;
    public int curScore;
    [SerializeField] TextMeshProUGUI bestPlayerNameText;
    [SerializeField] TextMeshProUGUI currentPlayerNameText;
    [SerializeField] TextMeshProUGUI bestScoreText;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGame();
    }

    public void StartGame()
    {
        playerName = bestPlayerNameText.text;
        currentPlayerName = currentPlayerNameText.text;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        SaveGame();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public int highScore;
    }

    // save high score and player name
    public void SaveGame()
    {
        PlayerData data = new PlayerData();
        // add if highScore more then current highScore
        data.playerName = playerName;
        data.highScore = highScore;

        
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    // load high score and last player name
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            playerName = data.playerName;
            highScore = data.highScore;

            bestScoreText.text = "Best Score: " + playerName + ": " + highScore;
            bestPlayerNameText.text = playerName; 
        }
    }

}
