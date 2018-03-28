using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using CarsRentEF.EF;
using CarsRentEF.Models;
using CarsRentEF.Repos;
using CarsRentMVC.Models.ViewModels;

namespace CarsRentMVC.Controllers
{
    [Authorize(Roles = "manager")]
    public class RentController : Controller
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

        // GET: Rent
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

        // GET: Rent/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Rent rent = await _repo.GetOneAsync(id);

            if (rent == null)
                return HttpNotFound();

            return View(rent);
        }

        public ActionResult ClientSearch(int id)
        {
            Client client = _db.Клиенты.Find(id);
            return PartialView("_ClientAjaxPartial", client);
        }

        public ActionResult CarSearch(int id)
        {
            Car car = _db.Автомобили.Find(id);
            return PartialView("_CarAjaxPartial", car);
        }

        public ActionResult OtherCheckbox(Rent rent)
        {
            return PartialView("_OtherCheckboxPartial", rent);
        }

        // GET: Rent/Create
        public ActionResult Create()
        {
            ViewBag.АвтоID = new SelectList(_db.Автомобили, "АвтоID", "Госномер");
            ViewBag.КлиентID = new SelectList(_db.Клиенты, "КлиентID", "НомерПаспорта");
            ViewBag.SaveClientId = 1;
            ViewBag.SaveCarId = 1;
            ViewBag.Statuses = new SelectList(new List<Status> {
                new Status {Name = "Бронь", Value = "Бронь"},
                new Status {Name = "Эксплуатация", Value = "Эксплуатация"},
                new Status {Name = "Закрыт", Value = "Закрыт"},
                new Status {Name = "Просрочен", Value = "Просрочен"}
            }, "Value", "Name");

            Rent rent = new Rent {
                DiscountsForRent = _db.Скидки,
                OptionsForRent = _db.Options,
                Состояние = "Бронь",
                ДатаВыдачи = DateTime.Now,
                ДатаВозврата = DateTime.Now.AddDays(1)
            };
            return View(rent);
        }

        // POST: Rent/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "КлиентID,АвтоID,ДатаВыдачи,ДатаВозврата,ИтоговаяСумма,Состояние")] Rent rent,
            List<string> discounts,
            List<string> options,
            string statuses)
        {
            rent.DiscountsForRent = _db.Скидки; // Чтобы при перегрузке представления не исчезали checkbox
            rent.OptionsForRent = _db.Options; // Чтобы при перегрузке представления не исчезали checkbox
            ViewBag.SaveClientId = rent.КлиентID;
            ViewBag.SaveCarId = rent.АвтоID;

            Car car = _db.Автомобили.Find(rent.АвтоID);
            Client client = _db.Клиенты.Find(rent.КлиентID);
            TimeSpan ts = rent.ДатаВозврата - rent.ДатаВыдачи;
            int days = ts.Days;
            double sum = days * car.СтоимостПрокатаВСут;

            IEnumerable<Option> opt = rent.OptionsForRent = _db.Options;
            double sumOfOptions = 0;
            foreach (Option i in opt)
                if (options.Contains(i.RentDayCost.ToString())) {
                    sumOfOptions += i.RentDayCost * (double)days;
                    rent.ДопОборудование += i.Name + " " + i.RentDayCost + "$; ";
                }

            sum += sumOfOptions;

            IEnumerable<Discount> ds = rent.DiscountsForRent = _db.Скидки;
            double percentOfDiscount = 0;
            foreach (Discount i in ds)
                if (discounts.Contains(i.Процент.ToString())) {
                    percentOfDiscount += i.Процент;
                    rent.Скидки += i.НаименованиеСкидки + " " + i.Процент + "%; ";
                }
            double sumOfDiscount = (sum * percentOfDiscount) / 100;

            if (!ModelState.IsValid) {
                ModelState.AddModelError(string.Empty,
                    @"Ошибка в данных. Пожалуйста проверьте все поля.");
                return View(rent);
            }

            ViewBag.АвтоID = new SelectList(_db.Автомобили, "АвтоID", "Госномер", rent.АвтоID);
            ViewBag.КлиентID = new SelectList(_db.Клиенты, "КлиентID", "НомерПаспорта", rent.КлиентID);
            ViewBag.Statuses = new SelectList(new List<Status> {
                new Status {Name = "Бронь", Value = "Бронь"},
                new Status {Name = "Эксплуатация", Value = "Эксплуатация"},
                new Status {Name = "Закрыт", Value = "Закрыт"},
                new Status {Name = "Просрочен", Value = "Просрочен"}
            }, "Value", "Name");


            // Если автомобиль уже учавствует в договоре аредны
            if (car?.Статус == "Арендован") {
                ModelState.AddModelError(string.Empty,
                    @"Этот автомобиль арендован другим клиентом. Выберите другое авто.");
                return View(rent);
            }
            car.Статус = "Арендован";

            if (client.ЧерныйСписок.ToLower() == "да") {
                ModelState.AddModelError(string.Empty,
                    @"Клиент находится в черном списке. С ним невозможно заключить договор до выяснения обстоятельств.");
                return View(rent);
            }

            rent.Состояние = statuses;
            if (rent.Состояние.ToLower() == "закрыт") {
                ModelState.AddModelError(string.Empty, @"Невозможно создание нового договора с состоянием 'Закрыт'.");
                return View(rent);
            }

            if (rent.Состояние.ToLower() == "просрочен") {
                ModelState.AddModelError(string.Empty, @"Невозможно создание нового договора с состоянием 'Просрочен'.");
                return View(rent);
            }

            if (rent.ДатаВозврата <= rent.ДатаВыдачи) {
                ModelState.AddModelError(string.Empty, @"Дата возврата не может быть наступить в тот же день или раньше даты выдачи.");
                return View(rent);
            }

            // Высчитать сумму договора аренды
            rent.ИтоговаяСумма = sum - sumOfDiscount;

            try {
                _db.SaveChanges();
                await _repo.AddAsync(rent);
                TempData["message"] = $"Договор аренды с ID={rent.АрендаID} сохранен в БД.";
                return RedirectToAction("Details", new {id = rent.АрендаID});

            } catch (Exception ex) {
                ModelState.AddModelError(string.Empty, $"Невозможно сохранить объект в БД: {ex.Message}");
                return View(rent);
            }
        }

        public ActionResult RentDiscountCheckbox(int id)
        {
            Rent rent = _db.Аренды.Find(id);
            rent.DiscountsForRent = _db.Скидки;
            return PartialView("_CheckBoxDiscountPartial", rent);
        }

        public ActionResult RentOptionCheckbox(int id)
        {
            Rent rent = _db.Аренды.Find(id);
            rent.OptionsForRent = _db.Options;
            return PartialView("_CheckBoxOptionPartial", rent);
        }

        // GET: Rent/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rent rent = await _db.Аренды.FindAsync(id);
            if (rent == null) {
                return HttpNotFound();
            }
            ViewBag.АвтоID = new SelectList(_db.Автомобили, "АвтоID", "Госномер", rent.АвтоID);
            ViewBag.КлиентID = new SelectList(_db.Клиенты, "КлиентID", "НомерПаспорта", rent.КлиентID);
            ViewBag.Statuses = new SelectList(new List<Status> {
                new Status {Name = "Бронь", Value = "Бронь"},
                new Status {Name = "Эксплуатация", Value = "Эксплуатация"},
                new Status {Name = "Закрыт", Value = "Закрыт"},
                new Status {Name = "Просрочен", Value = "Просрочен"}
            }, "Value", "Name");
            ViewBag.PreviousAutoId = rent.АвтоID;
            ViewBag.Penalties = rent.Штрафы;
            rent.DiscountsForRent = _db.Скидки;
            rent.OptionsForRent = _db.Options;

            rent.PenaltyForRent = _db.Штрафы;
            IEnumerable<Penalty> pt = rent.PenaltyForRent;
            double sum = 0;
            foreach (Penalty i in pt) {
                if (rent.Штрафы.Contains(i.НаименованиеШтрафа))
                    sum += i.СуммаШтрафа;
            }
            ViewBag.SumOfPenalties = sum;


            return View(rent);
        }

        // POST: Rent/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "АрендаID,КлиентID,АвтоID,ДатаВыдачи,ДатаВозврата,ИтоговаяСумма,Состояние,Timestamp")] Rent rent,
            List<string> discounts,
            List<string> options,
            string status,
            string previousAutoId,
            string rentpenalties,
            string rentsumofpenalties)
        {
            rent.DiscountsForRent = _db.Скидки; // Чтобы при перегрузке представления не исчезали checkbox
            rent.OptionsForRent = _db.Options; // Чтобы при перегрузке представления не исчезали checkbox
            rent.PenaltyForRent = _db.Штрафы;
            rent.Штрафы = rentpenalties;
            ViewBag.Penalties = rentpenalties;
            ViewBag.SumOfPenalties = double.Parse(rentsumofpenalties);

            if (!ModelState.IsValid) {
                ModelState.AddModelError(string.Empty,
                    @"Ошибка в модели данных. Проверте значения всех полей.");
                return View(rent);
            }

            ViewBag.АвтоID = new SelectList(_db.Автомобили, "АвтоID", "Госномер", rent.АвтоID);
            ViewBag.КлиентID = new SelectList(_db.Клиенты, "КлиентID", "НомерПаспорта", rent.КлиентID);
            ViewBag.Statuses = new SelectList(new List<Status> {
                new Status {Name = "Бронь", Value = "Бронь"},
                new Status {Name = "Эксплуатация", Value = "Эксплуатация"},
                new Status {Name = "Закрыт", Value = "Закрыт"},
                new Status {Name = "Просрочен", Value = "Просрочен"}
            }, "Value", "Name");

            Car car = _db.Автомобили.Find(rent.АвтоID);
            Client client = _db.Клиенты.Find(rent.КлиентID);

            if (client.ЧерныйСписок.ToLower() == "да") {
                ModelState.AddModelError(string.Empty,
                    @"Клиент находится в черном списке. С ним невозможно заключить договор до выяснения обстоятельств.");
                return View(rent);
            }

            // Если автомобиль уже учавствует в договоре аредны
            int myStartCar = int.Parse(previousAutoId);
            if (car?.Статус == "Арендован" && rent.АвтоID != myStartCar) {
                ModelState.AddModelError(string.Empty,
                    @"Этот автомобиль арендован другим клиентом. Выберите другое авто.");

                Car car2 = _db.Автомобили.Find(myStartCar);
                if (car2 != null) {
                    car2.Статус = "Свободен";
                    _db.SaveChanges();
                }

                ViewBag.PreviousAutoId = 0;
                return View(rent);
            }

            rent.Состояние = status;
            if (rent.Состояние.ToLower() == "закрыт") {
                car.Статус = "Свободен";
            } else
                car.Статус = "Арендован";

            if (rent.ДатаВозврата <= rent.ДатаВыдачи) {
                ModelState.AddModelError(string.Empty, @"Дата возврата не может быть наступить в тот же день или раньше даты выдачи.");
                return View(rent);
            }

            TimeSpan ts = rent.ДатаВозврата - rent.ДатаВыдачи;
            int days = ts.Days;
            double sum = days * car.СтоимостПрокатаВСут;

            rent.ИтоговаяСумма += double.Parse(rentsumofpenalties);

            IEnumerable<Option> opt = rent.OptionsForRent = _db.Options;
            double sumOfOptions = 0;
            foreach (Option i in opt)
                if (options.Contains(i.RentDayCost.ToString())) {
                    sumOfOptions += i.RentDayCost * (double)days;
                    rent.ДопОборудование += i.Name + " " + i.RentDayCost + "$; ";
                }

            sum += sumOfOptions;

            IEnumerable<Discount> ds = rent.DiscountsForRent = _db.Скидки;
            double percentOfDiscount = 0;
            foreach (Discount i in ds)
                if (discounts.Contains(i.Процент.ToString())) {
                    percentOfDiscount += i.Процент;
                    rent.Скидки += i.НаименованиеСкидки + " " + i.Процент + "%; ";
                }
            double sumOfDiscount = (sum * percentOfDiscount) / 100;

            // Высчитать сумму договора аренды
            rent.ИтоговаяСумма += sum - sumOfDiscount;

            try {
                _db.SaveChanges();
                await _repo.SaveAsync(rent);
                TempData["message"] = $"Договор аренды с ID={rent.АрендаID} сохранен в БД.";
                return RedirectToAction("Details", new {id = rent.АрендаID});

            } catch (Exception ex) {
                ModelState.AddModelError(string.Empty, $"Unable to create record: {ex.Message}");
                return View(rent);
            }
        }

        // GET: Rent/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Rent rent = await _repo.GetOneAsync(id);

            if (rent == null)
                return HttpNotFound();

            return View(rent);
        }

        // POST: Rent/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Rent rent= await _db.Аренды.FindAsync(id);
            Car car = _db.Автомобили.Find(rent?.АвтоID);
            if (car != null)
                car.Статус = "Свободен";

            _db.Аренды.Remove(rent);
            await _db.SaveChangesAsync();
            TempData["message"] = $"Договор аренды с ID={rent.АрендаID} удален из БД.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _repo.Dispose(); // для DbContext
                _db.Dispose();
            }
            base.Dispose(disposing);  // для Controller
        }
    }
}
