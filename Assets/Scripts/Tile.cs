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
		var spawnPosition = GiveClampedDiceEventPosition( diceEvent.position );

		FFLogger.DrawLine( transform.position, spawnPosition, Color.green, 2f );
	}

	public void EnemyDiceEventResponse( DiceEvent diceEvent )
	{
		FFLogger.Log( "Enemy Dice Response: " + name, gameObject );

		var spawnPosition = GiveClampedDiceEventPosition( diceEvent.position );

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
#endregion

#region EditorOnly
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
	}
#endif
#endregion
}
