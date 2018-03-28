using System.Collections.Generic;
using CarsRentEF.Models;

namespace CarsRentMVC.Models.ViewModels
{
    public class RentListViewModel
    {
        public IEnumerable<Rent> Rents { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}