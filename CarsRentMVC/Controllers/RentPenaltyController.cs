using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CarsRentEF.EF;
using CarsRentEF.Models;
using CarsRentEF.Repos;
using CarsRentMVC.Models.ViewModels;

namespace CarsRentMVC.Controllers
{
    [Authorize(Roles = "master")]
    public class RentPenaltyController : Controller
    {
        private CarsRentEntities _db = new CarsRentEntities();
        private readonly RentRepo _repo = new RentRepo();

        public ActionResult RentListReturnHelp(IEnumerable<Rent> rentList, int pageSize, int page)
        {
            return View("Index", new RentListViewModel {
                Rents = rentList,
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = pageSize
                }
            });
        }

        // GET: RentPenalty
        public ActionResult Index(string filter = null, int page = 1)
        {
            List<Rent> rents = new List<Rent>();
            rents = _repo.GetAll();
            ViewBag.RentFailed = 0;
            int pageSize = 10;

            foreach (Rent i in rents) {
                if (i.ДатаВозврата < (DateTime.Now - new TimeSpan(1, 0, 0, 0))
                    && i.Состояние.ToLower() == "эксплуатация") {
                    i.Состояние = "Просрочен";
                    _repo.Save(i);
                }
                if (i.Состояние.ToLower() == "просрочен")
                    ViewBag.RentFailed += 1;
            }

            return View("Index", new RentListViewModel {
                Rents = rents.OrderByDescending(r => r.ДатаВыдачи).Skip((page - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = rents.Count
                }
            });
        }

        [HttpPost]
        public ActionResult Index(string filter = null)
        {
            List<Rent> rents = new List<Rent>();
            rents = _repo.GetAll();
            ViewBag.RentFailed = 0;
            int pageSize = 10;
            int page = 1;

            foreach (Rent i in rents) {
                if (i.ДатаВозврата < (DateTime.Now - new TimeSpan(1, 0, 0, 0))
                    && i.Состояние.ToLower() == "эксплуатация") {
                    i.Состояние = "Просрочен";
                    _repo.Save(i);
                }
                if (i.Состояние.ToLower() == "просрочен")
                    ViewBag.RentFailed += 1;
            }

            IEnumerable<Rent> rentList;

            if (filter == "") {
                return View("Index", new RentListViewModel {
                    Rents = rents.OrderByDescending(r => r.ДатаВыдачи).Skip((page - 1) * pageSize).Take(pageSize),
                    PagingInfo = new PagingInfo {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = rents.Count
                    }
                });
            }

            rentList = rents.OrderByDescending(r => r.ДатаВыдачи).Where(x => x.Состояние.ToLower().Contains(filter.ToLower()));
            if (rentList.Count() != 0)
                return RentListReturnHelp(rentList, pageSize, page);

            rentList = rents.OrderByDescending(r => r.ДатаВыдачи).Where(x => x.Client.ФИО.ToLower().Contains(filter.ToLower()));
            if (rentList.Count() != 0)
                return RentListReturnHelp(rentList, pageSize, page);

            rentList = rents.OrderByDescending(r => r.ДатаВыдачи).Where(x => x.Car.Марка.ToLower().Contains(filter.ToLower()));
            if (rentList.Count() != 0)
                return RentListReturnHelp(rentList, pageSize, page);

            rentList = rents.OrderByDescending(r => r.ДатаВыдачи).Where(x => x.Car.Госномер.ToLower().Contains(filter.ToLower()));
            if (rentList.Count() != 0)
                return RentListReturnHelp(rentList, pageSize, page);

            rentList = rents.OrderByDescending(r => r.ДатаВыдачи).Where(x => x.Client.НомерПаспорта.ToLower().Contains(filter.ToLower()));
            if (rentList.Count() != 0)
                return RentListReturnHelp(rentList, pageSize, page);

            rentList = rents.OrderByDescending(r => r.ДатаВыдачи).Where(x => x.Client.Адрес.ToLower().Contains(filter.ToLower()));
            if (rentList.Count() != 0)
                return RentListReturnHelp(rentList, pageSize, page);

            rentList = rents.OrderByDescending(r => r.ДатаВыдачи).Where(x => x.Client.Телефон.ToLower().Contains(filter.ToLower()));
            if (rentList.Count() != 0)
                return RentListReturnHelp(rentList, pageSize, page);

            return RentListReturnHelp(rentList, pageSize, page);
        }

        public async Task<ActionResult> AddPenalty(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Rent rent = await _repo.GetOneAsync(id);

            if (rent == null)
                return HttpNotFound();

            rent.PenaltyForRent = _db.Штрафы;
            ViewBag.StartedPenalties = rent.Штрафы;

            IEnumerable<Penalty> pn = rent.PenaltyForRent = _db.Штрафы;
            double sumOfPenalties = 0;
            foreach (Penalty i in pn) {
                if (rent.Штрафы.Contains(i.НаименованиеШтрафа)) {
                    sumOfPenalties += i.СуммаШтрафа;
                }
            }

            ViewBag.PreviousSumOfPenalties = sumOfPenalties;
            ViewBag.FullRentSum = rent.ИтоговаяСумма;

            return View(rent);
        }

        [HttpPost]
        public ActionResult Save(int id, List<string> penalties, string startedpenalties)
        {
            Rent rent = _db.Аренды.Find(id);
            IEnumerable<Penalty> pn = rent.PenaltyForRent = _db.Штрафы;

            if (penalties.All(p => p == "false")) {
                foreach (Penalty i in pn)
                    if (startedpenalties.Contains(i.НаименованиеШтрафа))
                        rent.ИтоговаяСумма -= i.СуммаШтрафа;

                rent.Штрафы = "";
                _db.SaveChanges();
                TempData["message"] = $"Изменения внесены в договор и сохранены в БД.";
                return RedirectToAction("Index");
            }

            double sumOfPenalties = 0;
            foreach (Penalty i in pn) {
                if (penalties.Contains(i.СуммаШтрафа + i.НаименованиеШтрафа) && !rent.Штрафы.Contains(i.НаименованиеШтрафа)) {
                    sumOfPenalties += i.СуммаШтрафа;
                    rent.Штрафы += i.НаименованиеШтрафа + " " + i.СуммаШтрафа + "$; ";
                }

                if (!penalties.Contains(i.СуммаШтрафа + i.НаименованиеШтрафа) && startedpenalties.Contains(i.НаименованиеШтрафа)) {
                    rent.ИтоговаяСумма -= i.СуммаШтрафа;
                    string str = rent.Штрафы.Replace($"{i.НаименованиеШтрафа} {i.СуммаШтрафа}$; ", "");
                    rent.Штрафы = str;
                }
            }

            // Высчитать сумму договора аренды
            rent.ИтоговаяСумма = rent.ИтоговаяСумма + sumOfPenalties;

            _db.SaveChanges();
            TempData["message"] = $"Изменения внесены в договор и сохранены в БД.";
            return RedirectToAction("Index");
        }

        // GET: RentPenalty/Details
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Rent rent = await _repo.GetOneAsync(id);

            if (rent == null)
                return HttpNotFound();

            return View(rent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
