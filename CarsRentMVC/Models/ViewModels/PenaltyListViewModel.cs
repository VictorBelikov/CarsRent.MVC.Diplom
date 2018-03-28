using System.Collections.Generic;
using CarsRentEF.Models;

namespace CarsRentMVC.Models.ViewModels
{
    public class PenaltyListViewModel
    {
        public IEnumerable<Penalty> Penalties { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}