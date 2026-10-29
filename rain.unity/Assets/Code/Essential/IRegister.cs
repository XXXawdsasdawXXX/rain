namespace Code.Game.World.Turns
{
    public interface IRegister<TType>
    {
        void Register(TType entity);
        void UnRegister(TType entity);
    }
}