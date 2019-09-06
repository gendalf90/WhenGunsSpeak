namespace Weapon
{
    public interface IShootable
    {
        void StartShooting();

        //можно покрыть кейсы с одиночным огнем
        void StopShooting();
    }
}
