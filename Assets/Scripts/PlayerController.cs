/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class PlayerController : MonoBehaviour
{
#region Fields
    [ Header( "Event Listeners " ) ]
    public EventListenerDelegateResponse levelStartListener;
    public MultipleEventListenerDelegateResponse levelFinishListener;

    [ Header( "Input Related " ) ] 
    public SharedBoolProperty inputActiveProperty;
    public SharedVector2Property inputDirectionProperty;

    [ Header( "Setup " ) ]
    public DiceThrower diceThrower;
    public Transform target;

    // Private Fields \\
    private bool targeting = false;
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
	}

    private void Awake()
    {
		levelStartListener.response  = LevelStartResponse;
		levelFinishListener.response = LevelFinishResponse;

		target.gameObject.SetActive( false );
	}
#endregion

#region API
#endregion

#region Implementation
    private void LevelStartResponse()
    {
		inputActiveProperty.changeEvet += InputActiveResponse;
	}

    private void LevelFinishResponse()
    {
		inputActiveProperty.changeEvet -= InputActiveResponse;

		targeting = false;
	}

    private void InputActiveResponse()
    {
        if( inputActiveProperty.sharedValue && diceThrower.CanThrowDice )
        {
			target.position = diceThrower.ClosestTargetPosition;

			diceThrower.EnableTrajectoryLine();
			diceThrower.UpdateTargetPoint( diceThrower.ClosestTargetPosition );

			targeting = true;
			target.gameObject.SetActive( true );
			inputDirectionProperty.changeEvent += InputDirectionChangeResponse;
		}
        else if ( targeting && !inputActiveProperty.sharedValue )
        {
			diceThrower.Launch();

			targeting = false;
			target.gameObject.SetActive( false );
            inputDirectionProperty.changeEvent -= InputDirectionChangeResponse;
		}
    }

    private void InputDirectionChangeResponse()
    {
		var direction  = new Vector3( inputDirectionProperty.sharedValue.x, 0, inputDirectionProperty.sharedValue.y );
		var position   = target.position + direction * Time.deltaTime * GameSettings.Instance.player_TargetMoveSpeed;
		    position.y = GameSettings.Instance.dice_TargetHeight;

		position.x = Mathf.Clamp( position.x, diceThrower.DownLeftPosition.x, diceThrower.UpRightPosition.x );
		position.z = Mathf.Clamp( position.z, diceThrower.DownLeftPosition.z, diceThrower.UpRightPosition.z );

		target.position = position;
		diceThrower.UpdateTargetPoint( position );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
