/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class Soldier : MonoBehaviour
{
#region Fields
	[ Header( "Shared Variables" ) ]
	public SoldierPool soldierPool;


	// Private Fields \\
	private Animator animator;

	// Delegate
	private UnityMessage updateMethod;
#endregion

#region Unity API

	private void Awake()
	{
		animator = GetComponent< Animator >();

		updateMethod = ExtensionMethods.EmptyMethod;
	}

	private void Update()
	{
		updateMethod();
	}
#endregion

#region API
	// Spawns the soldier with given world position and rotation.
	public void Spawn( Vector3 position, Quaternion rotation )
	{
		gameObject.SetActive( true );

		transform.position = position;
		transform.rotation = rotation;
	}

	public void Attack( Soldier target )
	{
		
	}
#endregion

#region Implementation
	private void ReturnToDefault()
	{
		gameObject.SetActive( false );
		soldierPool.Stack.Push( this );
	}
#endregion
}
