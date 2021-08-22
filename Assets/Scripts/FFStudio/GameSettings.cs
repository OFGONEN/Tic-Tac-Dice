/* Created by and for usage of FF Studios (2021). */

using NaughtyAttributes;
using UnityEngine;

namespace FFStudio
{
	public class GameSettings : ScriptableObject
    {
#region Fields
        public int maxLevelCount;

        // UI
        [ Foldout( "UI Settings" ), Tooltip( "Duration of the movement for ui element"          ) ] public float ui_Entity_Move_TweenDuration;
        [ Foldout( "UI Settings" ), Tooltip( "Duration of the fading for ui element"            ) ] public float ui_Entity_Fade_TweenDuration;
		[ Foldout( "UI Settings" ), Tooltip( "Duration of the scaling for ui element"           ) ] public float ui_Entity_Scale_TweenDuration;
		[ Foldout( "UI Settings" ), Tooltip( "Duration of the movement for floating ui element" ) ] public float ui_Entity_FloatingMove_TweenDuration;
        [ Foldout( "UI Settings" ), Tooltip( "Percentage of the screen to register a swipe"     ) ] public int swipeThreshold;

        // Dice
        [ BoxGroup( "Dice" ), Tooltip( "Dice wait time to be launched again" ) ] public float dice_coolDown = 2f; 
        [ BoxGroup( "Dice" ), Tooltip( "Dice's trajectory point count" ) ] public int dice_TrajectoryPointCount = 50; 
        [ BoxGroup( "Dice" ), Tooltip( "Dice's trajectory point count" ), MinMaxSlider( 0.5f, 3f ) ] public Vector2 dice_MinMaxTravelTime;
        [ BoxGroup( "Dice" ), Tooltip( "Dice wait time to disappear it after rigidbody sleep" ) ] public float dice_waitTimeAfterSleep = 0.35f; 
        [ BoxGroup( "Dice" ), Tooltip( "Dice's soldier spawn radius" ) ] public float dice_SoldierSpawnRadius = 1.5f; 

        // Tile
        [ BoxGroup( "Tile" ), Tooltip( "Tile's wait time for merging soldiers" ) ] public float tile_MergeWaitTime = 1.5f; 
        [ BoxGroup( "Tile" ), Tooltip( "Tile's wait time for giving attacking order to soldiers" ) ] public float tile_AttackWaitTime = 0.75f; 
        [ BoxGroup( "Tile" ), Tooltip( "Tile's color when captured by Ally" ) ] public Color tile_allyColor = Color.blue; 
        [ BoxGroup( "Tile" ), Tooltip( "Tile's color when captured by Enemy" ) ] public Color tile_enemyColor = Color.red; 

        // Player
        [ BoxGroup( "Player" ), Tooltip( "Player dice throw target move speed." ) ] public float player_TargetMoveSpeed = 5f; 

        // AI
        [ BoxGroup( "AI" ), Tooltip( "Random point radius" ) ] public float ai_randomPointRadius = 2f; 
        [ BoxGroup( "AI" ), Tooltip( "Random point radius" ), MinMaxSlider( 1f, 3f) ] public Vector2 ai_target_AimTime; 
        [ BoxGroup( "AI" ), Tooltip( "Random point radius" ), MinMaxSlider( 0.5f, 2f) ] public Vector2 ai_target_WaitTime; 

        // Stored Variables
        [ HideInInspector ] public static float board_DistanceBetweenTargetPoints;


        private static GameSettings instance;

        private delegate GameSettings ReturnGameSettings();
        private static ReturnGameSettings returnInstance = LoadInstance;

        public static GameSettings Instance
        {
            get
            {
                return returnInstance();
            }
        }
#endregion

#region Implementation
        static GameSettings LoadInstance()
		{
			if( instance == null )
				instance = Resources.Load< GameSettings >( "game_settings" );

			returnInstance = ReturnInstance;

			return instance;
		}

		static GameSettings ReturnInstance()
        {
            return instance;
        }
#endregion
    }
}
