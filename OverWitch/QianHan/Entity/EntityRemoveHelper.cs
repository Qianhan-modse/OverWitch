using System.Collections.Generic;
using OverWitch.QianHan.Entities;
using UnityEngine;

namespace Assets.OverWitch.QianHan.Entities
{
    public class EntityRemoveHelper
    {
        static Entity entity;
        static GameObject gameObject;
        //这是一个私有构造函数，用于创建一个实体移除助手
        private EntityRemoveHelper(Entity living,GameObject game)
        {
            entity = living;
            gameObject = game;
        }
        /// <summary>
        /// 这里是用于清理合集的方法，用于清理实体的所有组件，或许是所有组件，谁知道呢
        /// </summary>
        /// <param name="parent"></param>
        private static void ClearChildEntities(Transform parent)
        {
            var childrenToRemove = new List<Entity>();
            foreach(Transform child in parent)
            {
                var childEntity = child.GetComponent<Entity>();
                if(childEntity!=null)
                {
                    childrenToRemove.Add(childEntity);
                }
            }
            World.removeEntityAll(childrenToRemove);

        }
        /// <summary>
        /// 这里是私有方法，用于清除游戏对象，拒绝使用Unity自带的Destroy方法，因为自带的很废，根本不满足需求
        /// 目前可能存在问题，需要进一步测试
        /// </summary>
        private void clearGameObject(GameObject gameObject)
        {
            gameObject.SetActive(false);
            if (gameObject = null) return;
            if (gameObject is GameObject)
            {
                ClearChildEntities(gameObject.transform);
            }
            else
            {
                World.removeEntity(entity);
            }
        }
        /// <summary>
        /// 这只是一个简单的方法，用于删除游戏对象
        /// 目前可能存在问题，需要进一步测试
        /// </summary>
        /// <param name="object"></param>
        public void removeGameObject(GameObject @object)
        {
            //虽然实体类型是游戏对象的一部分，但为了保证可能出现的离谱情况，还是要进行判断
            //这里的游戏对象等于实体是冗沉的，但是为了保证代码的健壮性，还是要进行判断
            {
                if (gameObject = null) return;
                else if (entity.GameObject)
                {
                    World.removeEntity(entity);
                }
                else
                {
                    clearGameObject(@object);
                }
            }
        }
    }
}
