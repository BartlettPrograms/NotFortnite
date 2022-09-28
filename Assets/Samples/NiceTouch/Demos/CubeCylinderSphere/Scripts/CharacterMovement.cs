using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
namespace MoreMountains.NiceTouch
{	
	/// <summary>
	/// CharacterMovement is a very basic character class, used only to demonstrate how to bind the various controls to a character.
	/// </summary>
	public class CharacterMovement : MonoBehaviour 
	{
		/// The Speed of the character
	    public float Speed = 50f;
		/// the character's maximum speed
		public float MaximumSpeed = 20f;
		/// the force applied when jumping
		public float JumpForce = 100f;

	    protected Rigidbody _rigidbody;
	    protected float _horizontalMovement;
	    protected float _verticalMovement;
		protected bool _axisBased=false;

		/// <summary>
		/// On Start, we get the character's rigidbody
		/// </summary>
	    protected virtual void Start ()
	    {
	        _rigidbody = GetComponent<Rigidbody>();
	    }

	    /// <summary>
	    /// On Update, we move the character based on what's been input
	    /// </summary>
		protected virtual void Update ()
	    {
			Vector3 movement = new Vector3 (_horizontalMovement, 0.0f, _verticalMovement);
			_rigidbody.MovePosition(transform.position + movement * (Speed * Time.deltaTime));

	    }

	    /// <summary>
	    /// Sets the character's horizontal and vertical movement based on what's passed as a parameter
	    /// </summary>
	    /// <param name="newMovement">New movement.</param>
	    public virtual void Move(Vector2 newMovement)
	    {
	    	if (!_axisBased)
	    	{
				_horizontalMovement = newMovement.x;
				_verticalMovement = newMovement.y;	    
			}
	    }

	    /// <summary>
	    /// Sets the character's horizontal movement
	    /// </summary>
	    /// <param name="value">Value.</param>
	    public virtual void SetHorizontalAxis(float value)
	    {
			_axisBased=true;
	    	_horizontalMovement = value;
		}

		/// <summary>
		/// Sets the character's vertical movement
		/// </summary>
		/// <param name="value">Value.</param>
	    public virtual void SetVerticalAxis(float value)
		{
			_axisBased=true;
	    	_verticalMovement = value;
		}

		/// <summary>
		/// Makes the character jump
		/// </summary>
	    public virtual void Jump()
	    {
			_rigidbody.AddForce(Vector3.up * (JumpForce * -Physics.gravity.y));
	    }
	}
}