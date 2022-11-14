using Photon.Pun;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool fire;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		PhotonView photonView;

		private void Awake()
		{
			photonView = GetComponent<PhotonView>();
		}

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			if (photonView != null && !photonView.IsMine)
				return;
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (photonView != null && !photonView.IsMine)
				return;
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (photonView != null && !photonView.IsMine)
				return;
			JumpInput(value.isPressed);
		}

		public void OnFire(InputValue value)
		{
			if (photonView != null && !photonView.IsMine)
				return;
			FireInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void FireInput(bool newFireState)
		{
			fire = newFireState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			if (photonView != null && !photonView.IsMine)
				return;
			//SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}