/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FFStudio;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class Test_Launch : MonoBehaviour
{
#region Fields
	public Vector3 throwVector;

	private Rigidbody dice_rigidbody;

	private Vector3 start_Position;
	private Quaternion start_Rotation;

	private UnityMessage fixedUpdateDelegate;
#endregion

#region Unity API
	private void Awake()
	{
		dice_rigidbody = GetComponent< Rigidbody >();

		dice_rigidbody.isKinematic = false;
		dice_rigidbody.useGravity = false;

		start_Position = transform.position;
		start_Rotation = transform.rotation;

		fixedUpdateDelegate = ExtensionMethods.EmptyMethod;
	}

	private void FixedUpdate() 
	{
		fixedUpdateDelegate();
	}
#endregion

#region API
#endregion

#region Implementation
	[ Button() ]
	private void Launch()
	{
		dice_rigidbody.useGravity = true;

		dice_rigidbody.velocity        = Vector3.zero;
		dice_rigidbody.angularVelocity = Vector3.zero;

		transform.position = start_Position;
		transform.rotation = start_Rotation;


		dice_rigidbody.AddForce( throwVector, ForceMode.Impulse );
		dice_rigidbody.AddTorque( Random.insideUnitSphere, ForceMode.Impulse );

		DOVirtual.DelayedCall( 1f, () => fixedUpdateDelegate = VelocityCheck );
		fixedUpdateDelegate = VelocityCheck;
	}

	private void VelocityCheck()
	{
		if( dice_rigidbody.IsSleeping() )
		{
			var dotProduct_up     = Vector3.Dot( transform.up, Vector3.up );
			var dotProduct_foward = Vector3.Dot( transform.forward, Vector3.up );
			var dotProduct_right  = Vector3.Dot( transform.right, Vector3.up );

			var facing_up     = Mathf.Approximately( 1, Mathf.Abs( dotProduct_up ) );
			var facing_foward = Mathf.Approximately( 1, Mathf.Abs( dotProduct_foward ) );
			var facing_right  = Mathf.Approximately( 1, Mathf.Abs( dotProduct_right ) );

			if( facing_up || facing_foward || facing_right )
			{
				FFLogger.Log( $"Facing up:{facing_up} - Facing foward:{facing_foward} - Facing right:{facing_right}" );
				fixedUpdateDelegate = ExtensionMethods.EmptyMethod;
			}
			else
			{
				FFLogger.Log( "Facing Angular" );
				dice_rigidbody.AddForce( Vector3.up * 3, ForceMode.Impulse );
			}
		}
	}
#endregion

#region EditorOnly
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.color = Color.red;
		var targetPosition = ( transform.position + throwVector.normalized * 3) ;
		Handles.DrawLine( transform.position, targetPosition );
	}

	[ Button() ]
	private void LogDotProduct()
	{
		var dotProduct_up     = Vector3.Dot( transform.up, Vector3.up );
		var dotProduct_foward = Vector3.Dot( transform.forward, Vector3.up );
		var dotProduct_right  = Vector3.Dot( transform.right, Vector3.up );

		FFLogger.Log( "Up: " + dotProduct_up );
		FFLogger.Log( "Forward: " + dotProduct_foward );
		FFLogger.Log( "Right: " + dotProduct_right );
}
#endif
#endregion
}
