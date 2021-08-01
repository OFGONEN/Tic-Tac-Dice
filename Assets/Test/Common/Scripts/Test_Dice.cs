/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using FFStudio;
using NaughtyAttributes;
using UnityEngine;

public class Test_Dice : MonoBehaviour
{
#region Fields
	public DicePool allyDicePool;
	public DicePool enemyDicePool;

	public Vector3 launchVector;

	[ ReadOnly ] public Dice currentDice;
#endregion

#region Unity API
	private void Awake()
	{
		allyDicePool.mainParent = transform;
		allyDicePool.InitPool( transform, false );

		enemyDicePool.mainParent = transform;
		enemyDicePool.InitPool( transform, false );
	}
#endregion

#region API

	[ Button() ]
	public void SpawnADice()
	{
		currentDice = allyDicePool.GiveEntity( transform, true );
		currentDice.Spawn( Vector3.forward * 20, Quaternion.identity );
	}

	[ Button() ]
	public void Launch()
	{
		currentDice.Launch( launchVector );
	}

	[Button()]
	public void Halt()
	{
		currentDice.Halt();
	}

	[ Button() ]
	public void HaltAndLaunch()
	{
		Halt();
		Launch();
	}

	[ Button() ]
	public void SpawnAndHaltAndLaunch()
	{
		HaltAndLaunch();

		var newDice = allyDicePool.GiveEntity( transform, true );

		newDice.Spawn( currentDice.transform.position, currentDice.transform.rotation );
		newDice.Launch( new Vector3( -10, 5, -5 ) );
	}

	[ Button() ]
	public void SpawnEnemyDice()
	{
		var allyDice = allyDicePool.GiveEntity( transform, true );
		allyDice.Spawn( Vector3.forward * 10, Quaternion.identity );

		var enemyDice = enemyDicePool.GiveEntity( transform, true );
		enemyDice.Spawn( Vector3.forward * -10, Quaternion.identity );

		var allyLaunch = launchVector;
		var enemyLaunch = launchVector;

		enemyLaunch.z *= -1f;

		allyDice.Launch( allyLaunch );
		enemyDice.Launch( enemyLaunch );
	}

#endregion

#region Implementation
#endregion
}
