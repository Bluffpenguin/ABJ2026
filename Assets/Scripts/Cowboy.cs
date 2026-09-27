using UnityEngine;

public class Cowboy : MonoBehaviour
{
    Animator anim;
    [SerializeField] Transform animTransform;
    [SerializeField] GameObject bulletPrefab;
    bool foundPlayer = false;

    [SerializeField] float patrolRange;
	[SerializeField] float patrolSpeed;

	Vector2 direction = Vector2.left;
    Vector2 startingPosition;


	private void Awake()
	{
        anim = GetComponentInChildren<Animator>();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
       startingPosition = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!foundPlayer)
        {
            transform.Translate(patrolSpeed * direction * Time.deltaTime);

            
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
        {
            foundPlayer = true;
        }
	}
}
