/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using NaughtyAttributes;

/* This class holds references to ScriptableObject assets. These ScriptableObjects are singletons, so they need to load before a Scene does.
 * Using this class ensures at least one script from a scene holds a reference to these important ScriptableObjects. */
public class AppAssetHolder : MonoBehaviour
{
#region Fields
	public GameSettings gameSettings;
	public CurrentLevelData currentLevelData;

	[ BoxGroup( "Pools" ) ] public DicePool pool_dice_ally;
	[ BoxGroup( "Pools" ) ] public DicePool pool_dice_enemy;
	[ BoxGroup( "Pools" ) ] public SoldierPool pool_soldier_ally;
	[ BoxGroup( "Pools" ) ] public SoldierPool pool_soldier_enemy;
	[ BoxGroup( "Pools" ) ] public SoldierPool pool_superSoldier_ally;
	[ BoxGroup( "Pools" ) ] public SoldierPool pool_superSoldier_enemy;


	private void Awake()
	{
		pool_dice_ally.InitPool( transform, false );
		pool_dice_enemy.InitPool( transform, false );
		pool_soldier_ally.InitPool( transform, false );
		pool_soldier_enemy.InitPool( transform, false );
		pool_superSoldier_ally.InitPool( transform, false );
		pool_superSoldier_enemy.InitPool( transform, false );
	}
#endregion
}
