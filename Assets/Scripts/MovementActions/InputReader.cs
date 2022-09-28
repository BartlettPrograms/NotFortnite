using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


namespace PhysicsBasedCharacterController
{
    public class InputReader : MonoBehaviour
    {
        [Header("Input specs")]
        public UnityEvent changedInputToMouseAndKeyboard;
        public UnityEvent changedInputToGamepad;

        [Header("Enable inputs")]
        public bool enableJump = true;
        public bool enableCrouch = true;
        public bool enableSprint = true;


        private MovementActions movementActions;

        [HideInInspector]
        public Vector2 axisInput;
        [HideInInspector]
        public Vector2 cameraInput;
        [HideInInspector]
        public Vector2 mouseScroll;
        [HideInInspector]
        public bool jump;
        [HideInInspector]
        public bool jumpHold;
        [HideInInspector]
        public float zoom;
        [HideInInspector]
        public bool sprint;
        [HideInInspector]
        public bool crouch;
        [HideInInspector]
        public bool attack;
        [HideInInspector]
        public bool aimIn;
        [HideInInspector]
        public bool targetLock;


        private bool hasJumped = false;
        private bool skippedFrame = false;
        private bool isMouseAndKeyboard = true;
        private bool oldInput = true;

        public delegate void StartTouchEvent(Vector2 position, float time);
        public event StartTouchEvent OnStartTouch;
        public delegate void EndTouchEvent(Vector2 position, float time);
        public event EndTouchEvent OnEndTouch;
        /**/


        private void Awake()
        {
            movementActions = new MovementActions();

            movementActions.Gameplay.Movement.performed += ctx => OnMove(ctx);
        
            movementActions.Gameplay.Camera.performed += ctx => OnCamera(ctx);
            
            movementActions.Gameplay.Scroll.performed += ctx => OnScroll(ctx);

            movementActions.Gameplay.Jump.performed += ctx => OnJump();
            movementActions.Gameplay.Jump.canceled += ctx => JumpEnded();

            movementActions.Gameplay.Sprint.performed += ctx => OnSprint(ctx);
            movementActions.Gameplay.Sprint.canceled += ctx => SprintEnded(ctx);

            movementActions.Gameplay.Crounch.performed += ctx => OnCrouch(ctx);
            movementActions.Gameplay.Crounch.canceled += ctx => CrouchEnded(ctx);
            
            movementActions.Gameplay.Attack.performed += ctx => OnAttack(ctx);
            movementActions.Gameplay.Attack.canceled += ctx => AttackEnded(ctx);

            movementActions.Gameplay.AimIn.performed += ctx => AimIn(ctx);
            movementActions.Gameplay.AimIn.canceled += ctx => AimCancel(ctx);
            
            movementActions.Gameplay.TargetLock.performed += ctx => TargetLock(ctx);
            movementActions.Gameplay.TargetLock.canceled += ctx => TargetLockReleased(ctx);
            
            movementActions.Gameplay.Scroll.performed += ctx => TargetLock(ctx);
            movementActions.Gameplay.TargetLock.canceled += ctx => TargetLockReleased(ctx);
            
            movementActions.Touch.TouchPress.started += ctx => StartTouch(ctx);
            movementActions.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
        }


        //old input system
        private void Update()
        {
            /*
            axisInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized;
            
            if (enableJump)
            {
                if (Input.GetButtonDown("Jump")) OnJump();
                if(jumpHold = Input.GetButtonUp("Jump"))JumpEnded();
            }
            
            if (enableSprint) sprint = Input.GetKeyDown(KeyCode.LeftShift);
            if (enableCrouch) crouch = Input.GetKeyDown(KeyCode.LeftControl);

            GetDeviceOld();

            //to use the old input system on the camera remove the component "Cinemachine Input Provider" and tune the input in the cinemachine components

            */
        }


