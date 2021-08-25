/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "Tile Event", menuName = "FF/Event/Game/Tile" ) ]
public class TileEvent : GameEvent
{
	[ HideInInspector ] public Parties party;
	[ HideInInspector ] public int tileID;
}
