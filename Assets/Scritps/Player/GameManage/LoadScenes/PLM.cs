using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PLM : MonoBehaviour
{
    private static PLM instance;

    void Awake()
    {
        // Kiểm tra nếu instance đã tồn tại
        if (instance == null)
        {
            // Nếu chưa tồn tại, thiết lập instance và không phá hủy đối tượng này khi tải màn hình mới
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Nếu đã tồn tại, phá hủy đối tượng này để tránh trùng lặp
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // Đăng ký sự kiện khi màn hình mới được tải
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Hủy đăng ký sự kiện khi màn hình mới được tải
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Hàm được gọi khi màn hình mới được tải
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Tìm đối tượng SpawnPoint trong màn hình mới
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint != null)
        {
            // Di chuyển player đến vị trí của SpawnPoint
            transform.position = spawnPoint.transform.position;
            CameraManager.instance.SetCameraTarget(transform);
        }
    }
}
