using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

	[SerializeField] int startingMoney = 1000;
	[SerializeField] int currentMoney = 1000;
    int totalMoneyCollected = 0;

    public VisualTreeAsset template;
    [SerializeField] UIDocument uiMoneyPopups;
    VisualElement costFadeContainer;
    VisualElement moneyContainer;
    Label currentMoneyLabel;
    int containerSize = 0;
    int maxContainerSize = 10;

    Sprite lastDamageDeathSprite;

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
        costFadeContainer = uiMoneyPopups.rootVisualElement.Q("CostFade");
        moneyContainer = uiMoneyPopups.rootVisualElement.Q("Money");
        currentMoneyLabel = uiMoneyPopups.rootVisualElement.Q<Label>("MoneyTotal");
		currentMoneyLabel.text = "$" + currentMoney;

        moneyContainer.style.visibility = Visibility.Hidden;

        totalMoneyCollected = startingMoney;
	}

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            SpawnPopup(1);
        }

        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            SpawnPopup(10);
        }

		if (Keyboard.current.pKey.wasPressedThisFrame)
		{
			SpawnPopup(100);
		}
	}

    void SpawnPopup(int cost)
    {
		TemplateContainer popupContainer = template.Instantiate();
        Label bullet = popupContainer.Q<Label>("CostBullet");
        
        if(cost > 0)
        {
            bullet.text = ("+$" + cost);
            bullet.style.color = Color.green;
        }
        else
        {
			bullet.text = ("-$" + Mathf.Abs(cost));
			bullet.style.color = Color.red;
		}

        bullet.style.fontSize = new Length(26 + 2 * (Mathf.Abs(cost) / 10));
        //uiMoneyPopups.rootVisualElement.Q("CostFade").Add(bullet);
		costFadeContainer.Insert(0, bullet);
        containerSize++;

        if (containerSize > maxContainerSize)
        {
            costFadeContainer.RemoveAt(maxContainerSize - 1);
            containerSize--;
        }

	}

    public void ResetMoney()
    {
        currentMoney = startingMoney;
		currentMoneyLabel.text = "$" + currentMoney;
        costFadeContainer.Clear();
        containerSize = 0;
        totalMoneyCollected = startingMoney;
        
	}

    public void SubtractMoney(int amount, Sprite deathSprite = null)
    {
        currentMoney -= amount;
		currentMoneyLabel.text = "$" + currentMoney;

		SpawnPopup(-amount);

        lastDamageDeathSprite = deathSprite;

        if (currentMoney <= 0) GameManager.Instance.OnLose();

        
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
		currentMoneyLabel.text = "$" + currentMoney;
        totalMoneyCollected += amount;
		SpawnPopup(amount);
    }

    public void EnableUI() { moneyContainer.style.visibility = Visibility.Visible; }
	public void DisableUI() { moneyContainer.style.visibility = Visibility.Hidden; }

    public int GetTotalMoney() { return totalMoneyCollected; }
}
