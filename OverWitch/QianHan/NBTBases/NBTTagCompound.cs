namespace Assets.OverWitch.QianHan.NBTBases
{
    internal class NBTTagCompound : NBTBase
    {
        public override NBTBase copy()
        {
            return new NBTTagCompound();
        }

        public override byte getId()
        {
            return 10;
        }

        public override string toString()
        {
            throw new System.NotImplementedException();
        }
    }
}