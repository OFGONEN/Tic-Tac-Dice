/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;

public class AIController : MonoBehaviour
{
#region Fields
    [ Header( "Event Listeners " ) ]
    public EventListenerDelegateResponse levelStartListener;
    public MultipleEventListenerDelegateResponse levelFinishListener;

    [ Header( "Setup " ) ]
    public TileSet tileSet;
    public DiceThrower diceThrower;
    public Transform target;

    // Private Fields \\
    private bool targeting = false;

    // Delegates
    private Tween aimTween;
    private UnityMessage updateMethod;

#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		levelStartListener.OnEnable();
		levelFinishListener.OnEnable();
	}

    private void OnDisable()
    {
		levelStartListener.OnDisable();
		levelFinishListener.OnDisable();

        if( aimTween != null)
        {
			aimTween.Kill();
			aimTween = null;
		}
	}

    private void Awake()
    {
		levelStartListener.response  = LevelStartResponse;
		levelFinishListener.response = LevelFinishResponse;

		updateMethod = ExtensionMethods.EmptyMethod;

		target.gameObject.SetActive( false );
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region API
#endregion

#region Implementation
    private void LevelStartResponse()
    {
		DOVirtual.DelayedCall( GameSettings.Instance.ai_target_WaitTime.ReturnRandomValue(), AimDiceThrower );
	}

    private void LevelFinishResponse()
    {
        if( targeting && aimTween != null)
        {
			aimTween.Kill();
			aimTween = null;
		}

		updateMethod = ExtensionMethods.EmptyMethod;
		targeting = false;
	}

    private void AimDiceThrower()
    {
        // Find a random tile to throw the dice to.
		var randomID = Random.Range( 0, tileSet.itemList.Count );

		Tile randomTile;
		tileSet.itemDictionary.TryGetValue( randomID, out randomTile );

        // Calculate a random inside the tile. 
		var targetPosition   = randomTile.transform.position + Random.insideUnitSphere * GameSettings.Instance.ai_randomPointRadius;
		    targetPosition.y = 0;

        // Enable and update trajectory line.
		diceThrower.EnableTrajectoryLine();
		diceThrower.UpdateTargetPoint( diceThrower.ClosestTargetPosition );

        // Update target gameobject.
		targeting = true;
		target.gameObject.SetActive( true );
		target.position = diceThrower.ClosestTargetPosition;

        // Create a tween for aiming.
		var aimDuration = GameSettings.Instance.ai_target_AimTime.ReturnRandomValue();
		aimTween = target.DOMove( targetPosition, aimDuration );
		aimTween.OnUpdate( OnAimUpdate );
		aimTween.OnComplete( OnAimComplete );
	}

    private void OnAimUpdate()
    {
		diceThrower.UpdateTargetPoint( target.position );
	}

    private void OnAimComplete()
    {
		aimTween = null;

		targeting = false;
		target.gameObject.SetActive( false );

		diceThrower.Launch();

		updateMethod = CheckIfCanThrow;
	}

    private void CheckIfCanThrow()
    {
        if( diceThrower.CanThrowDice )
        {
		    DOVirtual.DelayedCall( GameSettings.Instance.ai_target_WaitTime.ReturnRandomValue(), AimDiceThrower );
			updateMethod = ExtensionMethods.EmptyMethod;
		}
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
