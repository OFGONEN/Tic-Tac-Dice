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
		allyDiceEvent.position = diceTransform.position;
		allyDiceEvent.party = Parties.Ally;

		allyDiceEvent.Raise();
	}

	[ Button( "#Test 2: Enemy Dice Event Tile Response" ) ]
	public void EnemyDiceEventTileResponse()
	{
		enemyDiceEvent.position = diceTransform.position;
		enemyDiceEvent.party    = Parties.Enemy;

		enemyDiceEvent.Raise();
	}

#endregion

#region Implementation
#endregion

#region EditorOnly
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.DrawWireCube( diceTransform.position, Vector3.one );
	}
#endif
#endregion

}
