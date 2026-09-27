using UnityEngine;

public class CowboyShotCaller : MonoBehaviour
{
    private Cowboy cowboyMain;
	private void Awake()
	{
		cowboyMain = transform.parent.GetComponent<Cowboy>();
	}
	public void ShotCall()
	{
		cowboyMain.Shoot();
	}

	public void EndAttack()
	{
		cowboyMain.EndOfAttack();
	}
}
