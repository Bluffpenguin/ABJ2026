using UnityEngine;

public class TaxBullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [HideInInspector] public float bulletSpeed = 1;
    [HideInInspector] public int bulletDamage = 50;
    [HideInInspector] public float bulletForce = 1;
    [SerializeField] public Vector2 direction = Vector2.zero;
    [SerializeField] float lifespan = 10f;
    [SerializeField] float rotationSpeed = 1;
    SpriteRenderer spriteRenderer;
    float lifespanProgress = 0;

	private void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}
	private void FixedUpdate()
	{
        lifespanProgress += Time.fixedDeltaTime;
        if (lifespanProgress > lifespan) gameObject.SetActive(false);

        transform.Translate(direction * bulletSpeed * Time.fixedDeltaTime);
        spriteRenderer.transform.rotation = new Quaternion(0, 0, transform.rotation.z + rotationSpeed * Time.fixedDeltaTime, 0);
	}

    public void ResetBullet()
    {
        spriteRenderer.transform.rotation = Quaternion.identity;
        lifespanProgress = 0;
    }
	private void OnCollisionEnter2D(Collision2D collision)
	{
		PlayerController.Instance.DamageWithForce(bulletForce, transform, bulletDamage);
		gameObject.SetActive(false);
	}
}
