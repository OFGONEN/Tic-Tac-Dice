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
        [ BoxGroup( "Dice" ), Tooltip( "Dice's trajectory point count" ) ] public int dice_TrajectoryPointCount = 50; 
        [ BoxGroup( "Dice" ), Tooltip( "Dice's trajectory point count" ), MinMaxSlider( 0.5f, 3f ) ] public Vector2 dice_MinMaxTravelTime;
        [ BoxGroup( "Dice" ), Tooltip( "Dice wait time to disappear it after rigidbody sleep" ) ] public float dice_waitTimeAfterSleep = 0.35f; 
        [ BoxGroup( "Dice" ), Tooltip( "Dice's soldier spawn radius" ) ] public float dice_SoldierSpawnRadius = 1.5f; 
        [ BoxGroup( "Dice" ), Tooltip( "Dice's target transform +Y value" ) ] public float dice_TargetHeight = 0.15f; 

        // Tile
        [ BoxGroup( "Tile" ), Tooltip( "Tile's wait time for merging soldiers" ) ] public float tile_MergeWaitTime = 1.5f; 
        [ BoxGroup( "Tile" ), Tooltip( "Tile's wait time for giving attacking order to soldiers" ) ] public float tile_AttackWaitTime = 0.75f; 
        [ BoxGroup( "Tile" ), Tooltip( "Tile's color when captured by Ally" ) ] public Color tile_allyColor = Color.blue; 
        [ BoxGroup( "Tile" ), Tooltip( "Tile's color when captured by Enemy" ) ] public Color tile_enemyColor = Color.red;
        [ BoxGroup( "Tile" ), Tooltip( "Tile's texture when captured by Ally" ) ] public Sprite tile_allyTexture;
        [ BoxGroup( "Tile" ), Tooltip( "Tile's texture when captured by Enemy" ) ] public Sprite tile_enemyTexture;

		// Soldier
		[BoxGroup( "Soldier" ), Tooltip( "Damage delay after animation is played" )] public float soldier_AttackDelay = 0.75f; 

        // Player
        [ BoxGroup( "Player" ), Tooltip( "Player dice throw target move speed." ) ] public float player_TargetMoveSpeed = 5f; 
        [ BoxGroup( "Player" ), Tooltip( "Player's dice wait time to be launched again" ) ] public float player_Dice_Cooldown = 4f; 

        // AI
        [ BoxGroup( "AI" ), Tooltip( "Random point radius" ) ] public float ai_randomPointRadius = 2f; 
        [ BoxGroup( "AI" ), Tooltip( "Random point radius" ), MinMaxSlider( 1f, 3f) ] public Vector2 ai_target_AimTime; 
        [ BoxGroup( "AI" ), Tooltip( "Random point radius" ), MinMaxSlider( 0.5f, 2f) ] public Vector2 ai_target_WaitTime; 
        [ BoxGroup( "AI" ), Tooltip( "AI's dice wait time to be launched again" ) ] public float ai_Dice_Cooldown = 3f; 

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
