using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // ��ɫ��λ��
    public float distance = 5f;  // ������ɫ�ľ���
    public float height = 2f;  // ����ĸ߶�
    public float rotationSpeed = 5f;  // �����ת�ٶ�
    public float mouseLookSpeedX = 3f;  // ���X����ת�ٶ�

    private float currentX = 0f;

    private void Update()
    {
        HandleMouseLook();  // ���������ת�߼�
    }

    private void FixedUpdate()
    {
        FollowPlayer();  // ÿ֡�����ɫλ��
    }

    // ���������λ��
    private void FollowPlayer()
    {
        // �������λ��
        Vector3 desiredPosition = player.position - player.forward * distance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * rotationSpeed);

        // �������ʼ�ճ����ɫ
        transform.LookAt(player);
    }

    // ���������Ƶ������ת
    private void HandleMouseLook()
    {
        // ��ȡ����ƶ�����
        currentX += Input.GetAxis("Mouse X") * mouseLookSpeedX;

        // ��ת��ɫ
        player.Rotate(0, Input.GetAxis("Mouse X") * mouseLookSpeedX, 0);

        // �����ת�����ֽ�ɫ����
        transform.rotation = Quaternion.Euler(0f, currentX, 0f);
    }
}