        private void GetDeviceNew(InputAction.CallbackContext ctx)
        {
            //get device input
            oldInput = isMouseAndKeyboard;

            if (ctx.control.device is Keyboard || ctx.control.device is Mouse) isMouseAndKeyboard = true;
            else isMouseAndKeyboard = false;

            if (oldInput != isMouseAndKeyboard && isMouseAndKeyboard) changedInputToMouseAndKeyboard.Invoke();
            else if (oldInput != isMouseAndKeyboard && !isMouseAndKeyboard) changedInputToGamepad.Invoke();
        }


        private void GetDeviceOld()
        {
            /*

            //get device input
            oldInput = isMouseAndKeyboard;

            if (Input.GetJoystickNames().Length > 0) isMouseAndKeyboard = false;
            else isMouseAndKeyboard = true;

            if (oldInput != isMouseAndKeyboard && isMouseAndKeyboard) changedInputToMouseAndKeyboard.Invoke();
            else if (oldInput != isMouseAndKeyboard && !isMouseAndKeyboard) changedInputToGamepad.Invoke();wda

            */
        }
        
        


        #region Actions
        
        /*
         * Added by Matt Bart 8/22
         */

        public void AimIn(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) aimIn = true;
        }

        public void AimCancel(InputAction.CallbackContext ctx)
        {
            aimIn = false;
        }

        public void OnAttack(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) attack = true;
        }

        public void AttackEnded(InputAction.CallbackContext ctx)
        {
            attack = false;
        }

        public void TargetLock(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) targetLock = true;
        }

        public void TargetLockReleased(InputAction.CallbackContext ctx)
        {
            targetLock = false;
        }

        public void StartTouch(InputAction.CallbackContext ctx)
        {
            Debug.Log("Touch started " + movementActions.Touch.TouchPosition.ReadValue<Vector2>());
            if (OnStartTouch != null)
                OnStartTouch(movementActions.Touch.TouchPosition.ReadValue<Vector2>(), (float)ctx.startTime);
        }

        public void EndTouch(InputAction.CallbackContext ctx)
        {
            Debug.Log("Touch ended " + movementActions.Touch.TouchPosition.ReadValue<Vector2>());
            if (OnEndTouch != null)
                OnEndTouch(movementActions.Touch.TouchPosition.ReadValue<Vector2>(), (float)ctx.time);
        }
        
        /*
         * 
         */

        public void OnMove(InputAction.CallbackContext ctx)
        {
            axisInput = ctx.ReadValue<Vector2>();
            GetDeviceNew(ctx);
        }


        public void OnJump()
        {
            if(enableJump)
            {
                jump = true;
                jumpHold = true;

                hasJumped = true;
                skippedFrame = false;
            }
        }


        public void JumpEnded()
        {
            if(enableJump)
            {
                jump = false;
                jumpHold = false;
            }
        }


        private void FixedUpdate()
        {
            if (hasJumped && skippedFrame)
            {
                jump = false;
                hasJumped = false;
            }
            if (!skippedFrame) skippedFrame = true;
        }


        public void OnCamera(InputAction.CallbackContext ctx)
        {
            cameraInput = ctx.ReadValue<Vector2>();
            GetDeviceNew(ctx);
        }
        
        
        public void OnScroll(InputAction.CallbackContext ctx)
        {
            mouseScroll = ctx.ReadValue<Vector2>();
            GetDeviceNew(ctx);
        }


        public void OnSprint(InputAction.CallbackContext ctx)
        {
            if (enableSprint) sprint = true;
        }


        public void SprintEnded(InputAction.CallbackContext ctx)
        {
            if (enableSprint) sprint = false;
        }


        public void OnCrouch(InputAction.CallbackContext ctx)
        {
            if (enableCrouch) crouch = true;
        }


        public void CrouchEnded(InputAction.CallbackContext ctx)
        {
            if (enableCrouch) crouch = false;
        }

        #endregion


        #region Enable / Disable

        private void OnEnable()
        {
            movementActions.Enable();
        }


        private void OnDisable()
        {
            movementActions.Disable();
        }

        #endregion
    }
}