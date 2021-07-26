/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FFStudio;
using NaughtyAttributes;

public class Test_Dice : MonoBehaviour
{
#region Fields
	public Vector3 cube_ThrowVector;
	
	// Private Fields 
	private Rigidbody cube_Rigidbody;
#endregion

#region Unity API
	private void Awake()
	{
		cube_Rigidbody = GetComponent< Rigidbody >();
	}

	private void OnCollisionEnter(Collision other) 
	{
		FFLogger.Log( "Collision: " + other.contacts[ 0 ].point );
	}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region EditorOnly
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.color = Color.red;
		var targetPosition = ( transform.position + cube_ThrowVector.normalized * 3) ;
		Handles.DrawLine( transform.position, targetPosition );
	}
#endif
#endregion
}
