using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	public class MMControlsMovementDemoPlayer : MonoBehaviour
	{
		public enum CharacterStates { Idle, Walking, Running }
		
		[Header("Bindings")]
		public Animator TargetAnimator;

		public Transform TargetRotatingModel;

		public ParticleSystem WalkParticles;
		[Header("Movement")]
		public float MovementSpeed = 8f;
		public float RotationSpeed = 20f;

		[Header("Debug")]
		[MMReadOnly]
		public Vector2 _input;
		[MMReadOnly] 
		public CharacterStates CharacterState;
		
		protected Rigidbody _rigidbody;
		protected Vector3 _newMovement;
		private static readonly int _idleAnimationParameter = Animator.StringToHash("Idle");
		private static readonly int _walkingAnimationParameter = Animator.StringToHash("Walking");
		private static readonly int _runningAnimationParameter = Animator.StringToHash("Running");

		protected const float _idleThreshold = 0.1f;
		protected const float _runThreshold = 0.6f;
		protected ParticleSystem.EmissionModule _emissionModule;

		protected virtual void Awake()
		{
			_rigidbody = this.gameObject.GetComponent<Rigidbody>();
			_emissionModule = WalkParticles.emission;
		}
		
		public virtual void SetRawInput(Vector2 newInput)
		{
			_input = newInput;
		}

		public virtual void SetHorizontalInput(float newValue)
		{
			_input.x = newValue;
		}

		public virtual void SetVerticalInput(float newValue)
		{
			_input.y = newValue;
		}

		protected virtual void Update()
		{
			HandleStates();
			UpdateAnimator();
			RotateModel();
			HandleParticles();
		}

		protected virtual void FixedUpdate()
		{
			MoveRigidbody();
		}

		protected virtual void MoveRigidbody()
		{
			_newMovement.x = _input.x;
			_newMovement.y = 0f;
			_newMovement.z = _input.y;
			_newMovement = _newMovement * MovementSpeed;
			_rigidbody.MovePosition(_rigidbody.position + _newMovement * Time.fixedDeltaTime);
		}

		protected Quaternion _tmpRotation;
		
		protected virtual void RotateModel()
		{
			if (_newMovement == Vector3.zero)
			{
				return;
			}
			_tmpRotation = Quaternion.LookRotation(_newMovement);
			TargetRotatingModel.transform.rotation = Quaternion.Slerp(TargetRotatingModel.transform.rotation, _tmpRotation, Time.deltaTime * RotationSpeed);
		}

		protected virtual void HandleStates()
		{
			float movementMagnitude = _newMovement.magnitude;
			float remappedMovement = MMMaths.Remap(movementMagnitude, 0f, MovementSpeed, 0f, 1f);
			
			if (remappedMovement < _idleThreshold)
			{
				CharacterState = CharacterStates.Idle;
			}
			else if (remappedMovement > _runThreshold)
			{
				CharacterState = CharacterStates.Running;
			}
			else
			{
				CharacterState = CharacterStates.Walking;
			}
		}

		protected virtual void HandleParticles()
		{
			_emissionModule.enabled = (CharacterState != CharacterStates.Idle);
		}

		protected virtual void UpdateAnimator()
		{
			TargetAnimator.SetBool(_idleAnimationParameter, (CharacterState == CharacterStates.Idle));
			TargetAnimator.SetBool(_walkingAnimationParameter, (CharacterState == CharacterStates.Walking));
			TargetAnimator.SetBool(_runningAnimationParameter, (CharacterState == CharacterStates.Running));
		}
	}	
}

