/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FFStudio;
using NaughtyAttributes;
using DG.Tweening;

public class Test_DiceSequence : MonoBehaviour
{
#region Fields
	public Vector3 throwVector;
	
	// Private Fields 
	private Rigidbody cube_Rigidbody;
	private BoxCollider cube_BoxCollider;
	[ SerializeField ] private List< Vector3 > collisionPoints = new List< Vector3 >(10);
	[ SerializeField ] private List< Vector3 > cubePositions = new List< Vector3 >(10);

	private Vector3 cube_StartPosition;
	private Quaternion cube_StartRotation;

	private int sequenceCount;

	// Delegates
	private CollisionEnter onCollisionEnterMethod = ExtensionMethods.EmptyMethod;
#endregion

#region Unity API
	private void Awake()
	{
		cube_Rigidbody   = GetComponent< Rigidbody >();
		cube_BoxCollider = GetComponent< BoxCollider >();

		cube_Rigidbody.isKinematic = false;
		cube_Rigidbody.useGravity  = false;
		cube_BoxCollider.enabled   = false;

		cube_StartPosition = transform.position;
		cube_StartRotation = transform.rotation;
	}

	private void OnCollisionEnter(Collision other) 
	{
		onCollisionEnterMethod( other );
	}
#endregion

#region API
#endregion

#region Implementation
	[ Button() ]
	private void LaunchSequence()
	{
		cube_Rigidbody.useGravity = true;
		cube_BoxCollider.enabled  = true;

		cube_Rigidbody.AddForce( throwVector, ForceMode.Impulse );
		cube_Rigidbody.AddTorque( Random.insideUnitSphere, ForceMode.Impulse );

		onCollisionEnterMethod = ExtensionMethods.EmptyMethod;

		var sequence = DOTween.Sequence();

		sequence.AppendInterval( 0.5f );

		sequence.AppendCallback( () => 
		{
			onCollisionEnterMethod = OnCollisionEnterAfterLaunch;
		} );

	}

	private void OnCollisionEnterAfterLaunch( Collision other )
	{
		FFLogger.Log( "Collision: " + other.contacts[ 0 ].point );
		collisionPoints.Add( other.contacts[ 0 ].point );
		cubePositions.Add( transform.position );

		cube_Rigidbody.velocity        = Vector3.zero;
		cube_Rigidbody.angularVelocity = Vector3.zero;

		cube_Rigidbody.position = cube_StartPosition;
		cube_Rigidbody.rotation = cube_StartRotation;

		sequenceCount++;

		if( sequenceCount < 10  )
		{
			DOVirtual.DelayedCall( 0.5f, LaunchSequence );
		}
		else 
		{
			Vector3 point_Collision    = Vector3.zero;
			Vector3 point_CubePosition = Vector3.zero;

			for( var i = 0; i < collisionPoints.Count; i++ )
			{
				point_Collision    += collisionPoints[ i ];
				point_CubePosition += cubePositions[ i ];
			}

			point_Collision    /= collisionPoints.Count;
			point_CubePosition /= cubePositions.Count;

			FFLogger.Log( $"Test Result \nAverage Collision Point:{point_Collision}\nAverage Cube Point:{point_CubePosition}" );
		}
	}


#endregion

#region EditorOnly
#if UNITY_EDITOR

	GUIStyle style = new GUIStyle();

	string[] diceNumbers = new string[ 6 ];
	[ SerializeField, ReadOnly ] float[] dotProducts = new float[ 6 ];

	private void OnDrawGizmosSelected()
	{
		// Handles.color = Color.red;
		// var targetPosition = ( transform.position + throwVector.normalized * 3) ;
		// Handles.DrawLine( transform.position, targetPosition );

		// Up: 1, Right: 3, Forward: 5
		style.fontSize = 25;

		UpdateDotProducts();

		Handles.Label( transform.position + transform.up, diceNumbers[ 0 ], style );
		Handles.Label( transform.position + -transform.up, diceNumbers[ 1 ], style );
		Handles.Label( transform.position + transform.right, diceNumbers[ 2 ], style );
		Handles.Label( transform.position + -transform.right, diceNumbers[ 3 ], style );
		Handles.Label( transform.position + transform.forward, diceNumbers[ 4 ], style );
		Handles.Label( transform.position + -transform.forward, diceNumbers[ 5 ], style );
	}

	private void UpdateDotProducts()
	{
		dotProducts[ 0 ] = Vector3.Dot( transform.up, Vector3.up ); 
		dotProducts[ 1 ] = Vector3.Dot( -transform.up, Vector3.up ); 
		dotProducts[ 2 ] = Vector3.Dot( transform.right, Vector3.up );  
		dotProducts[ 3 ] = Vector3.Dot( -transform.right, Vector3.up ); 
		dotProducts[ 4 ] = Vector3.Dot( transform.forward, Vector3.up ); 
		dotProducts[ 5 ] = Vector3.Dot( -transform.forward, Vector3.up ); 

		int selected = 0;
		float max = dotProducts[ 0 ];

		for( var i = 0; i < diceNumbers.Length - 1; i++ )
		{
			if( dotProducts[ i + 1 ] >= max )
			{
				max = dotProducts[ i + 1 ];
				selected = i + 1;
			}
		}

		// Up: 1, Right: 3, Forward: 5

		diceNumbers[ 0 ] = "1";
		diceNumbers[ 1 ] = "6";
		diceNumbers[ 2 ] = "3";
		diceNumbers[ 3 ] = "4";
		diceNumbers[ 4 ] = "5";
		diceNumbers[ 5 ] = "2";

		diceNumbers[ selected ] = diceNumbers[ selected ] + "!";
	}
#endif
#endregion
}
