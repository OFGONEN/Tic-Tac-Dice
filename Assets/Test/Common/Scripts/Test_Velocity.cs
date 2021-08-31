/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;
using DG.Tweening;
using UnityEditor;

public class Test_Velocity : MonoBehaviour
{
#region Fields
	public Vector3 launchVector;
	public Transform target;

	public float travelTime;

	[ HorizontalLine ]
	public LineRenderer lineRenderer;

	// Private Fields
	private Rigidbody dice_Rigidbody;
	private float elapsedFixedTime;

	private Vector3 start_Position;
	private Quaternion start_Rotation;

	// Delegates
	private UnityMessage fixedUpdateMethod;	

	// Debug
	[SerializeField, ReadOnly] private float expectedTime;
#endregion

#region Unity API

	private void Awake()
	{
		dice_Rigidbody = GetComponent< Rigidbody >();

		dice_Rigidbody.isKinematic = false;
		dice_Rigidbody.useGravity  = false;

		fixedUpdateMethod = ExtensionMethods.EmptyMethod;

		start_Position = transform.position;
		start_Rotation = transform.rotation;

		lineRenderer.useWorldSpace = true;
	}

	private void FixedUpdate() 
	{
		fixedUpdateMethod();
	}
#endregion

#region API
	[ Button() ]
	public void LaunchToTarget()
	{
		dice_Rigidbody.position = start_Position;
		dice_Rigidbody.rotation = start_Rotation;

		dice_Rigidbody.useGravity = true;

		launchVector   = Vector3.zero;
		launchVector.x = ( target.position.x - dice_Rigidbody.position.x ) / travelTime;
		launchVector.z = ( target.position.z - dice_Rigidbody.position.z ) / travelTime;

		launchVector.y = -Physics.gravity.y * travelTime / 2 ;

		dice_Rigidbody.AddForce( launchVector, ForceMode.Impulse );
		dice_Rigidbody.AddTorque( Random.insideUnitSphere * 2, ForceMode.Impulse );

		elapsedFixedTime  = 0;
		fixedUpdateMethod = OnFixedUpdate_LaunchToTarget;

		var lineCount = 20;
		var linePositions = new List< Vector3 >( lineCount );

		for( var i = 0; i < lineCount; i++ )
		{
			var step = travelTime * i / ( lineCount - 1 );

			Vector3 timePosition = new Vector3(
				launchVector.x * step,
				launchVector.y * step + Physics.gravity.y * step * step / 2f,
				launchVector.z * step
			);

			linePositions.Add( timePosition + start_Position );
		}

		lineRenderer.positionCount = lineCount;
		lineRenderer.SetPositions( linePositions.ToArray() );
	}

	[ Button() ]
	public void FreeFall()
	{
		var height = 10f;

		var position    = start_Position;
		    position.y += height;

		dice_Rigidbody.position = position;
		dice_Rigidbody.useGravity = true;

		elapsedFixedTime = 0;

		expectedTime = Mathf.Sqrt( Mathf.Abs(2f * height / Physics.gravity.y) );
		FFLogger.Log( "Supposed time: " + expectedTime );

		fixedUpdateMethod = OnFixedUpdate_FreeFall;
	}
#endregion

#region Implementation

	private void OnFixedUpdate_LaunchToTarget()
	{
		elapsedFixedTime += Time.fixedDeltaTime;

		if( Vector3.Distance( dice_Rigidbody.position, target.position ) <= 0.2f )
		{
			dice_Rigidbody.velocity   = Vector3.zero;
			dice_Rigidbody.useGravity = false;

			fixedUpdateMethod = ExtensionMethods.EmptyMethod;

			FFLogger.Log( "Elapsed Fixed Time: " + elapsedFixedTime );
		}

	}

	private void OnFixedUpdate_FreeFall()
	{
		elapsedFixedTime += Time.fixedDeltaTime;

		if( dice_Rigidbody.position.y - start_Position.y <= 0.1f )
		{
			dice_Rigidbody.velocity = Vector3.zero;
			dice_Rigidbody.position = start_Position;
			dice_Rigidbody.useGravity = false;

			fixedUpdateMethod = ExtensionMethods.EmptyMethod;

			FFLogger.Log( "Elapsed Fixed Time: " + elapsedFixedTime );
		}
	}
#endregion

#region EditorOnly
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.DrawWireCube( target.position, Vector3.one / 2 );

		if( dice_Rigidbody != null )
		{
			Handles.DrawLine( dice_Rigidbody.position, target.position );
			Handles.Label( dice_Rigidbody.position + Vector3.up, "Distance: " + Vector3.Distance( dice_Rigidbody.position, target.position ) );
		}
	}
#endif
#endregion
}
