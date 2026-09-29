using System.Collections.Generic;
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

    [Header("Popup Numbers")]
    [SerializeField] List<PopupNumber> popupNumbers = new List<PopupNumber>();
    Queue<PopupNumber> popUpQueue = new Queue<PopupNumber>();
    [SerializeField] float basePopupSize = 0.4f;
    [SerializeField] float popupSizeIncrementAmount = 0.05f;
    [SerializeField] int popupSizeIncrementThreshold = 25;


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
        
        foreach (PopupNumber number in popupNumbers)
        {
            popUpQueue.Enqueue(number);
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

    void SpawnWorldPopup(int cost, Vector3 position)
    {
        if (popUpQueue.Count <= 0)
        {
            foreach (PopupNumber popup in popupNumbers)
            {
                popUpQueue.Enqueue(popup);
            }
        }
        PopupNumber popUp = popUpQueue.Dequeue();
        float textSize = basePopupSize + (popupSizeIncrementAmount * (Mathf.Abs(cost) / popupSizeIncrementThreshold));

        popUp.transform.position = position;
        popUp.gameObject.SetActive(true);
        popUp.SetText(cost, textSize);
    }

    public void ResetMoney()
    {
        currentMoney = startingMoney;
		currentMoneyLabel.text = "$" + currentMoney;
        costFadeContainer.Clear();
        containerSize = 0;
        totalMoneyCollected = startingMoney;
        foreach (PopupNumber popUp in popupNumbers)
        {
            popUp.gameObject.SetActive(false);
        }
        
	}

    public void SubtractMoney(int amount, Vector3 popUpPosition, Sprite deathSprite = null)
    {
        currentMoney -= amount;
		currentMoneyLabel.text = "$" + currentMoney;

		SpawnWorldPopup(-amount, popUpPosition);

        lastDamageDeathSprite = deathSprite;

        if (currentMoney <= 0) GameManager.Instance.OnLose();

        
    }

    public void AddMoney(int amount, Vector3 popUpPosition)
    {
        currentMoney += amount;
		currentMoneyLabel.text = "$" + currentMoney;
        totalMoneyCollected += amount;
		SpawnWorldPopup(amount, popUpPosition);
    }

    public void EnableUI() { moneyContainer.style.visibility = Visibility.Visible; }
	public void DisableUI() { moneyContainer.style.visibility = Visibility.Hidden; }

    public int GetTotalMoney() { return totalMoneyCollected; }
}
