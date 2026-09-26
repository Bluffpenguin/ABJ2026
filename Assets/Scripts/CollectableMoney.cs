using UnityEngine;

public class CollectableMoney : MonoBehaviour
{
    [SerializeField] int amountToGive = 10;

	private void Start()
	{
		GameManager.Instance.AddCoinToList(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			MoneyManager.Instance.AddMoney(amountToGive);
			gameObject.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		GameManager.Instance.RemoveCoinFromList(gameObject);
	}
}
