using UnityEngine;
using System.Collections.Generic;

public class Trashcan : MonoBehaviour
{
    bool knockedOver = false;
    [SerializeField] List<GameObject> inside;
    [SerializeField] Sprite baseSprite;
    [SerializeField] Sprite knockOverSprite;
    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = baseSprite;

        foreach (GameObject go in inside)
        {
            go.SetActive(false);
        }
    }


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!knockedOver)
		{
			foreach (GameObject go in inside)
			{
				go.SetActive(true);
			}
		}
		knockedOver = true;
		spriteRenderer.sprite = knockOverSprite;
	}
}
