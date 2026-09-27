using UnityEngine;

public class ObstacleBounce : MonoBehaviour
{
	[SerializeField] float bounceForce = 10f;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerController.Instance.ApplyForce(bounceForce, transform);
		}
	}
}
