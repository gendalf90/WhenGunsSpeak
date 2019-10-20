namespace Weapon
{
    public interface ISpawnable
    {
        void SpawnIfNameIs(string name);

        void UnspawnIfNameIs(string name);
    }
}
