namespace Code.Game.World.Turns
{
    public interface IRegister<in TType>
    {
        void Register(TType entity);
        void UnRegister(TType entity);
    }
}