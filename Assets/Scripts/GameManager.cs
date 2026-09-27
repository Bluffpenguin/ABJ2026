using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    List<GameObject> coins = new List<GameObject>();
    List<GameObject> floors = new List<GameObject>();

    [SerializeField] Transform startingPosition;
    [SerializeField] Image deathScreenObj;
    [SerializeField] GameObject deathScreenVid;
    [SerializeField] GameObject deathScreenButton;

    [SerializeField] GameObject player;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject creditsMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject HUD;

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
        HUD.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLose(Sprite deathScreen)
    {
        HUD.SetActive(false);
        /*
        if (deathScreenObj != null)
        {
			deathScreenObj.sprite = deathScreen;
			deathScreenObj.gameObject.SetActive(true);
		}
        else
            deathScreenVid.SetActive(true);
        */
        deathScreenVid.SetActive(true);

        deathScreenButton.SetActive(true);

	}

    public void ResetWorld()
    {
        HUD.SetActive(true );
		deathScreenObj.gameObject.SetActive(false);
        deathScreenVid.gameObject.SetActive(false);
        deathScreenButton.SetActive(false);
		MoneyManager.Instance.ResetMoney();
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
        HUD.SetActive(true);
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
}
