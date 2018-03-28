using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsRentEF.Models;

namespace CarsRentEF.Repos
{
    public class ClientRepo : BaseRepo<Client>, IRepo<Client>
    {
        public ClientRepo() {
            Table = Context.Клиенты;
        }

        public int Delete(int id, byte[] timeStamp) {
            Context.Entry(new Client() {
                КлиентID = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChanges();
        }

        public Task<int> DeleteAsync(int id, byte[] timeStamp) {
            Context.Entry(new Client() {
                КлиентID = id,
                Timestamp = timeStamp
            }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }
    }
}
