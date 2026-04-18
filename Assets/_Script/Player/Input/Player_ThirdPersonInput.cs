using Cinemachine;
using UnityEngine;

namespace TD
{
    public class Player_ThirdPersonInput : MonoBehaviour, PlayerInput.IThirdPersonInputActions
    {
        public Vector2 ScrollInput { get; private set; }

        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float cameraZoomSpeed = 0.1f;
        [SerializeField] private float cameraMinZoom = 1f;
        [SerializeField] private float cameraMaxZoom = 5f;

        private Cinemachine3rdPersonFollow thirdpersonsFollow;

        private void Awake()
        {
            thirdpersonsFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }

        private void OnEnable()
        {
            if (PlayerInputManager.Instance?.playerInput == null)
            {
                Debug.LogError("PlayerInputManager instance or playerInput is null. cannot enable");
                return;
            }

            PlayerInputManager.Instance.playerInput.ThirdPersonInput.Enable();
            PlayerInputManager.Instance.playerInput.ThirdPersonInput.SetCallbacks(this);
        }

        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.playerInput == null)
            {
                Debug.LogError("PlayerInputManager instance or playerInput is null. cannot disable");
                return;
            }

            PlayerInputManager.Instance.playerInput.ThirdPersonInput.Disable();
            PlayerInputManager.Instance.playerInput.ThirdPersonInput.RemoveCallbacks(this);
        }

        private void Update()
        {
            thirdpersonsFollow.CameraDistance = Mathf.Clamp(thirdpersonsFollow.CameraDistance + ScrollInput.y, cameraMinZoom, cameraMaxZoom);
        }

        private void LateUpdate()
        {
            ScrollInput = Vector2.zero;
        }

        public void OnScrollCamera(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if(!context.performed)
            {
                return;
            }

            Vector2 scrollinput = context.ReadValue<Vector2>();
            ScrollInput = -1.0f * scrollinput.normalized * cameraZoomSpeed;
        }

    }

}