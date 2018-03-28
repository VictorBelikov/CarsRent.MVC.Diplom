using System.Collections.Generic;
using CarsRentEF.Models;

namespace CarsRentMVC.Models.ViewModels
{
    public class OptionListViewModel
    {
        public IEnumerable<Option> Options { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}