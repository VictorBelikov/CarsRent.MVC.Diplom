using System.Collections.Generic;
using CarsRentEF.Models;

namespace CarsRentMVC.Models.ViewModels
{
    public class ReportDetailsViewModel
    {
        public Report Report { get; set; }
        public double FullProfit { get; set; }
        public int Days { get; set; }
        public IEnumerable<Rent> Rents { get; set; }
        public IEnumerable<Car> Cars { get; set; }
        public IEnumerable<Option> Options { get; set; }
    }
}