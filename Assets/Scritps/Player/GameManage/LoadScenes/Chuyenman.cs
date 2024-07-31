using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chuyenman : MonoBehaviour
{
    // Kiểm tra điều kiện để chuyển màn hình
    private void Update()
    {
        // Điều kiện để chuyển màn hình
        if (Input.GetKeyDown(KeyCode.N)) // Thay thế bằng điều kiện của bạn, ví dụ: hoàn thành nhiệm vụ
        {
            LoadNextScene();
        }
    }

    // Hàm để chuyển sang màn hình tiếp theo
    public void LoadNextScene()
    {
        // Lấy chỉ số của màn hình hiện tại
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Chuyển sang màn hình tiếp theo
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
