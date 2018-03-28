using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CarsRentEF.Models;
using CarsRentEF.Repos;
using CarsRentMVC.Models.ViewModels;

namespace CarsRentMVC.Controllers
{
    [Authorize(Roles = "manager")]
    public class CarController : Controller
    {
        private readonly CarRepo _repo = new CarRepo();

        public ActionResult CarListReturnHelp(IEnumerable<Car> carList, int pageSize, int page)
        {
            return View("Index", new CarListViewModel {
                Cars = carList,
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = pageSize
                }
            });
        }

        // GET: Car
        public ActionResult Index(string filter = null, int page = 1)
        {
            List<Car> cars = new List<Car>();
            cars = _repo.GetAll();
            int pageSize = 10;

            ViewBag.CarBodies = new SelectList(new List<Status> {
                new Status {Name = "Выберите тип кузова...", Value = "Выберите тип кузова..."},
                new Status {Name = "Седан", Value = "Седан"},
                new Status {Name = "Универсал", Value = "Универсал"},
                new Status {Name = "Пикап", Value = "Пикап"},
                new Status {Name = "Микроавтобус", Value = "Микроавтобус"},
                new Status {Name = "Хэтчбек", Value = "Хэтчбек"},
                new Status {Name = "Внедорожник", Value = "Внедорожник"},
                new Status {Name = "Кабриолет", Value = "Кабриолет"},
                new Status {Name = "Купе", Value = "Купе"},
            }, "Value", "Name");

            return View("Index", new CarListViewModel {
                Cars = cars.OrderBy(r => r.СтоимостПрокатаВСут).Skip((page - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = cars.Count
                }
            });
        }

        [HttpPost]
        public ActionResult Index(string carbodies, string filter = null)
        {
            List<Car> cars = new List<Car>();
            cars = _repo.GetAll();
            int pageSize = 10;
            int page = 1;

            IEnumerable<Car> carList;

            ViewBag.CarBodies = new SelectList(new List<Status> {
                new Status {Name = "Выберите тип кузова...", Value = "Выберите тип кузова..."},
                new Status {Name = "Седан", Value = "Седан"},
                new Status {Name = "Универсал", Value = "Универсал"},
                new Status {Name = "Пикап", Value = "Пикап"},
                new Status {Name = "Микроавтобус", Value = "Микроавтобус"},
                new Status {Name = "Хэтчбек", Value = "Хэтчбек"},
                new Status {Name = "Внедорожник", Value = "Внедорожник"},
                new Status {Name = "Кабриолет", Value = "Кабриолет"},
                new Status {Name = "Купе", Value = "Купе"},
            }, "Value", "Name");

            if (filter == "") {
                return View("Index", new CarListViewModel {
                    Cars = cars.OrderBy(r => r.СтоимостПрокатаВСут).Skip((page - 1) * pageSize).Take(pageSize),
                    PagingInfo = new PagingInfo {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = cars.Count
                    }
                });
            }

            if (filter == "Выберите тип кузова...") {
                return View("Index", new CarListViewModel {
                    Cars = cars.OrderBy(r => r.СтоимостПрокатаВСут).Skip((page - 1) * pageSize).Take(pageSize),
                    PagingInfo = new PagingInfo {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = cars.Count
                    }
                });
            }

            carList = cars.OrderBy(r => r.СтоимостПрокатаВСут).Where(x => x.Статус.ToLower().Contains(filter.ToLower()));
            if (carList.Count() != 0)
                return CarListReturnHelp(carList, pageSize, page);

            carList = cars.OrderBy(r => r.СтоимостПрокатаВСут).Where(x => x.Модель.ToLower().Contains(filter.ToLower()));
            if (carList.Count() != 0)
                return CarListReturnHelp(carList, pageSize, page);

            carList = cars.OrderBy(r => r.СтоимостПрокатаВСут).Where(x => x.Марка.ToLower().Contains(filter.ToLower()));
            if (carList.Count() != 0)
                return CarListReturnHelp(carList, pageSize, page);

            carList = cars.OrderBy(r => r.СтоимостПрокатаВСут).Where(x => x.ТипКузова.ToLower().Contains(filter.ToLower()));
            if (carList.Count() != 0)
                return CarListReturnHelp(carList, pageSize, page);

            carList = cars.OrderBy(r => r.СтоимостПрокатаВСут).Where(x => x.Госномер.ToLower().Contains(filter.ToLower()));
            if (carList.Count() != 0)
                return CarListReturnHelp(carList, pageSize, page);

            carList = cars.OrderBy(r => r.СтоимостПрокатаВСут).Where(x => x.ТипДвигателя.ToLower().Contains(filter.ToLower()));
            if (carList.Count() != 0)
                return CarListReturnHelp(carList, pageSize, page);

            return CarListReturnHelp(carList, pageSize, page);
        }

        // GET: Car/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Car car = await _repo.GetOneAsync(id);

            if (car == null)
                return HttpNotFound();

            // если имя представления в методе не указано, то вернуть одноименное методу действия
            return View(car);
        }

