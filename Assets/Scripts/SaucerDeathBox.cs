using UnityEngine;

public class SaucerDeathBox : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
        {
			// Call death
			GameManager.Instance.OnLose();
        }
	}
}
