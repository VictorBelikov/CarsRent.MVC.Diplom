using System.Collections.Generic;
using CarsRentEF.Models;

namespace CarsRentMVC.Models.ViewModels
{
    public class ReportListViewModel
    {
        public IEnumerable<Report> Reports { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}