        // GET: Car/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.Statuses = new SelectList(new List<Status> {
                new Status {Name = "Свободен", Value = "Свободен"},
                new Status {Name = "Арендован", Value = "Арендован"}
            }, "Value", "Name");

            ViewBag.Engines = new SelectList(new List<Status> {
                new Status {Name = "Бензин", Value = "Бензин"},
                new Status {Name = "Дизель", Value = "Дизель"},
                new Status {Name = "Газ", Value = "Газ"},
                new Status {Name = "Электричество", Value = "Электричество"}
            }, "Value", "Name");

            ViewBag.Bodies = new SelectList(new List<Status> {
                new Status {Name = "Седан", Value = "Седан"},
                new Status {Name = "Универсал", Value = "Универсал"},
                new Status {Name = "Пикап", Value = "Пикап"},
                new Status {Name = "Микроавтобус", Value = "Микроавтобус"},
                new Status {Name = "Хэтчбек", Value = "Хэтчбек"},
                new Status {Name = "Внедорожник", Value = "Внедорожник"},
                new Status {Name = "Кабриолет", Value = "Кабриолет"},
                new Status {Name = "Купе", Value = "Купе"},
            }, "Value", "Name");

            return View(new Car {
                Статус = "Свободен",
                ТипДвигателя = "Бензин",
                ТипКузова = "Седан"
            });
        }

        // POST: Car/Create
        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Car car, string statuses, string engines, string bodies, HttpPostedFileBase imageUpload = null)
        {
            if (!ModelState.IsValid) {
                ModelState.AddModelError(string.Empty,
                    @"Ошибка в данных. Пожалуйста проверьте все поля.");
                return View(car);
            }

            ViewBag.Statuses = new SelectList(new List<Status> {
                new Status {Name = "Свободен", Value = "Свободен"},
                new Status {Name = "Арендован", Value = "Арендован"}
            }, "Value", "Name");

            ViewBag.Engines = new SelectList(new List<Status> {
                new Status {Name = "Бензин", Value = "Бензин"},
                new Status {Name = "Дизель", Value = "Дизель"},
                new Status {Name = "Газ", Value = "Газ"},
                new Status {Name = "Электричество", Value = "Электричество"}
            }, "Value", "Name");

            ViewBag.Bodies = new SelectList(new List<Status> {
                new Status {Name = "Седан", Value = "Седан"},
                new Status {Name = "Универсал", Value = "Универсал"},
                new Status {Name = "Пикап", Value = "Пикап"},
                new Status {Name = "Микроавтобус", Value = "Микроавтобус"},
                new Status {Name = "Хэтчбек", Value = "Хэтчбек"},
                new Status {Name = "Внедорожник", Value = "Внедорожник"},
                new Status {Name = "Кабриолет", Value = "Кабриолет"},
                new Status {Name = "Купе", Value = "Купе"},
            }, "Value", "Name");

            car.ТипКузова = bodies;
            car.Статус = statuses;
            car.ТипДвигателя = engines;
            if (car.Статус.ToLower() != "свободен") {
                ModelState.AddModelError(string.Empty, @"Новый автомобиль в парке должен иметь состояние 'Свободен'.");
                return View(car);
            }

            int i;
            if (car.Госномер[0] == '-' || Int32.TryParse(car.Госномер, out i)) {
                ModelState.AddModelError(string.Empty, @"Введите корректный Госномер");
                return View(car);
            }

            if (car.ГодВыпуска <= 0 || car.ГодВыпуска < 1900) {
                ModelState.AddModelError(string.Empty, @"Введите корректный Год выпуска автомобиля");
                return View(car);
            }

            if (car.Стоимость <= 0 || car.СтоимостПрокатаВСут <= 0) {
                ModelState.AddModelError(string.Empty, @"Стоимость не может быть отрицательной, либо равной 0.");
                return View(car);
            }

            if (imageUpload != null) {
                var count = imageUpload.ContentLength;
                car.Image = new byte[count];
                imageUpload.InputStream.Read(car.Image, 0, (int)count);
                car.MimeType = imageUpload.ContentType;
            }

            try {
                await _repo.AddAsync(car);
                TempData["message"] = $"Автомобиль с госномером {car.Госномер} сохранен в БД.";
                return RedirectToAction("Details", new {id = car.АвтоID});

            } catch (Exception ex) {
                ModelState.AddModelError(string.Empty, $"Unable to create record: {ex.Message}");
                return View(car);
            }
        }

        // GET: Car/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Car car = await _repo.GetOneAsync(id);

            if (car == null)
                return HttpNotFound();

            ViewBag.Statuses = new SelectList(new List<Status> {
                new Status {Name = "Свободен", Value = "Свободен"},
                new Status {Name = "Арендован", Value = "Арендован"}
            }, "Value", "Name");

            ViewBag.Engines = new SelectList(new List<Status> {
                new Status {Name = "Бензин", Value = "Бензин"},
                new Status {Name = "Дизель", Value = "Дизель"},
                new Status {Name = "Газ", Value = "Газ"},
                new Status {Name = "Электричество", Value = "Электричество"}
            }, "Value", "Name");

            ViewBag.Bodies = new SelectList(new List<Status> {
                new Status {Name = "Седан", Value = "Седан"},
                new Status {Name = "Универсал", Value = "Универсал"},
                new Status {Name = "Пикап", Value = "Пикап"},
                new Status {Name = "Микроавтобус", Value = "Микроавтобус"},
                new Status {Name = "Хэтчбек", Value = "Хэтчбек"},
                new Status {Name = "Внедорожник", Value = "Внедорожник"},
                new Status {Name = "Кабриолет", Value = "Кабриолет"},
                new Status {Name = "Купе", Value = "Купе"},
            }, "Value", "Name");

            return View(car);
        }

        // POST: Car/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Car car, string statuses, string engines, string bodies, HttpPostedFileBase imageUpload = null)
        {
            ViewBag.Statuses = new SelectList(new List<Status> {
                new Status {Name = "Свободен", Value = "Свободен"},
                new Status {Name = "Арендован", Value = "Арендован"}
            }, "Value", "Name");

            ViewBag.Engines = new SelectList(new List<Status> {
                new Status {Name = "Бензин", Value = "Бензин"},
                new Status {Name = "Дизель", Value = "Дизель"},
                new Status {Name = "Газ", Value = "Газ"},
                new Status {Name = "Электричество", Value = "Электричество"}
            }, "Value", "Name");

            ViewBag.Bodies = new SelectList(new List<Status> {
                new Status {Name = "Седан", Value = "Седан"},
                new Status {Name = "Универсал", Value = "Универсал"},
                new Status {Name = "Пикап", Value = "Пикап"},
                new Status {Name = "Микроавтобус", Value = "Микроавтобус"},
                new Status {Name = "Хэтчбек", Value = "Хэтчбек"},
                new Status {Name = "Внедорожник", Value = "Внедорожник"},
                new Status {Name = "Кабриолет", Value = "Кабриолет"},
                new Status {Name = "Купе", Value = "Купе"},
            }, "Value", "Name");

            if (!ModelState.IsValid)
                return View(car);

            if (imageUpload != null) {
                var count = imageUpload.ContentLength;
                car.Image = new byte[count];
                imageUpload.InputStream.Read(car.Image, 0, (int)count);
                car.MimeType = imageUpload.ContentType;
            }

            car.ТипКузова = bodies;
            car.Статус = statuses;
            car.ТипДвигателя = engines;

            int i;
            if (car.Госномер[0] == '-' || Int32.TryParse(car.Госномер, out i)) {
                ModelState.AddModelError(string.Empty, @"Введите корректный Госномер");
                return View(car);
            }

            if (car.ГодВыпуска <= 0 || car.ГодВыпуска < 1900) {
                ModelState.AddModelError(string.Empty, @"Введите корректный Год выпуска автомобиля");
                return View(car);
            }

            if (car.Стоимость <= 0 || car.СтоимостПрокатаВСут <= 0) {
                ModelState.AddModelError(string.Empty, @"Стоимость не может быть отрицательной, либо равной 0.");
                return View(car);
            }

            if (car.Статус != "Свободен" && car.Статус != "Арендован") {
                ModelState.AddModelError(string.Empty,
                    @"Некорректное состояние автомобиля. Допустимые состояния: 'Свободен', 'Арендован'");
                return View(car);
            }

            try {
                await _repo.SaveAsync(car);
                TempData["message"] = $"Автомобиль с госномером {car.Госномер} сохранен в БД.";
                return RedirectToAction("Details", new {id = car.АвтоID});

            } catch (DbUpdateConcurrencyException ex) {
                ModelState.AddModelError(string.Empty,
                    "Unable to save record. Another user updated the record");
            } catch (Exception ex) {
                ModelState.AddModelError(string.Empty, $"Unable to save record: {ex.Message}");
            }
            return View(car);
        }

        // GET: Car/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Car car = await _repo.GetOneAsync(id);

            if (car == null)
                return HttpNotFound();

            return View(car);
        }

        // POST: Car/Delete/5
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete([Bind(Include = "АвтоID,Timestamp")] Car car)
        {
            try {
                await _repo.DeleteAsync(car);
                TempData["message"] = $"Автомобиль удален из БД.";
                return RedirectToAction("Index");

            } catch (DbUpdateConcurrencyException) {
                ModelState.AddModelError(string.Empty,
                    "Unable to delete record. Another user updated the record");
            } catch (Exception ex) {
                ModelState.AddModelError(string.Empty, $"Unable to delete record: {ex.Message}");
            }
            return View(car);
        }

        public async Task<FileResult> GetImage(int автоId)
        {
            Car car = await _repo.GetOneAsync(автоId);
            if (car != null) {
                return new FileContentResult(car.Image, car.MimeType);
            } else
                return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _repo.Dispose(); // для DbContext

            base.Dispose(disposing);  // для Controller
        }
    }
}
