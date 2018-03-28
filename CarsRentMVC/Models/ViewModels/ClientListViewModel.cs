using System.Collections.Generic;
using CarsRentEF.Models;

namespace CarsRentMVC.Models.ViewModels
{
    public class ClientListViewModel
    {
        public IEnumerable<Client> Clients { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
