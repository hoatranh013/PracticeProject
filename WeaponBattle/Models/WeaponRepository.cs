namespace WeaponBattle.Models
{
    public class WeaponRepository : IWeaponRepository
    {
        private List<WeaponModel> WeaponModels = new List<WeaponModel>();
        private int _nextId = 1;


        public IEnumerable<WeaponModel> GetAll()
        {
            return WeaponModels;
        }

        public WeaponModel Get(int id)
        {
            return WeaponModels.Find(p => p.WeaponId == id);
        }

        public WeaponModel Add(WeaponModel item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            item.WeaponId = _nextId++;
            WeaponModels.Add(item);
            return item;
        }

        public void Remove(int id)
        {
            WeaponModels.RemoveAll(p => p.WeaponId == id);
        }

        public bool Update(WeaponModel item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int index = WeaponModels.FindIndex(p => p.WeaponId == item.WeaponId);
            if (index == -1)
            {
                return false;
            }
            WeaponModels.RemoveAt(index);
            WeaponModels.Add(item);
            return true;
        }

    }
}