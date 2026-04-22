using UnityEngine;

namespace TD
{
    public class ExperienceGem : Pick_Up_Manager, ICollectible
    {
        public int experienceAmountGranted = 10;
        public void Collect()
        {
            PlayerStats player = FindAnyObjectByType<PlayerStats>();
            player.IncreaseExperience(experienceAmountGranted);
        }
    }
}