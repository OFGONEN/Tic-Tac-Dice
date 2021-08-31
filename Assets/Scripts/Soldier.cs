/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;
using UnityEditor;

public class Soldier : MonoBehaviour
{
#region Fields
	[ Header( "Event Listeners" ) ]
	public MultipleEventListenerDelegateResponse newLevelLoadListeners;

	[ Header( "Shared Variables" ) ]
	public SoldierPool soldierPool;

	[ Header( "Fired Events" ) ]
	public ParticleSpawnEvent soldierSpawnParticleEvent;

	[ HorizontalLine, Header( "Setup" ) ]
	[ SerializeField ] private SoldierData soldierData;

	// Public Properties \\
	public SoldierData SoldierData => soldierData;
	public bool IsSoldierDead => !alive;
	public Soldier AttackTarget => attackTarget;

	// Private Fields \\
	private Animator animator;

	// Attack Releated
	private Soldier attackTarget;
	[ SerializeField, ReadOnly ] private int currentHealth;
	private bool alive = false;
	private bool canAttack = true;
	private Tween attackCooldownTween;

	// Attack Lists
	[ SerializeField, ReadOnly ] private List< Soldier > allySoldiersList;
	[ SerializeField, ReadOnly ] private List< Soldier > allyTypeSoldiersList;
	[ SerializeField, ReadOnly ] private List< Soldier > enemySoldiersList;

	// Move Related
	private Vector3 movePosition;

	// Delegate
	private UnityMessage updateMethod;
#endregion

#region Unity API

	private void Awake()
	{
		animator = GetComponent< Animator >();

		updateMethod = ExtensionMethods.EmptyMethod;

		currentHealth = soldierData.health;

		newLevelLoadListeners.response = ReturnToDefault;
		newLevelLoadListeners.OnEnable();
	}

	private void OnDisable()
	{
		KillTweens();
	}

	private void OnDestroy() 
	{
		newLevelLoadListeners.OnDisable();
	}

	private void Update()
	{
		updateMethod();
	}

	private void OnTriggerEnter( Collider other )
	{
		if( soldierData.canDiceKill )
			Die();
	}
#endregion

#region API
	// Spawns the soldier with given world position and rotation.
	public void Spawn( Vector3 position, Quaternion rotation, List< Soldier > allySoldiers, List< Soldier > allyTypeSoldiers, List< Soldier > enemySoldiers )
	{
		// Set default values
		currentHealth = soldierData.health;
		alive         = true;
		canAttack     = true;

		allySoldiersList     = allySoldiers;
		allyTypeSoldiersList = allyTypeSoldiers;
		enemySoldiersList    = enemySoldiers;

		updateMethod = ExtensionMethods.EmptyMethod;

		gameObject.SetActive( true );

		transform.position = position;
		transform.rotation = rotation;

		position.y -= 1f;
		// Soldier spawn Particle Effect 
		soldierSpawnParticleEvent.changePosition = true;
		soldierSpawnParticleEvent.spawnPoint     = position;
		soldierSpawnParticleEvent.particleAlias  = "soldier_spawn";
		soldierSpawnParticleEvent.Raise();
	}

	// Attack a target soldier
	public void Attack( Soldier target )
	{
		attackTarget = target; // Cache the attack target

		animator.SetBool( "running", true );

		updateMethod = OnAttackUpdate; // Move towards attack target every frame, attack it if possible.
	}

	// Attack the closest enemy soldier
	public void AttackClosest()
	{
		// If there is no soldier to attack, return.
		if( enemySoldiersList == null || enemySoldiersList.Count <= 0 )
			return;

		// Find the closest Enemy.
		Soldier closestEnemy = enemySoldiersList[ 0 ];
		float closestDistance = Vector3.Distance( transform.position, enemySoldiersList[ 0 ].transform.position );

		for( var i = 1; i < enemySoldiersList.Count; i++ )
		{
			var enemy = enemySoldiersList[ i ];
			var distance = Vector3.Distance( transform.position, enemy.transform.position );

			if( distance <= closestDistance && !enemy.IsSoldierDead )
			{
				closestDistance = distance;
				closestEnemy = enemy;
			}
		}

		// Attack the closest enemy.
		Attack( closestEnemy );
	}

