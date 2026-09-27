using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    List<GameObject> coins = new List<GameObject>();

    [SerializeField] Transform startingPosition;

    [SerializeField] FloorGenerator floor;

	private void Awake()
	{
        if (Instance != null) Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLose()
    {
        ResetWorld();
    }

    public void ResetWorld()
    {
        MoneyManager.Instance.ResetMoney();
        PlayerController.Instance.ResetPlayer(startingPosition.position);
        floor.DeleteFloors();

        foreach (GameObject coin in coins)
        {
            coin.SetActive(true);
        }
    }

    public void AddCoinToList(GameObject coin)
    {
        coins.Add(coin);
    }

    public void RemoveCoinFromList(GameObject coin)
    {
        coins.Remove(coin);
    }
}
