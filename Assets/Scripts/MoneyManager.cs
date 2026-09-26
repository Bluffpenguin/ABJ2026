using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

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

    [SerializeField] int currentMoney = 1000;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubtractMoney(int amount)
    {
        currentMoney -= amount;
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
    }
}
