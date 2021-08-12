/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using NaughtyAttributes;
using UnityEditor;

public class Test_TileCapture : MonoBehaviour
{
#region Fields
	[ BoxGroup( "Setup" ) ] public GameEvent levelStartEvent;

	[ BoxGroup( "Tile Capture Test" ) ] public TileEvent tileCaptureEvent;
	[ BoxGroup( "Tile Capture Test" ) ] public int tileID;
	[ BoxGroup( "Tile Capture Test" ) ] public Parties tileParty = Parties.Ally;


	[ BoxGroup( "Dice Event Tile Response Test" ) ] public DiceEvent allyDiceEvent;
	[ BoxGroup( "Dice Event Tile Response Test" ) ] public DiceEvent enemyDiceEvent;
	[ BoxGroup( "Dice Event Tile Response Test" ) ] public Transform diceTransform;


	[ BoxGroup( "Soldier Attack Test" ) ] public Soldier allySoldier;
	[ BoxGroup( "Soldier Attack Test" ) ] public Soldier allySuperSoldier;
	[ BoxGroup( "Soldier Attack Test" ) ] public Soldier enemySoldier;
	[ BoxGroup( "Soldier Attack Test" ) ] public Soldier enemySuperSoldier;
#endregion

#region Unity API
	private void Start()
	{
		levelStartEvent.Raise();
	}
#endregion

#region API
	[ Button( "#Test 1: Tile Capture" ) ]
	public void TileCapture()
	{
		tileCaptureEvent.tileID = tileID;
		tileCaptureEvent.party  = tileParty;

		tileCaptureEvent.Raise();
	}

	[ Button( "#Test 2: Ally Dice Event Tile Response" ) ]
	public void AllyDiceEventTileResponse()
	{
		allyDiceEvent.diceNumber = 3;
		allyDiceEvent.position = diceTransform.position;
		allyDiceEvent.party = Parties.Ally;

		allyDiceEvent.Raise();
	}

	[ Button( "#Test 2: Enemy Dice Event Tile Response" ) ]
	public void EnemyDiceEventTileResponse()
	{
		enemyDiceEvent.diceNumber = 3;
		enemyDiceEvent.position = diceTransform.position;
		enemyDiceEvent.party    = Parties.Enemy;

		enemyDiceEvent.Raise();
	}


	[ Button( "#Test 3: Soldiers Attack 1-1" ) ]
	public void SoldierAttackOneOnOne()
	{
		// Call spawn methods for setting variables to default values such as, alive variable to true.
		allySoldier.Spawn( allySoldier.transform.position, allySoldier.transform.rotation );
		enemySoldier.Spawn( enemySoldier.transform.position, enemySoldier.transform.rotation );

		allySoldier.Attack( enemySoldier );
		enemySoldier.Attack( allySoldier );
	}

	[Button( "#Test 3: Soldiers Attack Super-Normal" )]
	public void SoldierAttackSuperOnNormal()
	{
		// Call spawn methods for setting variables to default values such as, alive variable to true.
		allySuperSoldier.Spawn( allySuperSoldier.transform.position, allySuperSoldier.transform.rotation );
		enemySoldier.Spawn( enemySoldier.transform.position, enemySoldier.transform.rotation );

		allySuperSoldier.Attack( enemySoldier );
		enemySoldier.Attack( allySuperSoldier );
	}

	[Button( "#Test 3: Soldiers Attack Super-Super" )]
	public void SoldierAttackSuperOnSuper()
	{
		// Call spawn methods for setting variables to default values such as, alive variable to true.
		allySuperSoldier.Spawn( allySuperSoldier.transform.position, allySuperSoldier.transform.rotation );
		enemySuperSoldier.Spawn( enemySuperSoldier.transform.position, enemySuperSoldier.transform.rotation );

		allySuperSoldier.Attack( enemySuperSoldier );
		enemySuperSoldier.Attack( allySuperSoldier );
	}

	[Button( "#Test 3: Soldiers Attack 2-Normal" )]
	public void SoldierAttackTwoOnOne()
	{
		// Call spawn methods for setting variables to default values such as, alive variable to true.
		allySoldier.Spawn( allySoldier.transform.position, allySoldier.transform.rotation );
		allySuperSoldier.Spawn( allySuperSoldier.transform.position, allySuperSoldier.transform.rotation );
		enemySoldier.Spawn( enemySoldier.transform.position, enemySoldier.transform.rotation );

		allySoldier.Attack( enemySoldier );
		allySuperSoldier.Attack( enemySoldier );
		enemySoldier.Attack( allySoldier );
	}

#endregion

#region Implementation
#endregion

#region EditorOnly
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.DrawWireCube( diceTransform.position, Vector3.one / 2 );
	}
#endif
#endregion

}
