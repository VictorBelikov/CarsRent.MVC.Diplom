using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsRentEF.Models;

namespace CarsRentEF.Repos
{
    public class DiscountRepo : BaseRepo<Discount>, IRepo<Discount>
    {
        public DiscountRepo() {
            Table = Context.Скидки;
        }

        public int Delete(int id, byte[] timeStamp) {
            Context.Entry(new Discount() {
                СкидкаID = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChanges();
        }

        public Task<int> DeleteAsync(int id, byte[] timeStamp) {
            Context.Entry(new Discount() {
                СкидкаID = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }
    }
}
