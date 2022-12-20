using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
	[SerializeField] private float rotation_speed = 2.0f;
	[SerializeField] private Transform powerup_icon;
	
	void Update()
	{
		powerup_icon.Rotate(new Vector3(0, rotation_speed, 0));
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == "Player")
		{
			pickup(collider.gameObject.GetComponent<Player>());
			Destroy(gameObject);
		}
	}
	
	protected abstract void pickup(Player player);
}
