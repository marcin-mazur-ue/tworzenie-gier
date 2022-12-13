using UnityEngine;
using UnityEngine.UI;

public class Enemy : EnemyBase
{
	
	void Start()
	{
		initialization();
	}

	void FixedUpdate()
	{
		movement();
	}

	void Update()
	{
		attack_check();
	}



}
