using System;
using PlayerEntity;
using UnityEngine;

namespace Assets.StarTravelsDecisiveWarAge.代码逻辑
{
    public class PlayerMovement : MonoBehaviour
    {
        public EntityPlayer player;
        private float horizontalMove, verticalMove;
        private Vector3 moveDirection;
        public void Start()
        {
            player = GetComponent<EntityPlayer>();
            player.characterController = GetComponent<CharacterController>();
        }
        public void Update()
        {
            
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            // 根据是否在奔跑状态决定当前的速度
            if (isRunning)
            {
                Run();
            }
            else
            {
                Forward();
            }
            player.JumpAnimaotr(player.animator);
            // 获取输入（比如 WASD 或方向键）
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 move = transform.right * moveX + transform.forward * moveZ;

            // 角色移动
            player.characterController.Move(moveDirection * player.moveSpeed * Time.deltaTime);
            moveDirection = transform.forward * verticalMove + transform.right * horizontalMove;
        }
        public void Run()
        {
            player.moveSpeed = player.RunSpeed;
            horizontalMove = Input.GetAxis("Horizontal") * player.RunSpeed;
            verticalMove = Input.GetAxis("Vertical") * player.RunSpeed;
            
        }

        public void Forward()
        {
            player.moveSpeed=player.moveForward;
            // 获取水平和垂直方向的输入
            horizontalMove = Input.GetAxis("Horizontal") * player.moveForward;
            verticalMove = Input.GetAxis("Vertical") * player.moveForward;
        }
    }
}
