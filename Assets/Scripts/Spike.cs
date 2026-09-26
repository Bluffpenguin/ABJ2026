using UnityEngine;

public class Spike : MonoBehaviour
{
    [Header("Damage Variables")]
    [SerializeField] float force = 10f;
    [SerializeField] int cost = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.tag == "Player")
        {
            PlayerController.Instance.DamageWithForce(force, transform, cost);
        }
	}
}
