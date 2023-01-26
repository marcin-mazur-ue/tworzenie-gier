using UnityEngine;
using UnityEngine.AI;

public class Enemy_Strong : Enemy_Basic
{
	protected override void add_attacks()
	{
		attacks = new Attack[]
		{
			new Attack(50, 0.55f, 3.0f, 0.8f, 4000)
		};
	}
}
