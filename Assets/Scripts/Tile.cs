/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;

public class Tile : MonoBehaviour
{
#region Fields
	[ Header( "Event Listeners" ) ]

	[ Header( "Fired Events" ) ]
	public TileEvent tileCaptureEvent;


	[ Header( "Shared Variables" ) ]
	public TileSet tileSet;
	public SoldierPool allySoldierPool;
	public SoldierPool allySuperSoldierPool;
	public SoldierPool enemySoldierPool;
	public SoldierPool enemySuperSoldierPool;

	[Header( "Setup" )]
	public int tileID;

	// Private Fields \\

	[ SerializeField, ReadOnly ] private List< Soldier > ally_SoldierList = new List< Soldier >( 16 );
	[ SerializeField, ReadOnly ] private List< Soldier > ally_Normal_SoldierList = new List< Soldier >( 16 );
	[ SerializeField, ReadOnly ] private List< Soldier > ally_Super_SoldierList = new List< Soldier >( 16 );
	[ SerializeField, ReadOnly ] private List< Soldier > enemy_SoldierList = new List< Soldier >( 16 );
	[ SerializeField, ReadOnly ] private List< Soldier > enemy_Normal_SoldierList = new List< Soldier >( 16 );
	[ SerializeField, ReadOnly ] private List< Soldier > enemy_Super_SoldierList = new List< Soldier >( 16 );

	private List< Soldier > mergingSoldiers = new List< Soldier >( 16 );

	private Tween soldiersMergeTween;

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

		var spawnPosition = ClampDiceEventPosition( diceEvent.position ); // Control spawn position. 

		// Spawn soldiers.
		SpawnSoldiers( allySoldierPool, ally_SoldierList, ally_Normal_SoldierList, enemy_SoldierList, diceEvent.diceNumber, spawnPosition );

		if( enemy_SoldierList.Count > 0 )
		{
			// Kill delayed Merge before attacking.
			if( soldiersMergeTween != null)
			{
				soldiersMergeTween.Kill();
				soldiersMergeTween = null;
			}

			DOVirtual.DelayedCall( GameSettings.Instance.tile_AttackWaitTime, SoldiersAttack );
		}
		else 
		{
			// Merge soldiers
			soldiersMergeTween = DOVirtual.DelayedCall( GameSettings.Instance.tile_MergeWaitTime, MergeAllySoldiers );
		}

		FFLogger.DrawLine( transform.position, spawnPosition, Color.green, 2f );
	}

	public void EnemyDiceEventResponse( DiceEvent diceEvent )
	{
		FFLogger.Log( "Enemy Dice Response: " + name, gameObject );

		var spawnPosition = ClampDiceEventPosition( diceEvent.position ); // Control spawn position.

		// Spawn soldiers.
		SpawnSoldiers( enemySoldierPool, enemy_SoldierList, enemy_Normal_SoldierList, ally_SoldierList, diceEvent.diceNumber, spawnPosition );

		if( ally_SoldierList.Count > 0 )
		{
			// Kill delayed Merge before attacking.
			if( soldiersMergeTween != null)
			{
				soldiersMergeTween.Kill();
				soldiersMergeTween = null;
			}

			DOVirtual.DelayedCall( GameSettings.Instance.tile_AttackWaitTime, SoldiersAttack );
		}
		else 
		{
			// Merge soldiers
			soldiersMergeTween = DOVirtual.DelayedCall( GameSettings.Instance.tile_MergeWaitTime, MergeEnemySoldiers );
		}

		FFLogger.DrawLine( transform.position, spawnPosition, Color.red, 2f );
	}
#endregion

#region Implementation
	// Clamp dice event position for spawning every soldier inside the bounds
	private Vector3 ClampDiceEventPosition( Vector3 diceEventPosition )
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
	private void SpawnSoldiers( SoldierPool soldierPool, List< Soldier > alliedSoldierList, List< Soldier > alliedSoldierTypeList, List< Soldier > enemySoldierList, int soldierCount, Vector3 spawnPosition )
	{
		for( var i = 0; i < soldierCount; i++ )
		{
			var soldier = soldierPool.GiveEntity( transform, false );

			// Select a random point inside a sphere.
			var randomSpawnPosition   = Random.insideUnitSphere * GameSettings.Instance.dice_SoldierSpawnRadius;
			    randomSpawnPosition.y = 0; // Zero out its Y position.
			    randomSpawnPosition   = spawnPosition + randomSpawnPosition; // Random spawn point.

			var randomSpawnRotation = Quaternion.Euler( new Vector3( 0, Random.Range( 0f, 360f ), 0 ) ); // Random rotation in +Y axis.

			soldier.Spawn( randomSpawnPosition, randomSpawnRotation, alliedSoldierList, alliedSoldierTypeList, enemySoldierList );

			// Add spawned soldier to appropriate soldier lists
			alliedSoldierList.Add( soldier );
			alliedSoldierTypeList.Add( soldier );
		}
	}

	// Give a random point inside the bounds to spawn a soldier
	private Vector3 GiveRandomSpawnPoint()
	{
		var min = bounds.min + Vector3.one * GameSettings.Instance.dice_SoldierSpawnRadius;
		var max = bounds.max - Vector3.one * GameSettings.Instance.dice_SoldierSpawnRadius;

		return new Vector3( Random.Range( min.x, max.x ), 0, Random.Range( min.z, max.z ) );
	}

	// Give attack order to soldiers
	private void SoldiersAttack()
	{
		for( var i = 0; i < ally_SoldierList.Count; i++ )
		{
			ally_SoldierList[ i ].AttackClosest();
		}

		for( var i = 0; i < enemy_SoldierList.Count; i++ )
		{
			enemy_SoldierList[ i ].AttackClosest();
		}
	}

	private void MergeAllySoldiers()
	{
		MergeSoldiers( allySuperSoldierPool, ally_Normal_SoldierList, ally_SoldierList, ally_Super_SoldierList, enemy_SoldierList );
	}

	private void MergeEnemySoldiers()
	{
		MergeSoldiers( enemySuperSoldierPool, enemy_Normal_SoldierList, enemy_SoldierList, enemy_Super_SoldierList, ally_SoldierList );
	}

	private void MergeSoldiers( SoldierPool superSoldierPool, List< Soldier > normalSoldiersList, List< Soldier > allyList, List< Soldier > allySuperSoldierTypeList, List< Soldier > enemyList )
	{
		soldiersMergeTween = null;

		// If normal soldiers count is equal or less then 3, return.
		if( normalSoldiersList.Count <= 3 )
			return;

		mergingSoldiers.Clear(); // Clear temp list.
		mergingSoldiers.AddRange( normalSoldiersList ); // Cache the soldier reference since when they die they remove themselfes from the actual list.

		int superSoldierCount = ( int )( normalSoldiersList.Count / 2.5f ); // Number of super soldiers to spawn.

		// Kill normal soldiers.
		foreach( var soldier in mergingSoldiers )
		{
			soldier.Die();
		}

		// Spawn super soldiers.
		for( var i = 0; i < superSoldierCount; i++ )
		{
			var spawnPosition = GiveRandomSpawnPoint();

            var superSoldier = superSoldierPool.GiveEntity( transform, false );

			var randomSpawnRotation = Quaternion.Euler( new Vector3( 0, Random.Range( 0f, 360f ), 0 ) ); // Random rotation in +Y axis.

			superSoldier.Spawn( spawnPosition, randomSpawnRotation, allyList, allySuperSoldierTypeList, enemyList );

			// Add spawned soldier to appropriate soldier lists
			allyList.Add( superSoldier );
			allySuperSoldierTypeList.Add( superSoldier );
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
