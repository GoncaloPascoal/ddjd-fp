using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
	public class ThirdPersonController : MonoBehaviour
	{
		[Header("Player_For_Others")] [Tooltip("Player head transform")]
		public Transform playerHeadTransform;
		
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 2.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 5.335f;
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.50f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;
		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;
		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;

		[Header("Player Attacks")] 
		[Tooltip("Minimum dot product between player and enemy looking directions for backstab")]
		public float backstabAngleOffset = 0.95f;

		private Bar _staminaBar;

		private Damageable _damageable;
		private Attacker _attacker;
		
		// cinemachine
		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _animationBlend;
		private float _targetRotation;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		private float _stamina;
		private float Stamina
		{
			get => _stamina;
			set
			{
				_stamina = Mathf.Clamp(value, 0f, _stats.GetStatValue(StatName.Stamina));
				_staminaBar.SetValue(_stamina);
			}
		}

		private const float StaminaUsageSprint = -15f;
		private const float StaminaUsageJump = -20f;
		private const float StaminaUsageRoll = -20f;
		public readonly Dictionary<string, float> StaminaUsageAttacks = new Dictionary<string, float>
		{
			{"LightAttack", -22f},
			{"HeavyAttack", -32f},
		};
		private float _staminaNeededBeforeSprint;

		// timeout delta time
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// animation IDs
		private int _animIDSpeed;
		private int _animIDGrounded;
		private int _animIDJump;
		private int _animIDFreeFall;
		private int _animIDMotionSpeed;
		private int _animIDAttackNormal;

		private Animator _animator;
		private CharacterController _controller;
		private GameObject _mainCamera;
		private Staggerable _staggerable;

		private const float Threshold = 0.01f;

		private bool _hasAnimator;
		private bool _resurrecting;
		private Animator _enemyToResurrect;

		private Stats _stats;

		// Roll
		// TODO: change so that roll is only invulnerable in some frames
		[Header("Roll")]
		private bool _isRolling;

		// TODO: fix
		private bool IsCurrentDeviceMouse = true;

		private List<GameObject> _backstabTargets;
		private bool _isBackstabbing;
		private GameObject _currentTarget;

		private int _inCheckpoint = -1;

		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;

			_stats = GetComponent<Stats>();

			_hasAnimator = TryGetComponent(out _animator);
			_controller = GetComponent<CharacterController>();
			_staggerable = GetComponent<Staggerable>();
			_enemyToResurrect = null;

			_backstabTargets = new List<GameObject>();

			_damageable = GetComponent<Damageable>();
			_damageable.InitializeMaxHealth((int) _stats.GetStatValue(StatName.Health));

			_staminaBar = HUD.Instance.staminaBar;
			_stamina = _stats.GetStatValue(StatName.Stamina);
			_staminaBar.SetMaxValue(_stamina);
			_staminaBar.SetValueInstantly(_stamina);
			StartCoroutine(UpdateStaminaBar());

			_isBackstabbing = false;
			_resurrecting = false;

			_attacker = GetComponent<Attacker>();

			AssignAnimationIDs();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;

			var currentCheckpoint = GameData.CheckpointNumber;
			GameObject checkPoint = null;
			
			foreach (var check in GameObject.FindGameObjectsWithTag("Checkpoint"))
			{
				var checkpointScript = check.GetComponent<Checkpoint>();
				if (checkpointScript != null && checkpointScript.checkpointNumber == currentCheckpoint)
				{
					checkPoint = check;
					break;
				}
			}
			
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}

			GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>().Follow =
				CinemachineCameraTarget.transform;

			if (checkPoint != null)
			{
				var spawnPoint = checkPoint.transform.Find("PlayerSpawn").transform;

				_controller.enabled = false;
			
				transform.position = spawnPoint.position;
				transform.rotation = spawnPoint.rotation;
			
				_controller.enabled = true;

				if (GameData.InCheckpoint)
				{
					InCheckpoint(currentCheckpoint);
				}
			}
		}

		private IEnumerator UpdateStaminaBar()
		{
			while (true)
			{
				yield return new WaitForSeconds(0.25f);
				_staminaBar.SetMaxValue(_stats.GetStatValue(StatName.Stamina));
			}
		}

		private void Update()
		{
			_hasAnimator = TryGetComponent(out _animator);
			
			if (_inCheckpoint != -1) return;

			JumpAndGravity();
			GroundedCheck();
			GroundedOrFalling();
			Move();
			Attacks();
		}

		private void GroundedOrFalling()
		{
			// Possible Fix to An Infinite Loop That Happens When Both Bools Are Set To True
			if(_animator.GetBool(_animIDFreeFall) && _animator.GetBool(_animIDGrounded)) _animator.SetBool(_animIDFreeFall, false);
			if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle Walk Run Blend") && _attacker.IsAttacking())
			{
				_attacker.SetAttackingIfNotStarting(false);
				_animator.applyRootMotion = false;
			}

		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreeFall = Animator.StringToHash("FreeFall");
			_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
			_animIDAttackNormal = Animator.StringToHash("AttackNormal");
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetBool(_animIDGrounded, Grounded);
			}
		}

		private void CameraRotation()
		{
			Vector2 look = InputManager.Action("Look").ReadValue<Vector2>();
			
			// if there is an input and camera position is not fixed
			if (look.sqrMagnitude >= Threshold && !LockCameraPosition)
			{
				//Don't multiply mouse input by Time.deltaTime;
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetYaw += look.x * deltaTimeMultiplier;
				_cinemachineTargetPitch += look.y * deltaTimeMultiplier;
			}

			// clamp our rotations so our values are limited 360 degrees
			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Cinemachine will follow this target
			CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
		}

		private void Move()
		{
			if (_inCheckpoint != -1)
				return;

			Vector2 movement;

			bool isAttacking = _attacker.IsAttacking();
			bool isAttackingCanRotate = _attacker.IsStartingAttack();

			bool sprint = InputManager.Action("Sprint").IsPressed();

			if (isAttacking && !isAttackingCanRotate || _staggerable.IsStaggered() || isDead())
				movement = Vector2.zero;
			else
				movement = InputManager.Action("Move").ReadValue<Vector2>();

			// Can't move
			if (!isAttackingCanRotate && (isAttacking || _isBackstabbing || _resurrecting)) return;

			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = (sprint && _stamina > _staminaNeededBeforeSprint) ? SprintSpeed : MoveSpeed;


			if (Grounded && !_isRolling)
			{
				if (sprint && movement != Vector2.zero && _stamina > _staminaNeededBeforeSprint)
				{
					_staminaNeededBeforeSprint = 0;
					ChangeStamina(Time.deltaTime * StaminaUsageSprint);
					if (Stamina <= 0.0f) _staminaNeededBeforeSprint = 25f;
				}
				else
				{
					ChangeStamina(Time.deltaTime * _stats.GetStatValue(StatName.StaminaRecovery));
				}
			}
			
			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (movement == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			const float speedOffset = 0.1f;
			float inputMagnitude = movement.magnitude;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}
			_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

			// normalise input direction
			Vector3 direction = new Vector3(movement.x, 0.0f, movement.y);

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (!_isRolling)
			{
				if (movement != Vector2.zero)
				{
					_targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
					                  _mainCamera.transform.eulerAngles.y;
					float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
						ref _rotationVelocity, RotationSmoothTime);

					// rotate to face input direction relative to camera position
					transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
				}

				Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
				_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
				                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
			}
			else if (!_controller.isGrounded)
			{
				_controller.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
			}

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetFloat(_animIDSpeed, _animationBlend);
				_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
			}
			

			if (isAttackingCanRotate)
			{
				Vector3 dir = new Vector3(movement.x, 0.0f, movement.y);
				
				if (movement != Vector2.zero)
				{
					_targetRotation = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg +
					                  _mainCamera.transform.eulerAngles.y;
					float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
						ref _rotationVelocity, RotationSmoothTime);

					// rotate to face input direction relative to camera position
					transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
				}
			}
		}

		public void BackstabAttack()
		{
			// TODO: Add backstab modifier to weapon?
			_currentTarget.GetComponent<Enemy>().Backstab(2 * _stats.GetStatValue(StatName.Damage));
		}

		public void EndRoll()
		{
			_isRolling = false;
			_animator.SetBool("Rolling", false);
			_animator.applyRootMotion = false;
		}

		public void EndBackstabbing()
		{
			_isBackstabbing = false;
			_animator.SetBool("Backstab", false);
			_animator.applyRootMotion = false;
		}

		private void JumpAndGravity()
		{
			if (_inCheckpoint != -1) return;
			
			if (Grounded && !_isBackstabbing && !_resurrecting && !_staggerable.IsStaggered())
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				if (!_attacker.IsAttacking() && !_staggerable.IsStaggered() || isDead())
				{
					// Roll
					if (InputManager.Action("Roll").WasPressedThisFrame() && Stamina >= Mathf.Abs(StaminaUsageRoll) &&
					    !_isRolling && _verticalVelocity <= 0.0f)
					{
						ChangeStamina(StaminaUsageRoll);
						_isRolling = true;
						_animator.SetBool("Rolling", true);
						_animator.applyRootMotion = true;
					}

					// Jump
					if (InputManager.Action("Jump").WasPressedThisFrame() && Stamina >= Mathf.Abs(StaminaUsageJump) && !_isRolling)
					{
						ChangeStamina(StaminaUsageJump);
					
						// the square root of H * -2 * G = how much velocity needed to reach desired height
						_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
					
						// update animator if using character
						if (_hasAnimator)
						{
							_animator.SetBool(_animIDJump, true);
						}
					}

					// jump timeout
					if (_jumpTimeoutDelta >= 0.0f)
					{
						_jumpTimeoutDelta -= Time.deltaTime;
					}
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDFreeFall, true);
					}
				}
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private void Attacks()
		{
			if (_inCheckpoint != -1)
				return;

			if (_staggerable.IsStaggered() || isDead())
				return;

			if (StaminaUsageAttacks.Keys.All(a => !InputManager.Action(a).WasPressedThisFrame() || !Grounded))
				return;

			if (_isRolling)
				return;

			string attack = StaminaUsageAttacks.Keys.First(a => InputManager.Action(a).WasPressedThisFrame());
			float staminaUsage = StaminaUsageAttacks[attack];

			if (_backstabTargets.Count > 0)
			{
				_currentTarget = null;
				foreach (GameObject target in _backstabTargets)
				{
					float dotProd = Vector3.Dot(target.transform.forward.normalized, transform.forward.normalized);
					if (dotProd < backstabAngleOffset) continue;

					_isBackstabbing = true;
					_currentTarget = target;
					_animator.SetBool("Backstab", true);
					_animator.applyRootMotion = true;
					return;
				}
			}
			if (_stamina >= Mathf.Abs(staminaUsage))
			{

				_attacker.Attack(attack);
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;
			
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

		public void SetBackstabTarget(GameObject enemy)
		{
			_backstabTargets.Add(enemy);
		}
		
		public void RemoveBackstabTarget(GameObject enemy)
		{
			_backstabTargets.Remove(enemy);
		}

		public void ChangeStamina(float delta)
		{
			Stamina += delta;
			if (Stamina <= 0) Stamina = 0;
		}

		public void EnterCheckpoint(int checkpointNumber)
		{
			_inCheckpoint = checkpointNumber;
			_animator.SetTrigger("Checkpoint");
		}
		
		public void InCheckpoint(int checkpointNumber)
		{
			_inCheckpoint = checkpointNumber;
			_animator.SetBool("InCheckpoint", true);
		}

		public bool IsInCheckpoint()
		{
			return _inCheckpoint != -1;
		}

		public bool IsInCheckpoint(int checkpoint)
		{
			return _inCheckpoint == checkpoint;
		}

		public int GetCheckpoint()
		{
			return _inCheckpoint;
		}

		public void ExitCheckpoint()
		{
			_animator.SetBool("InCheckpoint", false);
		}

		public void OnExitCheckpointEnd()
		{
			_inCheckpoint = -1;
		}

		public bool IsRolling()
		{
			return _isRolling;
		}

		public void StartResurrection(Animator enemy)
		{
			if (!_resurrecting)
			{
				_animator.SetTrigger("Resurrection");
				_resurrecting = true;
				_enemyToResurrect = enemy;
			}
		}
		
		public void EndResurrection()
		{
			_animator.SetTrigger("EndResurrection");
			_enemyToResurrect.SetTrigger("Resurrect");
			_resurrecting = false;
		}

		public float GetStamina()
		{
			return _stamina;
		}

		public bool isDead()
		{
			return _animator.GetBool("isDying");
		}
		
		public void resetDeadAnimationBool()
		{
			if(isDead())
				_animator.SetBool("isDying",false);
			else
				_animator.SetBool("isDying",true);
		}
	}
}