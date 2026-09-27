using System.Collections.Generic;
using UnityEngine;

public class TaxBulletPool : MonoBehaviour
{
    public static TaxBulletPool instance;
    Queue<TaxBullet> activeBullets = new();
    Queue<TaxBullet> inactiveBullets = new();
    [SerializeField] List<TaxBullet> bullets = new List<TaxBullet>();
    [SerializeField] GameObject bulletPrefab;

	private void Awake()
	{
		if (instance == null) instance = this;
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
       foreach (TaxBullet bullet in  bullets) 
            inactiveBullets.Enqueue(bullet);
    }

	private void FixedUpdate()
	{
		if (activeBullets.TryPeek(out TaxBullet b))
        {
            if (b.gameObject.activeSelf == false)
            {
                activeBullets.Dequeue();
                inactiveBullets.Enqueue(b);
            }
        }
	}

	public void DespawnOldestBullet()
	{
        if (activeBullets.TryDequeue(out TaxBullet b))
        {
            b.gameObject.SetActive(false);
            inactiveBullets.Enqueue(b);
        }
	}

	public void SpawnBullet(Vector3 pos, Vector2 dir, float speed, int damage, float force)
    {
        if (bullets.Count == 0) return;

        if (inactiveBullets.TryDequeue(out TaxBullet b))
        {
			b.gameObject.SetActive(true);
			// Spawn a bullet
			b.direction = dir;
            b.bulletSpeed = speed;
            b.bulletDamage = damage;
            b.transform.position = pos;
            b.bulletForce = force;
  
            
            b.ResetBullet();
            activeBullets.Enqueue(b);
        }
        else
        {
            // Despawn oldest bullet
            DespawnOldestBullet();
            TaxBullet bullet = inactiveBullets.Dequeue();

			bullet.gameObject.SetActive(true);
			// Spawn a bullet
			bullet.direction = dir;
			bullet.bulletSpeed = speed;
			bullet.bulletDamage = damage;
			bullet.transform.position = pos;
			bullet.bulletForce = force;
            
			
            bullet.ResetBullet();
			activeBullets.Enqueue(bullet);
		}
    }

    
}
