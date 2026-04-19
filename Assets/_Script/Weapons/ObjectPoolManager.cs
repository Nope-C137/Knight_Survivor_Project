using System.Collections.Generic;
using UnityEngine;

namespace TD
{
    // Class phụ để định nghĩa các loại Pool trong Inspector
    [System.Serializable]
    public class Pool
    {
        public string tag;          // Tên định danh (vd: "Knife", "Bullet", "Firewall")
        public GameObject prefab;    // Prefab của vũ khí đó
        public int size;            // Số lượng khởi tạo ban đầu
    }

    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance;

        public List<Pool> pools; // Danh sách các loại vũ khí bạn muốn tạo Pool

        // Dictionary dùng để truy xuất Pool cực nhanh bằng tên (Tag)
        private Dictionary<string, List<GameObject>> poolDictionary;

        private void Awake()
        {
            Instance = this;
            poolDictionary = new Dictionary<string, List<GameObject>>();

            // Khởi tạo tất cả các Pool có trong danh sách
            foreach (Pool pool in pools)
            {
                List<GameObject> objectPool = new List<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Add(obj);

                    // Để gọn Hierarchy, bạn có thể cho các object này làm con của PoolManager
                    obj.transform.SetParent(this.transform);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        // Hàm lấy Object theo tên Tag
        public GameObject GetPooledObject(string tag)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool với tag " + tag + " không tồn tại!");
                return null;
            }

            // Tìm object đang rảnh
            foreach (GameObject obj in poolDictionary[tag])
            {
                if (obj != null && !obj.activeInHierarchy)
                {
                    return obj;
                }
            }

            // Nếu hết đạn, tạo thêm đạn mới cho loại đó
            Pool poolToExpand = pools.Find(p => p.tag == tag);
            if (poolToExpand != null)
            {
                GameObject newObj = Instantiate(poolToExpand.prefab);
                newObj.SetActive(false);
                newObj.transform.SetParent(this.transform);
                poolDictionary[tag].Add(newObj);
                return newObj;
            }

            return null;
        }
    }
}