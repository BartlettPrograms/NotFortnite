using System;
using System.Collections.Generic;
using System.Numerics;
using PhysicsBasedCharacterController;
using PlayerLockonCamTesting;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CompCameraController : MonoBehaviour
{
    public InputReader input;

    public const string MouseXString = "Mouse X";
    public const string MouseYString = "Mouse Y";
    public const string MouseScrollString = "MouseScrollWheel";
    public Vector2 mouseInput;

    [Header("Framing")]
    [SerializeField] private Camera _camera = null;
    [SerializeField] private Transform _followTransform = null;
    [SerializeField] private Vector3 _framingNormal = Vector3.zero;

    [Header("Distance")] 
    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _defaultDistance = 5f;
    [SerializeField] private float _minDistance = 0f;
    [SerializeField] private float _maxDistance = 10f;

    [Header("Rotation")] 
    [SerializeField] private bool _invertX = false;
    [SerializeField] private bool _invertY = false;
    [SerializeField] private float _rotationSharpness = 25f;
    [SerializeField] private float _defaultVerticalAngle = 20;
    [SerializeField] [Range(-90, 90)] private float _minVerticalAngle = -90;
    [SerializeField] [Range(-90, 90)] private float _maxVerticalAngle = 90;

    [Header("Obstruction")] 
    [SerializeField] private float _checkRadius = 0.2f;
    [SerializeField] private LayerMask _obstructionLayers = -1;
    private List<Collider> _ignoreColliders = new List<Collider>();

    [Header("Lock On")] 
    [SerializeField] private float _lockOnLossTime = 30;
    [SerializeField] private float _lockOnDistance = 20;
    [SerializeField] private LayerMask _lockOnLayers = -1;
    
    [SerializeField] private bool _lockedOn = true;
    [SerializeField] private float _lockOnLossTimeCurrent;
    [SerializeField] private ITargetable _target;
    [SerializeField] private Vector3 _lockOnFraming = Vector3.zero;
    [SerializeField, Range(1, 179)] private float _lockOnFOV = 40;

    [SerializeField] CamManager camManager;
    private bool _lockOnReady = true;

    public bool LockedOn { get => _lockedOn; }
    public ITargetable Target { get => _target; }
    //public Vector3 CameraPlanarDirection { get => _planarDirection; }

    
    // Private parts
    private float _fovNormal;
    private float _framingLerp;
    private Vector3 _planarDirection;
    private float _targetDistance;
    private Vector3 _targetPosition;
    private Quaternion _targetRotation;
    private float _targetVerticalAngle;

    private Vector3 _newPosition;
    private Quaternion _newRotation;

    private void OnValidate()
    {
        _defaultDistance = Mathf.Clamp(_defaultDistance, _minDistance, _maxDistance);
        _defaultVerticalAngle = Mathf.Clamp(_defaultVerticalAngle, _minVerticalAngle, _maxVerticalAngle);
    }

    private void Start()
    {
        // Ignore player colliders
        _ignoreColliders.AddRange(GetComponentsInChildren<Collider>());
        
        // Important
        _fovNormal = _camera.fieldOfView;
        _planarDirection = _followTransform.forward;
        
        // Calculate Targets
        _targetDistance = _defaultDistance;
        _targetVerticalAngle = _defaultVerticalAngle;
        _targetRotation = Quaternion.LookRotation(_planarDirection) * Quaternion.Euler(_targetVerticalAngle, 0, 0);
        _targetPosition = _followTransform.position - (_targetRotation * Vector3.forward) * _targetDistance; 
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        checkLockOn();
        
        if (Cursor.lockState != CursorLockMode.Locked)
            return;
        // Handle input
        float _zoom = input.mouseScroll.y * _zoomSpeed;
        mouseInput = input.cameraInput;
        if (_invertX) mouseInput.x *= -1f;
        if (_invertY)mouseInput.y *= -1f;
        _zoom *= -1f;

        Vector3 _framing = Vector3.Lerp(_framingNormal, _lockOnFraming, _framingLerp);
        Vector3 _focusPosition = _followTransform.position + _followTransform.TransformDirection(_framing);
        float _fov = Mathf.Lerp(_fovNormal, _lockOnFOV, _framingLerp);
        _camera.fieldOfView = _fov;

        // Lock on check
        if (_lockedOn && _target != null)
        {
            Vector3 _camToTarget = _target.TargetTransform.position - _camera.transform.position;
            Vector3 _planarCamToTarget = Vector3.ProjectOnPlane(_camToTarget, Vector3.up);
            Quaternion _lookRotation = Quaternion.LookRotation(_camToTarget, Vector3.up);

            _framingLerp = Mathf.Clamp01(_framingLerp + Time.deltaTime * 4);
            _planarDirection = _planarCamToTarget != Vector3.zero ? _planarCamToTarget.normalized : _planarDirection;
            _targetDistance = Mathf.Clamp(_targetDistance + _zoom, _minDistance, _maxDistance);
            _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle + mouseInput.y, _minVerticalAngle, _maxVerticalAngle);
        }
        else
        {
            _framingLerp = Mathf.Clamp01(_framingLerp - Time.deltaTime * 4);
            _planarDirection = Quaternion.Euler(0, mouseInput.x, 0) * _planarDirection;
            _targetDistance = Mathf.Clamp(_targetDistance + _zoom, _minDistance, _maxDistance);
            _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle + mouseInput.y, _minVerticalAngle, _maxVerticalAngle);
        }

        // Handle Obstructions (affects target distance)
        float _smallestDistance = _targetDistance;
        RaycastHit[] _hits = Physics.SphereCastAll(_focusPosition, _checkRadius, _targetRotation * -Vector3.forward,
            _targetDistance, _obstructionLayers);
        if (_hits.Length != 0)
            foreach (RaycastHit hit in _hits)
            if (!_ignoreColliders.Contains(hit.collider))
                if (hit.distance < _smallestDistance)
                    _smallestDistance = hit.distance;

        // Final Targets
        _targetRotation = Quaternion.LookRotation(_planarDirection) * Quaternion.Euler(_targetVerticalAngle, 0, 0);
        _targetPosition = _focusPosition - (_targetRotation * Vector3.forward) * _smallestDistance;
        
        // Handle Smoothing
        _newRotation = Quaternion.Slerp(_camera.transform.rotation, _targetRotation, Time.deltaTime * _rotationSharpness);
        _newPosition = Vector3.Lerp(_camera.transform.position, _targetPosition, Time.deltaTime * _rotationSharpness);
        
        // Apply
        _camera.transform.rotation = _newRotation;
        _camera.transform.position = _newPosition;

        if (_lockedOn && _target != null)
        {
            bool _valid =
                _target.Targetable &&
                InDistance(_target) &&
                InScreen(_target) &&
                NotBlocked(_target);
            if (_valid) { _lockOnLossTimeCurrent = 0; }
            else 
            { _lockOnLossTimeCurrent = Mathf.Clamp(_lockOnLossTimeCurrent + Time.deltaTime, 0, _lockOnLossTime); }
            if (_lockOnLossTimeCurrent == _lockOnLossTime)
            {
                _lockedOn = false;
                _target = null;
            }
        }
    }

    private void checkLockOn()
    {
        // Find a Lock On Target
        if (camManager.LockedOn && _lockOnReady)
        {
            _lockOnReady = false;
            
            // Filter Targetables
            List<ITargetable> _targetables = new List<ITargetable>();
            Collider[] _colliders = Physics.OverlapSphere(transform.position, _lockOnDistance, _lockOnLayers);
            foreach (Collider _collider in _colliders)
            {
                ITargetable _targetable = _collider.GetComponent<ITargetable>();
                if (_targetable != null)
                    if (_targetable.Targetable)
                        if (InScreen(_targetable))
                            if (NotBlocked(_targetable))
                            {
                                _targetables.Add(_targetable);
                            }
            }
            // Find closest targetable to screen center
            float _hypotenuse;
            float _smallestHypotenuse = Mathf.Infinity;
            ITargetable _closestTargetable = null;
            foreach (ITargetable _targetable in _targetables)
            {
                _hypotenuse = CalculateHypotenuse(_targetable.TargetTransform.position);
                if (_smallestHypotenuse > _hypotenuse)
                {
                    _closestTargetable = _targetable;
                    _smallestHypotenuse = _hypotenuse;
                }
            }
            
            // Final aim at select target
            _target = _closestTargetable;
            _lockedOn = _closestTargetable != null;
        }
        else if (!camManager.LockedOn)
        {
            _lockOnReady = true;
        }
    }
    

    private bool InDistance(ITargetable _targetable)
    {
        float _distance = Vector3.Distance(transform.position, _targetable.TargetTransform.position);
        return _distance <= _lockOnDistance;
    }
    
    private bool InScreen(ITargetable targetable)
    {
        Vector3 _viewPortPosition = _camera.WorldToViewportPoint(targetable.TargetTransform.position);

        if (!(_viewPortPosition.x > 0) || !(_viewPortPosition.x < 1)) { return false; }
        if (!(_viewPortPosition.y > 0) || !(_viewPortPosition.y < 1)) { return false; }
        if (!(_viewPortPosition.z > 0)) { return false; }

        return true;
    }

    private bool NotBlocked(ITargetable targetable)
    {
        Vector3 _origin = _camera.transform.position;
        Vector3 _direction = targetable.TargetTransform.position - _origin;

        float _radius = 0.15f;
        float _distance = _direction.magnitude;
        bool _notBlocked = !Physics.SphereCast(_origin, _radius, _direction, out RaycastHit hit, _distance, _obstructionLayers);

        return _notBlocked;
    }

    private float CalculateHypotenuse(Vector3 position)
    {
        float _screenCenterX = _camera.pixelWidth / 2;
        float _screenCenterY = _camera.pixelHeight / 2;

        Vector3 _screenPosition = _camera.WorldToScreenPoint(position);
        float _xDelta = _screenCenterX - _screenPosition.x;
        float _yDelta = _screenCenterY - _screenCenterY;
        float _hypotenuse = Mathf.Sqrt(Mathf.Pow(_xDelta, 2) + Mathf.Pow(_yDelta, 2));

        return _hypotenuse;
    }

    public bool CheckEnemyTargets()
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, _lockOnDistance, _lockOnLayers);
        foreach (Collider _collider in _colliders)
        {
            Debug.Log(_collider);
            ITargetable _targetable = _collider.GetComponent<ITargetable>();
            if (_targetable != null)
                if (_targetable.Targetable)
                    if (InScreen(_targetable))
                        if (NotBlocked(_targetable))
                            return true;
            
        }
        return false;
    }

    public void RemoveTarget()
    {
        _target = null;
    }
}
