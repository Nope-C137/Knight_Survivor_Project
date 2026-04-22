using UnityEngine;

namespace TD
{
    public class ExperienceGem : MonoBehaviour, ICollectible
    {
        public int experienceAmountGranted = 10;
        public void Collect()
        {
            PlayerStats player = FindAnyObjectByType<PlayerStats>();
            player.IncreaseExperience(experienceAmountGranted);
            Destroy(gameObject);
        }
    }
}