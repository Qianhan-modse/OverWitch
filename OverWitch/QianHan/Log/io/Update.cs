using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Items;
using OverWitch.QianHan.Util;

namespace Assets.OverWitch.QianHan.Log.io
{
    public interface Update
    {
        /// <summary>
        /// 这里是对应的更新方法
        /// </summary>
        /// <param name="world"></param>
        
        //世界更新时调用
        void onWorldUpdate(World world);
        //物品更新时调用可以被onItemUpdate代替
        void onUpdate(ItemTool tool);
        //实体更新时调用
        void onEntityUpdate();
        //当死亡时调用
        void onDeathUpdate(Entity entity, DamageSource source);
        //工具更新时调用，可替代onUpdate方法，虽然是onItemUpdate但需要传入ItemTool参数
        void onItemUpdate(Item item);

        /// <summary>
        /// 这里是对应的初始化方法
        /// </summary>
        //世界初始化，需要传入世界参数
        void onWorldStart(World world);

        //物品初始化，可以被onItemStart替代
        void onStart(Item item);
        //工具初始化，虽然是item但需要itemtool参数
        void onItemStart(ItemTool item);
        //实体初始化
        void onEntityStart();
    }
}
