using Assets.OverWitch.QianHan.Events.fml.events.worlds;
using OverWitch.QianHan.Entities;
using OverWitch.QianHan.Events;

namespace Assets.OverWitch.QianHan.Events.fml.events.worlds
{
    public class WorldEvent:Event
    {
        private World world;
        public WorldEvent(World world)
        {
            this.world = world;
        }
        public World getWorld() { return world; }
    }
    public class Load :WorldEvent
    {
        public Load(World world):base(world)
        {
        }
    }
    public class Unload : WorldEvent
    {
        public Unload(World world) : base(world)
        {
        }
    }
    public class Save : WorldEvent
    {
        public Save(World world) : base(world)
        {
        }
    }
    public class CreateSpawnPosition:WorldEvent
    {
        private WorldSettings settings;
        public CreateSpawnPosition(World world,WorldSettings settings) : base(world)
        {
            this.settings = settings;
        }
        public WorldSettings getSettings()
        {
            return settings;
        }
    }
}
