using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

namespace Work.Jiwon.Code.Player
{
    [CreateAssetMenu(fileName = "Input", menuName = "SO/Input/PlayerInput", order = 0)]
    public class PlayerInputSO : ScriptableObject,IPlayerActions
    {
        [SerializeField] private LayerMask whatIsGround;

        public event Action OnAttackPressed;
        public event Action OnUseSkillPressed;
        
        private Vector3 _screenPosition;
        private Vector3 _woldPosition;
        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        public void SetEnable(bool enable)
        {
            if (enable)
                _controls.Player.Enable();
            else
                _controls.Player.Disable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnAttackPressed?.Invoke();
        }

        public void OnPointer(InputAction.CallbackContext context)
        {
            _screenPosition = context.ReadValue<Vector2>();
        }

        public void OnUseSkill(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnUseSkillPressed?.Invoke();
        }

        public Vector3 GetWoldPosition()
        {
            Camera mainCam = Camera.main;
            Debug.Assert(mainCam != null, "No main camera");
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, mainCam.farClipPlane, whatIsGround.value))
            {
                _woldPosition = hit.point;
            }
            return _woldPosition;
        }
    }
}