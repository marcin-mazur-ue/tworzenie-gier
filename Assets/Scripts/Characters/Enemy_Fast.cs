using UnityEngine;
using UnityEngine.AI;

public class Enemy_Fast : Enemy_Basic
{
	protected override void add_attacks()
	{
		attacks = new Attack[]
		{
			new Attack(10, 0.175f, 0.75f, 1.5f, 800)
		};
	}
}
