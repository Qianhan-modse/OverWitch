using System.Collections.Generic;
using OverWitch.QianHan.Entities;
using UnityEngine;

namespace Assets.OverWitch.QianHan.Entities
{
    public static class EntityDestoryHelper
    {
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
    }
}
