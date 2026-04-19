using UnityEngine;

namespace TD
{
    [CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
    public class WeaponScriptableObject : ScriptableObject
    {
        //Basic stats for all weapons
        [Header("Weapon Stats")]
        [SerializeField] private GameObject prefab;
        public GameObject Prefab { get => prefab; private set => prefab = value; }
        [SerializeField] private float damage;
        public float Damage { get => damage; private set => damage = value; }

        [SerializeField] private float speed;
        public float Speed { get => speed; private set => speed = value; }

        [SerializeField] private float cooldownDuration;
        public float CooldownDuration { get => cooldownDuration; private set => cooldownDuration = value; }

        [SerializeField] private int pierce;
        public int Pierce { get => pierce; private set => pierce = value; }
    }
}