/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using UnityEditor;

public class Tile : MonoBehaviour
{
#region Fields
	[ Header( "Event Listeners" ) ]

	[ Header( "Fired Events" ) ]
	public TileEvent tileCaptureEvent;


	[ Header( "Shared Variables" ) ]
	public TileSet tileSet;
	public SoldierPool allySoldierPool;
	public SoldierPool enemySoldierPool;

	[Header( "Setup" )]
	public int tileID;

	// Private Fields \\

	// Components 
	private MeshRenderer meshRenderer;
	private Bounds bounds;
#endregion

#region Unity API
	private void OnEnable()
	{
		// Subscribe to set
		tileSet.AddDictionary( tileID, this );
		tileSet.AddList( this );
	}

	private void OnDisable()
	{
		// UnSubscribe to set
		tileSet.RemoveDictionary( tileID );
		tileSet.RemoveList( this );
	}

	private void Awake()
	{
		meshRenderer = GetComponentInChildren< MeshRenderer >();
		bounds       = meshRenderer.bounds;
	}
#endregion

#region API
	public void AllyDiceEventResponse( DiceEvent diceEvent )
	{
		FFLogger.Log( "Ally Dice Response: " + name, gameObject );
		var spawnPosition = GiveClampedDiceEventPosition( diceEvent.position ); // Control spawn position. 

		// Spawn soldiers.
		SpawnSoldiers( allySoldierPool, diceEvent.diceNumber, spawnPosition );

		FFLogger.DrawLine( transform.position, spawnPosition, Color.green, 2f );
	}

	public void EnemyDiceEventResponse( DiceEvent diceEvent )
	{
		FFLogger.Log( "Enemy Dice Response: " + name, gameObject );

		var spawnPosition = GiveClampedDiceEventPosition( diceEvent.position ); // Control spawn position.

		// Spawn soldiers.
		SpawnSoldiers( enemySoldierPool, diceEvent.diceNumber, spawnPosition );

		FFLogger.DrawLine( transform.position, spawnPosition, Color.red, 2f );
	}
#endregion

#region Implementation
	// Clamp dice event position for spawning every soldier inside the bounds
	private Vector3 GiveClampedDiceEventPosition( Vector3 diceEventPosition )
	{
		var eventPosition = diceEventPosition;

		var min = bounds.min + Vector3.one * GameSettings.Instance.dice_SoldierSpawnRadius;
		var max = bounds.max - Vector3.one * GameSettings.Instance.dice_SoldierSpawnRadius;

		eventPosition = new Vector3 (
			Mathf.Clamp( eventPosition.x, min.x, max.x ),
			0,
			Mathf.Clamp( eventPosition.z, min.z, max.z )
		);

		return eventPosition;
	}

	// Spawns soliders in random positions inside a circle with given radius variable.
	private void SpawnSoldiers( SoldierPool soldierPool, int soldierCount, Vector3 spawnPosition )
	{
		for( var i = 0; i < soldierCount; i++ )
		{
			var soldier = allySoldierPool.GiveEntity( transform, false );

			// Select a random point inside a sphere.
			var randomSpawnPosition   = Random.insideUnitSphere * GameSettings.Instance.dice_SoldierSpawnRadius;
			    randomSpawnPosition.y = 0; // Zero out its Y position.
			    randomSpawnPosition   = spawnPosition + randomSpawnPosition; // Random spawn point.

			var randomSpawnRotation = Quaternion.Euler( new Vector3( 0, Random.Range( 0f, 360f ), 0 ) ); // Random rotation in +Y axis.

			soldier.Spawn( randomSpawnPosition, randomSpawnRotation );
		}
	}
#endregion

#region EditorOnly
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
	}
#endif
#endregion
}
