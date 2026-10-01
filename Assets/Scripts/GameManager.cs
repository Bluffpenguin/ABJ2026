using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    List<GameObject> coins = new List<GameObject>();
    List<GameObject> floors = new List<GameObject>();

    [SerializeField] Transform startingPosition;
    
    [SerializeField] GameObject player;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject creditsMenu;
    [SerializeField] GameObject optionsMenu;

    [SerializeField] List<GameObject> levelPrefabs;
    Queue<GameObject> levelQueue = new();

    [Header("End Screen")]
    [SerializeField] VideoPlayer videoPlayer;
	[SerializeField] Image deathScreenObj;
	[SerializeField] GameObject deathScreenVid;
	[SerializeField] GameObject deathScreenButton;
    [SerializeField] TextMeshProUGUI totalMoneyCollectText;


	enum GameState
    {
        Menu,
        Playing
    }
    GameState currentGameState = GameState.Menu;

	private void Awake()
	{
        if (Instance != null) Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.music_mainTheme);
        player.SetActive(false);

		// Ensure Source is set to URL programmatically
		videoPlayer.source = VideoSource.Url;

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
        
		if (currentGameState == GameState.Playing)
        {
            if (player.transform.position.y > 15)
            {
                OnLose("Sky Death.mp4");
            }
        }
	}

	public void OnLose(string loseScreen = null)
    {
        /*
        if (deathScreenObj != null)
        {
			deathScreenObj.sprite = deathScreen;
			deathScreenObj.gameObject.SetActive(true);
		}
        else
            deathScreenVid.SetActive(true);
        */
        currentGameState = GameState.Menu;
        totalMoneyCollectText.text = "$" + MoneyManager.Instance.GetTotalMoney();
        player.SetActive(false);
        deathScreenVid.SetActive(true);

        deathScreenButton.SetActive(true);
        MoneyManager.Instance.DisableUI();
        if (loseScreen != null) { PlayLoseVideo(loseScreen); }
        else PlayLoseVideo("OilDeath.mp4");
	}

    void PlayLoseVideo(string loseScreen)
    {
		// Safely combine the streaming assets root path with the file name
		string fullPath = Path.Combine(Application.streamingAssetsPath, loseScreen);

		videoPlayer.url = fullPath;
		videoPlayer.Play();
	}

    public void ResetWorld()
    {
        //HUD.SetActive(true );
		deathScreenObj.gameObject.SetActive(false);
        deathScreenVid.gameObject.SetActive(false);
        deathScreenButton.SetActive(false);
		MoneyManager.Instance.ResetMoney();
        MoneyManager.Instance.EnableUI();
        player.SetActive(true);
        PlayerController.Instance.ResetPlayer(startingPosition.position);

        foreach (GameObject coin in coins)
        {
            coin.SetActive(true);
        }

        foreach (GameObject floor in floors)
        {
            if (floor != null)
            {
                Destroy(floor);
            }
        }

        floors.Clear();

        // Reset generator for next run.
        foreach (FloorGenerator generator in FindObjectsByType<FloorGenerator>(FindObjectsSortMode.None))
        {
            generator.ResetGenerator();
        }

        currentGameState = GameState.Playing;
        
    }

    public void AddCoinToList(GameObject coin)
    {
        coins.Add(coin);
    }

    public void RemoveCoinFromList(GameObject coin)
    {
        coins.Remove(coin);
    }

    public void AddFloorToList(GameObject coin)
    {
        floors.Add(coin);
    }

	#region Menu Buttons
    public void StartGame()
    {
        mainMenu.SetActive(false);
        player.SetActive(true);
        currentGameState = GameState.Playing;
        MoneyManager.Instance.EnableUI();
    }

    public void OptionsButton()
    {

    }

    public void CreditsButton()
    {
        creditsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseCredits()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
	#endregion

    public GameObject GetLevel()
    {
        if (levelQueue.Count == 0)
        {
            levelPrefabs.Shuffle();
            foreach (GameObject level in levelPrefabs)
            {
                levelQueue.Enqueue(level);
            }
        }

        return levelQueue.Dequeue();
    }

	
}

public static class ListExtensions
{
	public static void Shuffle<T>(this List<T> list)
	{
		// Loop backwards from the last element down to the second element
		for (int i = list.Count - 1; i > 0; i--)
		{
			// Pick a random index from 0 to i (inclusive)
			int randomIndex = Random.Range(0, i + 1);

			// Swap the element at i with the element at randomIndex
			T temp = list[i];
			list[i] = list[randomIndex];
			list[randomIndex] = temp;
		}
	}
}
