using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace TD
{
    public class PlayerStats : MonoBehaviour
    {
        public CharacterScriptableObject characterData;

        //current stats 
        [HideInInspector] public float currentHealth;
        [HideInInspector] public float currentRecovery;
        [HideInInspector] public float currentMight;
        [HideInInspector] public float currentProjectileSpeed;
        [HideInInspector] public float currentwalkAcceleration;
        [HideInInspector] public float currentWalkSpeed;
        [HideInInspector] public float currentrunAcceleration;
        [HideInInspector] public float currentRunSpeed;
        [HideInInspector] public float currentsprintAcceleration;
        [HideInInspector] public float currentSprintSpeed;
        [HideInInspector] public float currentInAirAcceleration;
        [HideInInspector] public float currentDrag;
        [HideInInspector] public float currentInAirDrag;
        [HideInInspector] public float currentGravity;
        [HideInInspector] public float currentJumpSpeed;
        [HideInInspector] public float currentMovingThreshold;
        [HideInInspector] public float currentTerminalVelocity;
        [HideInInspector] public float currentMagnet;

        //Spawn Weapon
        [Header("Weapon Spawn")]
        public List<GameObject> spawnWeapons;

        //Experience and level of the player
        [Header("Experience/Level")]
        public int experience = 0;
        public int level = 1;
        public int experienceCap;

        //Lớp để định nghĩa khoảng cấp độ và lượng kinh nghiệm cần thiết để tăng cấp
        [System.Serializable]
        public class LevelRange
        {
            public int startLevel;
            public int endLevel;
            public int experienceCapIncrease;
        }

        //I-Frames để player không bị tổn thương trong một khoảng thời gian sau khi bị đánh trúng
        [Header("I-Frames")]
        public float invincibilityDuration = 0.5f; // Thời gian bất tử sau khi bị đánh trúng
        private float invincibilityTimer = 0.0f;
        private bool isInvincible = false;

        public List<LevelRange> levelRanges;
        private void Awake()
        {
            characterData = CharacterSelector.GetData();
            CharacterSelector.instance.DestroySingleton(); // Hủy singleton sau khi lấy dữ liệu để tránh xung đột nếu có nhiều đối tượng PlayerStats

            // Gắn dữ liệu từ ScriptableObject vào các biến hiện tại
            currentHealth = characterData.MaxHealth;
            currentRecovery = characterData.Recovery;
            currentwalkAcceleration = characterData.WalkAcceleration;
            currentWalkSpeed = characterData.WalkSpeed;
            currentrunAcceleration = characterData.RunAcceleration;
            currentRunSpeed = characterData.RunSpeed;
            currentsprintAcceleration = characterData.SprintAcceleration;
            currentSprintSpeed = characterData.SprintSpeed;
            currentInAirAcceleration = characterData.InAirAcceleration;
            currentDrag = characterData.Drag;
            currentInAirDrag = characterData.InAirDrag;
            currentGravity = characterData.Gravity;
            currentJumpSpeed = characterData.JumpSpeed;
            currentMovingThreshold = characterData.MovingThreshold;
            currentTerminalVelocity = characterData.TerminalVelocity;
            currentMight = characterData.Might;
            currentProjectileSpeed = characterData.ProjectileSpeed;
            currentMagnet = characterData.Magnet;

            // Khởi tạo danh sách vũ khí đã spawn
            SpawnWeapon(characterData.StaringWeapon);
        }

        private void Start()
        {
            experienceCap = levelRanges[0].experienceCapIncrease; // Khởi tạo experience cap cho cấp độ 1
        }

        private void Update()
        {
            if (invincibilityTimer > 0)
            {
                invincibilityTimer -= Time.deltaTime;
            }
            // Khi thời gian bất tử kết thúc = 0, đặt lại trạng thái bất tử
            else if (isInvincible)
            {
                isInvincible = false; // Hết thời gian bất tử
            }

            Recover();
        }

        public void IncreaseExperience(int amount)
        {
            experience += amount;
            LevelUpChecker();
        }

        private void LevelUpChecker()
        {
            if(experience >= experienceCap)
            {
                level++;
                experience -= experienceCap; // Giảm kinh nghiệm sau khi lên cấp
                // Cập nhật experience cap cho cấp độ tiếp theo
                int experienceCapIncrease = 0;
                foreach (LevelRange range in levelRanges)
                {
                    if (level >= range.startLevel && level <= range.endLevel)
                    {
                        experienceCapIncrease = range.experienceCapIncrease;
                        break;
                    }
                }
                experienceCap += experienceCapIncrease;
            }
        }

        public void TakeDamage(float damage)
        {
            // Nếu đang trong thời gian bất tử, không nhận sát thương
            if (!isInvincible)
            {
                currentHealth -= damage;

                invincibilityTimer = invincibilityDuration; // Reset timer bất tử
                isInvincible = true; // Kích hoạt trạng thái bất tử

                if (currentHealth <= 0)
                {
                    Death();
                }
            }
        }

        public void Death()
        {
            Debug.Log("Player has died.");
        }

        public void ResstoreHealth(float amount)
        {
            // Chỉ hồi máu nếu hiện tại chưa đầy máu tối đa
            if (currentHealth <characterData.MaxHealth)
            {
                currentHealth += amount;

                if(currentHealth > characterData.MaxHealth)
                {
                    currentHealth = characterData.MaxHealth; // Đảm bảo không vượt quá máu tối đa
                }
            }
        }

        private void Recover()
        {
            if(currentHealth < characterData.MaxHealth)
            {
                currentHealth += currentRecovery * Time.deltaTime;

                if(currentHealth > characterData.MaxHealth)
                {
                    currentHealth = characterData.MaxHealth; // Đảm bảo không vượt quá máu tối đa
                }

            }
        }

        public void SpawnWeapon(GameObject weapon)
        {
            //Spawn vũ khí tại vị trí của player
            GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
            spawnedWeapon.transform.SetParent(transform); // Gắn vũ khí vào player để nó di chuyển cùng player
            spawnWeapons.Add(spawnedWeapon); // Thêm vũ khí vào danh sách vũ khí đã spawn
        }
    }
}