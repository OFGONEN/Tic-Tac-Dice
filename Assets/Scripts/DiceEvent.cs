/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "Dice Event", menuName = "FF/Event/Game/Dice" ) ]
public class DiceEvent : GameEvent
{
	[ HideInInspector ] public Vector3 position;
	[ HideInInspector ] public int diceNumber;
	[ HideInInspector ] public Parties party;
	[ HideInInspector ] public SoldierType soldierType;
	[ HideInInspector ] public int diceID;
}
