using UnityEngine;

namespace TD
{
    public class ExperienceGem : MonoBehaviour, ICollectible
    {
        public float rotateSpeed;
        public int experienceAmountGranted = 10;
        
        private void Update()
        {
            Handle_Rotation();
        }
        public void Collect()
        {
            PlayerStats player = FindAnyObjectByType<PlayerStats>();
            player.IncreaseExperience(experienceAmountGranted);
            Destroy(gameObject);
        }
        private void Handle_Rotation()
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}