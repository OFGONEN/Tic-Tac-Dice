using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Test_Input : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ) ] public SharedBoolProperty inputActiveProperty;
    [ BoxGroup( "Setup" ) ] public SharedVector2Property inputDirection;

    [ BoxGroup( "Input Direction Apply Test" ) ] public Transform target;
    [ BoxGroup( "Input Direction Apply Test" ) ] public float target_MoveSpeed;

    // Private Fields
	private UnityMessage inputChangedMethod;
    private Vector3 target_OriginPosition;
#endregion

    private void OnEnable()
    {
		inputActiveProperty.changeEvet += InputActiveResponse;
		inputDirection.changeEvent += InputDirectionChangedResponse;
	}

    private void OnDisable()
    {
		inputActiveProperty.changeEvet -= InputActiveResponse;
		inputDirection.changeEvent -= InputDirectionChangedResponse;
    }

	private void Awake()
    {
		inputChangedMethod = ExtensionMethods.EmptyMethod;

		target_OriginPosition = target.position;
	}

    private void InputActiveResponse()
    {
        FFLogger.Log( "Input Changed: " + inputActiveProperty.sharedValue );

        if( inputActiveProperty.sharedValue )
        {
			inputChangedMethod = ApplyInputDirectionChange;
			target.position = target_OriginPosition;
		}
        else
			inputChangedMethod = ExtensionMethods.EmptyMethod;
	}

    private void InputDirectionChangedResponse()
    {
        FFLogger.Log( "Input Direction: " + inputDirection.sharedValue );
		inputChangedMethod();
	}

    private void ApplyInputDirectionChange()
    {
		var direction = new Vector3( inputDirection.sharedValue.x, 0, inputDirection.sharedValue.y );

		target.position = target.position + direction * Time.deltaTime * target_MoveSpeed;
	}
}
