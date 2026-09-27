using UnityEngine;

public class Cowboy : MonoBehaviour
{
    Animator anim;
    [SerializeField] Transform animTransform;
    [SerializeField] GameObject bulletPrefab;
    bool foundPlayer = false;

    [SerializeField] float patrolRange;
	[SerializeField] float patrolSpeed;

	[SerializeField] Vector2 direction = Vector2.left;
    Vector2 startingPosition;

    [Header("Collision Check")]
    [SerializeField] LayerMask collisionMask;
    [SerializeField] float collisionCheckDistance = 1;
    [SerializeField] Vector2 castSize = new(1, 1);
    [SerializeField] Vector2 collisionOffset = new(0,1);

    [Header("Attack Variables")]
    [SerializeField] float attackDelay = 1;
    float delayProgress = 1;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletForce;
    [SerializeField] int bulletDamage;
    
    [SerializeField] Vector2 bulletSpawnOffset = new(0,0);
    GameObject player;
    bool attacking = false;

	private void Awake()
	{
        anim = GetComponentInChildren<Animator>();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
       startingPosition = transform.position;
        delayProgress = attackDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!foundPlayer)
        {
            transform.Translate(patrolSpeed * direction * Time.deltaTime);
        }
        else
        {
            delayProgress += Time.deltaTime;
            Vector2 directionToPlayer;
            if (player.transform.position.x < transform.position.x) directionToPlayer = Vector2.left;
            else directionToPlayer = Vector2.right;

            direction = directionToPlayer;

            if (delayProgress >= attackDelay)
            {
                // Try to attack
                //GameObject bullet = Instantiate(bulletPrefab, transform.position + (Vector3)(bulletSpawnOffset * new Vector2(directionToPlayer.x, 1)), Quaternion.identity);
                anim.SetTrigger("Shoot");
                attacking = true;
                delayProgress = 0;
            }
            
        }
    }

	private void FixedUpdate()
	{
        if (!foundPlayer || !attacking)
        {
			if (Physics2D.BoxCast(transform.position + (Vector3)collisionOffset, castSize, 0, direction, collisionCheckDistance, collisionMask))
			{
				direction *= -1;
			}

			if (direction == Vector2.left) animTransform.localScale = new(1, 1, 1);
			else animTransform.localScale = new(-1, 1, 1);
		}
        
	}

    public void Shoot()
    {
		TaxBulletPool.instance.SpawnBullet(transform.position + (Vector3)(bulletSpawnOffset * new Vector2(direction.x, 1)), direction, bulletSpeed, bulletDamage, bulletForce);
		delayProgress = 0;
	}

    public void EndOfAttack()
    {
        attacking = false;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        foundPlayer = true;
        player = collision.gameObject;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
        foundPlayer = false;
        player = null;
	}

	private void OnDrawGizmosSelected()
	{
        Gizmos.DrawSphere(transform.position + new Vector3(patrolRange, 0, 0), 0.2f);
		Gizmos.DrawSphere(transform.position - new Vector3(patrolRange, 0, 0), 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + (Vector3)collisionOffset, transform.position + (Vector3)(collisionCheckDistance * direction) + (Vector3)collisionOffset);

        Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position + (Vector3)(bulletSpawnOffset), 0.1f);
	}
}
