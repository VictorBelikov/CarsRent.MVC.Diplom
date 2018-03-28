using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsRentEF.Models;

namespace CarsRentEF.Repos
{
    public class PenaltyRepo : BaseRepo<Penalty>, IRepo<Penalty>
    {
        public PenaltyRepo() {
            Table = Context.Штрафы;
        }

        public int Delete(int id, byte[] timeStamp) {
            Context.Entry(new Penalty() {
                ШтрафID = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChanges();
        }

        public Task<int> DeleteAsync(int id, byte[] timeStamp) {
            Context.Entry(new Penalty() {
                ШтрафID = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }
    }
}
