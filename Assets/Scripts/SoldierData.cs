/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ CreateAssetMenu( fileName = "SoldierData", menuName = "FF/Data/Game/SoldierData" ) ]
public class SoldierData : ScriptableObject
{
	public int health;
	public float radius;
	public float attackCooldown;
	public float speed_Running;
	public float speed_Rotation;
	public bool canDiceKill;

}
