using UnityEngine;

public class Powerup_Health : Powerup
{
	[SerializeField] private int health_restored = 50;
	
	protected override void pickup(Player player)
	{
		player.heal(health_restored);
	}
}
