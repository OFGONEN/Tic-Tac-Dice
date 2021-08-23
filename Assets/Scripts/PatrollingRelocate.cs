/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class PatrollingRelocate : MonoBehaviour
{
#region Fields
    [ Header( "Setup" ) ]
    public TileSet tileSet;

#endregion

#region Properties
#endregion

#region Unity API
    private void OnTriggerEnter( Collider other )
    {
		var dice = other.GetComponentInParent< Dice >();

		// Spawn a new Dice.
		dice.Halt();

		//Find a random Tile.
		Tile tile;
		tileSet.itemDictionary.TryGetValue( Random.Range( 0, 9 ), out tile );

        // Calculate a launch vector
		var tilePosition   = tile.transform.position;
		    tilePosition.y = 0;

		var position   = dice.transform.position;
		    position.y = 0;

		var distanceRatio = Vector3.Distance( tilePosition, position ) / GameSettings.board_DistanceBetweenTargetPoints; // Calculate a ratio between the max distance and the current distance.

		var minMax_TravelTime = GameSettings.Instance.dice_MinMaxTravelTime;
		var travelTime   = Mathf.Lerp( minMax_TravelTime.x, minMax_TravelTime.y, distanceRatio );  // Find the travel time according the distance ratio.

		Vector3 launchVector;
		launchVector.x = ( tilePosition.x - position.x ) / travelTime;
		launchVector.z = ( tilePosition.z - position.z ) / travelTime;
		launchVector.y = -Physics.gravity.y * travelTime / 2;

		dice.Launch( launchVector );

        //TODO(ofg): Spawn a particle effect
		gameObject.SetActive( false );
	}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
