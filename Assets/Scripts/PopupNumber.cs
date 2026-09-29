using TMPro;
using UnityEngine;

public class PopupNumber : MonoBehaviour
{
    TextMeshProUGUI text;

    [SerializeField] private float duration = 3f;
    float currentTimer = 0;
    [SerializeField] private float floatSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

	private void OnEnable()
	{
        currentTimer = 0;
	}
	// Update is called once per frame
	void Update()
    {
        if (currentTimer < duration)
        {
            currentTimer += Time.deltaTime;
        }
        else { gameObject.SetActive(false); }

        transform.Translate(Vector2.up * floatSpeed *  Time.deltaTime);
    }

    public void SetText(int amount, float size)
    {
        if (amount < 0)
        {
            text.color = Color.red;
            text.text = "-$" + Mathf.Abs(amount);
        }
        else
        {
            text.color = Color.green;
            text.text = "$" + amount;
        }

        text.fontSize = size; 
    }
}
