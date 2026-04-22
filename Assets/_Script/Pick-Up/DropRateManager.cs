using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace TD
{
    public class DropRateManager : MonoBehaviour
    {
        [System.Serializable]
        public class Drops
        {
            public string itemName;
            public GameObject itemPrefabs;
            public float dropRate;
        }

        public List<Drops> dropsList;

        private void OnDestroy()
        {
            float randomNumber = UnityEngine.Random.Range(0f, 100f);
            List<Drops> possibleDrops = new List<Drops>();

            foreach (Drops rate in dropsList)
            {
                if (randomNumber <= rate.dropRate)
                {
                    possibleDrops.Add(rate);
                }
            }
            //Kiểm tra nếu có phần tử nào trong possibleDrops, nếu có thì chọn ngẫu nhiên một phần tử và instantiate nó
            if (possibleDrops.Count > 0)
            {
                Drops drops = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];

                // Nâng vị trí Spawn lên một chút (ví dụ +0.5f) để tránh kẹt vào sàn nhà
                Vector3 spawnPos = transform.position + Vector3.up * 0.5f;

                GameObject spawnedItem = Instantiate(drops.itemPrefabs, spawnPos, Quaternion.identity);

                // Lấy Rigidbody của item vừa tạo và triệt tiêu mọi lực văng
                Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }

    }
}
