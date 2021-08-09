/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

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
	}
#endregion

#region API
	public void AllyDiceEventResponse( DiceEvent diceEvent )
	{
		FFLogger.Log( "Ally Dice Response: " + name, gameObject );
	}

	public void EnemyDiceEventResponse( DiceEvent diceEvent )
	{
		FFLogger.Log( "Enemy Dice Response: " + name, gameObject );
	}
#endregion

#region Implementation
#endregion
}
