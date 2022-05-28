namespace WeaponBattle.Models
{
    public interface IWeaponRepository
    {
        IEnumerable<WeaponModel> GetAll();
        WeaponModel Get(int id);
        WeaponModel Add(WeaponModel model);
        void Remove(int id);
        bool Update(WeaponModel item);
    }
}
