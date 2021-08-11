/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;
using UnityEditor;

public class Soldier : MonoBehaviour
{
#region Fields
	[ Header( "Shared Variables" ) ]
	public SoldierPool soldierPool;


	[ HorizontalLine, Header( "Setup" ) ]
	[ SerializeField ] private SoldierData soldierData;


	// Private Fields \\
	private Animator animator;

	// Attack Releated
	private Soldier attackTarget;
	private Vector3 attackPoint;
	private int currentHealth;

	// Delegate
	private UnityMessage updateMethod;
#endregion

#region Unity API

	private void Awake()
	{
		animator = GetComponent< Animator >();

		updateMethod = ExtensionMethods.EmptyMethod;

		currentHealth = soldierData.health;
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
		attackTarget = target;
		attackPoint  = ( target.transform.position + transform.position ) / 2f;

		animator.SetBool( "running", true );

		updateMethod = AttackUpdate;
	}
#endregion

#region Implementation
	private void AttackUpdate()
	{
		// Move towards attack point
		transform.position = Vector3.MoveTowards( transform.position, attackPoint, Time.deltaTime * soldierData.speed_Running );

		// Look at towards attack point 
		transform.LookAtOverTimeAxis( attackPoint, Vector3.up, Time.deltaTime * soldierData.speed_Rotation );

		if( Vector3.Distance( transform.position, attackPoint ) <= soldierData.radius)
		{
			updateMethod = ExtensionMethods.EmptyMethod;

			animator.SetTrigger( "punch" );
			animator.SetBool( "running", false );

			// Attack
		}
	}

	private void ReturnToDefault()
	{
		currentHealth = soldierData.health;

		gameObject.SetActive( false );
		soldierPool.Stack.Push( this );
	}
#endregion

#region EditorOnly
#if UNITY_EDITOR	
	private void OnDrawGizmos()
	{
		Handles.DrawWireDisc( transform.position, transform.up, soldierData.radius );
	}
#endif
#endregion
}
