using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // 角色的位置
    public float distance = 5f;  // 相机与角色的距离
    public float height = 2f;  // 相机的高度
    public float rotationSpeed = 5f;  // 相机旋转速度
    public float mouseLookSpeedX = 3f;  // 鼠标X轴旋转速度

    private float currentX = 0f;

    private void Update()
    {
        HandleMouseLook();  // 更新鼠标旋转逻辑
    }

    private void FixedUpdate()
    {
        FollowPlayer();  // 每帧跟随角色位置
    }

    // 更新相机的位置
    private void FollowPlayer()
    {
        // 计算相机位置
        Vector3 desiredPosition = player.position - player.forward * distance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * rotationSpeed);

        // 保持相机始终朝向角色
        transform.LookAt(player);
    }

    // 处理鼠标控制的相机旋转
    private void HandleMouseLook()
    {
        // 获取鼠标移动的量
        currentX += Input.GetAxis("Mouse X") * mouseLookSpeedX;

        // 旋转角色
        player.Rotate(0, Input.GetAxis("Mouse X") * mouseLookSpeedX, 0);

        // 相机旋转，保持角色朝向
        transform.rotation = Quaternion.Euler(0f, currentX, 0f);
    }
}
