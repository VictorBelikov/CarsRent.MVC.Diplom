using System.Collections.Generic;
using CarsRentEF.Models;

namespace CarsRentMVC.Models.ViewModels
{
    public class DiscountListViewModel
    {
        public IEnumerable<Discount> Discounts { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}