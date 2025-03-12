using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemEntityes
{
    /// <summary>
    /// 表示掉落物的实体
    /// </summary>
    public class ItemEntity : Entity
    {
        public new string name;  // 实体名称
        private float lifetime;  // 掉落物的生命周期
        private float maxLifetime = 300f;  // 最大生存时间，例如 30 秒
        private bool dead;  // 是否已死亡

        public override void Start()
        {
            base.Start();
            dead = false;
            lifetime = 0f;
            name = string.Empty; // 默认空名称
        }

        // 更新方法，检查掉落物的生存时间
        private void onItemUpdate()
        {
            if (dead)
            {
                // 如果物品已死亡，移除实体
                World.removeEntity(this);  // 使用空安全操作符
            }
            else
            {
                // 增加生存时间
                lifetime += Time.deltaTime;  // 使用时间增量来增加生存时间

                // 如果超过最大生存时间，标记为死亡
                if (lifetime > maxLifetime)
                {
                    dead = true;
                    World.removeEntity(this);  // 移除实体
                }
            }
        }

        // 如果物品已死亡，移除
        public void checkAndRemoveIfDead()
        {
            if (dead)
            {
                World.removeEntity(this);  // 移除实体
            }
        }

        // Update 方法，每帧调用，处理掉落物的生命周期
        public override void Update()
        {
            onItemUpdate();  // 每帧检查掉落物是否应该死亡
        }
    }
}
