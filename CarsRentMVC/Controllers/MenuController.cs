using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarsRentMVC.Models;

namespace CarsRentMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly List<MenuItem> _items;

        public MenuController()
        {
            _items = new List<MenuItem> {
                new MenuItem {Name = "Администратор", Controller = "admin", Action = "index", Active = string.Empty},
                new MenuItem {Name = "Автомобили", Controller = "Car", Action = "Index", Active = string.Empty},
                new MenuItem {Name = "Штрафы", Controller = "Penalty", Action = "Index", Active = string.Empty},
                new MenuItem {Name = "Скидки", Controller = "Discount", Action = "Index", Active = string.Empty},
                new MenuItem {Name = "Опции", Controller = "Option", Action = "Index", Active = string.Empty},
                new MenuItem {Name = "Аккаунт", Controller = "Account", Action = "Index", Active = string.Empty},
                new MenuItem {Name = "Управление", Controller = "Manage", Action = "Index", Active = string.Empty},

                new MenuItem {Name = "Домой", Controller = "Home", Action = "Index", Active = string.Empty},
                new MenuItem {Name = "Аренды", Controller = "Rent", Action = "Index", Active = string.Empty},
                new MenuItem {Name = "Клиенты", Controller = "Client", Action = "Index", Active = string.Empty},
                new MenuItem {Name = "Статистические отчеты", Controller = "Report", Action = "Index", Active = string.Empty},
                new MenuItem {Name = "Штрафы аренды", Controller = "RentPenalty", Action = "Index", Active = string.Empty},
            };
        }

        public PartialViewResult Main(string a = "Index", string c = "Home")
        {
            _items.First(m => m.Controller == c).Active = "myMenuActive";
            return PartialView(_items);
        }

        public PartialViewResult AdminMain(string a = "Index", string c = "Admin")
        {
            _items.First(m => m.Controller == c).Active = "myMenuActive";
            return PartialView(_items);
        }
    }
}