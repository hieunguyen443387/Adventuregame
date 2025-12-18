using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject shurikenPrefab;     // Drag Shuriken prefab vào
    public Transform firePoint;         // Vị trí nems shuriken
    public bool facingRight = false;    // Hướng bắn
    //public Animator animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ThrowShuriken();
    }

    void ThrowShuriken()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Spawn shuriken
            GameObject shuriken = Instantiate(shurikenPrefab, firePoint.position, Quaternion.identity);
            // Nếu shuriken  có collider, bỏ va chạm giữa chúng để viên đạn không tự huỷ ngay khi spawn
            Collider2D shurikenCol = shuriken.GetComponent<Collider2D>();
            Collider2D ownerCol = GetComponent<Collider2D>();
            if (shurikenCol != null && ownerCol != null)
            {
                Physics2D.IgnoreCollision(shurikenCol, ownerCol);
            }
            Shuriken shurikenScript = shuriken.GetComponent<Shuriken>();
            if (shurikenScript != null)
            {
                float dir = transform.localScale.x > 0 ? 1f : -1f;
                shurikenScript.direction = new Vector2(dir, 0);
            }
            Debug.Log("Shoot() được gọi!");

            Debug.Log("shurikenPrefab = " + shurikenPrefab);
            Debug.Log("firePoint = " + firePoint);
            // Hủy viên đạn sau 6s nếu không va chạm gì để dọn dẹp
            Destroy(shuriken, 6f);
            Debug.Log("Đã tạo shuriken tại vị trí " + firePoint.position);
        }
    }
}