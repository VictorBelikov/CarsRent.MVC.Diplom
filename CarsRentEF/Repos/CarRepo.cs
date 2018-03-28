using System.Data.Entity;
using System.Threading.Tasks;
using CarsRentEF.Models;

namespace CarsRentEF.Repos
{
    public class CarRepo : BaseRepo<Car>, IRepo<Car>
    {
        public CarRepo() {
            Table = Context.Автомобили;
        }

        public int Delete(int id, byte[] timeStamp) {
            Context.Entry(new Car() {
                АвтоID = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChanges();
        }

        public Task<int> DeleteAsync(int id, byte[] timeStamp) {
            Context.Entry(new Car() {
                АвтоID = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }
    }
}
