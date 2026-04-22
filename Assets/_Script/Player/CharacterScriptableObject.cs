using UnityEngine;

namespace TD
{
    [CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
    public class CharacterScriptableObject : ScriptableObject
    {
        [SerializeField] private GameObject staringWeapon;
        public GameObject StaringWeapon { get => staringWeapon; private set => staringWeapon = value; }
        [Header("Base Stats")]
        [SerializeField] private float maxHealth;
        public float MaxHealth { get => maxHealth; private set => maxHealth = value; }

        [SerializeField] private float recovery;
        public float Recovery { get => recovery; private set => recovery = value; }

        [Header("Base Movement")]
        [SerializeField] private float walkAcceleration;
        public float WalkAcceleration { get => walkAcceleration; private set => walkAcceleration = value; }
        [SerializeField] private float walkSpeed;
        public float WalkSpeed { get => walkSpeed; private set => walkSpeed = value; }
        [SerializeField] private float runAcceleration;
        public float RunAcceleration { get => runAcceleration; private set => runAcceleration = value; }
        [SerializeField] private float runSpeed;
        public float RunSpeed { get => runSpeed; private set => runSpeed = value; }
        [SerializeField] private float sprintAcceleration;
        public float SprintAcceleration { get => sprintAcceleration; private set => sprintAcceleration = value; }
        [SerializeField] private float sprintSpeed;
        public float SprintSpeed { get => sprintSpeed; private set => sprintSpeed = value; }
        [SerializeField] private float inAirAcceleration;
        public float InAirAcceleration { get => inAirAcceleration; private set => inAirAcceleration = value; }
        [SerializeField] private float drag;
        public float Drag { get => drag; private set => drag = value; }
        [SerializeField] private float inAirDrag;
        public float InAirDrag { get => inAirDrag; private set => inAirDrag = value; }
        [SerializeField] private float gravity;
        public float Gravity { get => gravity; private set => gravity = value; }
        [SerializeField] private float jumpSpeed;
        public float JumpSpeed { get => jumpSpeed; private set => jumpSpeed = value; }
        [SerializeField] private float movingThreshold;
        public float MovingThreshold { get => movingThreshold; private set => movingThreshold = value; }
        [SerializeField] private float terminalVelocity;
        public float TerminalVelocity { get => terminalVelocity; private set => terminalVelocity = value; }

        [Header("Base Varible")]
        [SerializeField] private float might;
        public float Might { get => might; private set => might = value; }

        [SerializeField] private float projectileSpeed;
        public float ProjectileSpeed { get => projectileSpeed; private set => projectileSpeed = value; }
        [SerializeField] private float magnet;
        public float Magnet { get => magnet; private set => magnet = value; }
    }
}