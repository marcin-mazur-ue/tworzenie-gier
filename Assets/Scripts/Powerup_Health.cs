using UnityEngine;

public class Powerup_Health : MonoBehaviour
{
	[SerializeField] private int health_restored = 50;
	[SerializeField] private float rotation_speed = 2.0f;
	
	void Update()
	{
		transform.Rotate(new Vector3(0, rotation_speed, 0));
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == "Player")
		{
			collider.gameObject.GetComponent<Player>().heal(health_restored);
			Destroy(gameObject);
		}
	}
}
