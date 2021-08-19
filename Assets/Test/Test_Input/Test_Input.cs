using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Test_Input : MonoBehaviour
{
    [ BoxGroup( "Setup" ) ] public SharedBoolProperty inputActiveProperty;
    [ BoxGroup( "Setup" ) ] public SharedVector2Property inputDirection;

	private UnityMessage inputChangedMethod;

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
	}

    private void InputActiveResponse()
    {
        FFLogger.Log( "Input Changed: " + inputActiveProperty.sharedValue );

        if( inputActiveProperty.sharedValue )
			inputChangedMethod = ApplyInputDirectionChange;
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

    }
}
