using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

	[SerializeField] int startingMoney = 1000;
	[SerializeField] int currentMoney = 1000;
    

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
        }
	}

    
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetMoney()
    {
        currentMoney = startingMoney;
    }

    public void SubtractMoney(int amount)
    {
        currentMoney -= amount;

        if (currentMoney <= 0) GameManager.Instance.OnLose();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
    }
}