	// Take damage from attacking soldier. If our attack is same soldier as the attacker , attack as counter attack
	public void TakeDamage( Soldier attacker, bool counterAttack ) // Counter attack bool is there for preventing this method called indefinitly as recursive
	{
		currentHealth--;

		if( currentHealth <= 0 )
		{
			// If attacker is the same soldier we want to attack, we should attack at the same frame as counter attack.
			if( canAttack && attackTarget != null && attackTarget == attacker && !counterAttack ) 
				attackTarget.TakeDamage( this, true ); // Attack as counter attack

			Die();
		}
	}

	public void Die()
	{
		if( !alive ) return;

		alive     = false;
		canAttack = false;

		updateMethod = ExtensionMethods.EmptyMethod;

		allySoldiersList    .Remove( this );
		allyTypeSoldiersList.Remove( this );

		allySoldiersList     = null;
		allyTypeSoldiersList = null;
		enemySoldiersList    = null;

		ReturnToDefault();
	}

	public void Move( Vector3 position )
	{
		movePosition = position;

		animator.SetBool( "running", true );

		updateMethod = OnMoveUpdate;
	}
#endregion

#region Implementation
	private void OnMoveUpdate()
	{
		// Move towards attack target.
		transform.position = Vector3.MoveTowards( transform.position, movePosition, Time.deltaTime * soldierData.speed_Running );

		// Look at towards attack target. 
		transform.LookAtOverTimeAxis( movePosition, Vector3.up, Time.deltaTime * soldierData.speed_Rotation );

		if( Vector3.Distance( transform.position, movePosition ) <= 0.1f )
		{
			updateMethod = ExtensionMethods.EmptyMethod;
			animator.SetBool( "running", false );
		}

	}

	private void OnAttackUpdate()
	{
		var targetPosition = attackTarget.transform.position;

		// Target soldier died before we attacked it.
		if( attackTarget.IsSoldierDead )
		{
			updateMethod = ExtensionMethods.EmptyMethod;

			animator.SetBool( "running", false );

			AttackClosest();
		}
		// Target is in attack range, attack it.
		else if( canAttack && Vector3.Distance( transform.position, targetPosition ) <= ( soldierData.radius + attackTarget.SoldierData.radius ) )
		{
			updateMethod = ExtensionMethods.EmptyMethod;

			animator.SetTrigger( "punch" );
			animator.SetBool( "running", false );

			// Look at the target before attacking
			transform.LookAtAxis( targetPosition, Vector3.up );

			// Attack
			DOVirtual.DelayedCall( GameSettings.Instance.soldier_AttackDelay, Attack );

		}
		else // Move and rotates towards attack target.
		{
			// Move towards attack target.
			transform.position = Vector3.MoveTowards( transform.position, targetPosition, Time.deltaTime * soldierData.speed_Running );

			// Look at towards attack target. 
			transform.LookAtOverTimeAxis( targetPosition, Vector3.up, Time.deltaTime * soldierData.speed_Rotation );
		}
	}

	private void Attack()
	{
		attackTarget.TakeDamage( this, false );

		canAttack = false;

		if( alive )
		{
			if( attackTarget.IsSoldierDead )
				attackCooldownTween = DOVirtual.DelayedCall( soldierData.attackCooldown, OnAttackCoooldownFinish );
			else
				attackCooldownTween = DOVirtual.DelayedCall( soldierData.attackCooldown, OnAttackRepeat );
		}
	}

	private void OnAttackCoooldownFinish()
	{
		attackCooldownTween = null;
		canAttack = true;

		AttackClosest();
	}
	
	private void OnAttackRepeat()
	{
		canAttack = true;
		OnAttackUpdate();
	}

	private void ReturnToDefault()
	{
		gameObject.SetActive( false );
		soldierPool.Stack.Push( this );
	}

	private void KillTweens()
	{
		if( attackCooldownTween != null )
		{
			attackCooldownTween.Kill();
			attackCooldownTween = null;
		}
	}
#endregion

#region EditorOnly
#if UNITY_EDITOR	
	private void OnDrawGizmos()
	{
		Handles.DrawWireDisc( transform.position, transform.up, soldierData.radius );

		Handles.Label( transform.position + Vector3.up * 3, "Health: " + currentHealth );
	}
#endif
#endregion
}
