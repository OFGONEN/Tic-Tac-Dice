/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
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
#endregion

#region Unity API
	private void Awake()
	{
		dice_rigidbody = GetComponent< Rigidbody >();

		dice_rigidbody.isKinematic = false;
		dice_rigidbody.useGravity = false;

		start_Position = transform.position;
		start_Rotation = transform.rotation;
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
#endif
#endregion
}
