using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private void Awake()
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

    public void SetCameraTarget(Transform target)
    {
        // Tìm Cinemachine Virtual Camera trong màn hình hiện tại
        CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            vcam.Follow = target;
        }
    }
}
