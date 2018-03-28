using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CarsRentEF.EF;
using CarsRentEF.Models;
using CarsRentMVC.Models.ViewModels;

namespace CarsRentMVC.Controllers
{
    [Authorize(Roles = "manager")]
    public class ReportController : Controller
    {
        private CarsRentEntities _db = new CarsRentEntities();

        public ActionResult ReportListReturnHelp(IEnumerable<Report> reportList, int pageSize, int page)
        {
            return View("Index", new ReportListViewModel() {
                Reports = reportList,
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = pageSize
                }
            });
        }

        // GET: Report
        public ActionResult Index(string filter = null, int page = 1)
        {
            List<Report> reports = new List<Report>();
            reports = _db.Reports.ToList();
            int pageSize = 10;

            return View("Index", new ReportListViewModel {
                Reports = reports.OrderByDescending(r => r.DateOfFormation).Skip((page - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = reports.Count
                }
            });
        }

        [HttpPost]
        public ActionResult Index(string filter = null)
        {
            List<Report> reports = new List<Report>();
            reports = _db.Reports.ToList();
            int pageSize = 10;
            int page = 1;

            IEnumerable<Report> reportList;

            if (filter == "") {
                return View("Index", new ReportListViewModel {
                    Reports = reports.OrderByDescending(r => r.DateOfFormation).Skip((page - 1) * pageSize).Take(pageSize),
                    PagingInfo = new PagingInfo {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = reports.Count
                    }
                });
            }

            reportList = reports.OrderByDescending(r => r.DateOfFormation).Where(x => x.ShortName.ToLower().Contains(filter.ToLower()));
            if (reportList.Count() != 0)
                return ReportListReturnHelp(reportList, pageSize, page);

            reportList = reports.OrderByDescending(r => r.DateOfFormation).Where(x => x.DateOfFormation.ToShortDateString().Contains(filter.ToLower()));
            if (reportList.Count() != 0)
                return ReportListReturnHelp(reportList, pageSize, page);

            reportList = reports.OrderByDescending(r => r.DateOfFormation).Where(x => x.StartDate.ToShortDateString().Contains(filter.ToLower()));
            if (reportList.Count() != 0)
                return ReportListReturnHelp(reportList, pageSize, page);

            reportList = reports.OrderByDescending(r => r.DateOfFormation).Where(x => x.EndDate.ToShortDateString().Contains(filter.ToLower()));
            if (reportList.Count() != 0)
                return ReportListReturnHelp(reportList, pageSize, page);

            return ReportListReturnHelp(reportList, pageSize, page);
        }

        // GET: Report/Create
        public ActionResult Create()
        {
            return View(new Report {
                DateOfFormation = DateTime.Now,
                StartDate = new DateTime(2017, 12, 1),
                EndDate = DateTime.Now,
                ShortName = "Тестовый отчет"
            });
        }

        // POST: Report/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Report report)
        {
            if (ModelState.IsValid) {
                if (report.EndDate > DateTime.Now) {
                    ModelState.AddModelError(string.Empty,
                        @"Конец периода формируемого отчета не может быть больше текущей даты");
                    return View(report);
                }
                _db.Reports.Add(report);
                await _db.SaveChangesAsync();
                TempData["message"] = $"Отчет с ID={report.ReportId} сохранен в БД.";
                return RedirectToAction("Details", new { id = report.ReportId });
            }

            return View(report);
        }

        // GET: Report/Edit/
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = await _db.Reports.FindAsync(id);
            if (report == null) {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Report/Edit/
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Report report)
        {
            if (ModelState.IsValid) {
                if (report.EndDate > DateTime.Now) {
                    ModelState.AddModelError(string.Empty,
                        @"Конец периода формируемого отчета не может быть больше текущей даты");
                    return View(report);
                }
                _db.Entry(report).State = EntityState.Modified;
                TempData["message"] = $"Отчет с ID={report.ReportId} сохранен в БД.";
                await _db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = report.ReportId });
            }
            return View(report);
        }

        // GET: Report/Delete/
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = await _db.Reports.FindAsync(id);
            if (report == null) {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Report/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Report report = await _db.Reports.FindAsync(id);
            _db.Reports.Remove(report);
            await _db.SaveChangesAsync();
            TempData["message"] = $"Отчет \"{report.ShortName}\" удален из БД.";
            return RedirectToAction("Index");
        }

        // GET: Report/Details
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Report report = await _db.Reports.FindAsync(id);
            if (report == null) {
                return HttpNotFound();
            }

            IEnumerable<Car> cars = _db.Автомобили;
            IEnumerable<Option> options= _db.Options;
            IEnumerable<Discount> discounts = _db.Скидки;
            IEnumerable<Rent> rents = _db.Аренды
                .Where(r => r.ДатаВыдачи >= report.StartDate && r.ДатаВыдачи <= report.EndDate);
            
            double fullProfit = 0;
            int fullPercentDiscount = 0;

            foreach (Rent rent in rents) {
                foreach (Discount disc in discounts)
                    if (rent.Скидки.Contains(disc.НаименованиеСкидки))
                        fullPercentDiscount += disc.Процент;

                fullProfit += rent.ИтоговаяСумма;
                rent.Car.PeriodDays += (rent.ДатаВозврата - rent.ДатаВыдачи).Days;
                rent.Car.PeriodProfit += (rent.ДатаВозврата - rent.ДатаВыдачи).Days * rent.Car.СтоимостПрокатаВСут;

                foreach (Option opt in options) {
                    if (rent.ДопОборудование.Contains(opt.Name)) {
                        opt.PeriodDays += (rent.ДатаВозврата - rent.ДатаВыдачи).Days;
                        opt.PeriodProfit += (rent.ДатаВозврата - rent.ДатаВыдачи).Days * opt.RentDayCost;
                    }
                    opt.PeriodProfit -= (opt.PeriodProfit * fullPercentDiscount) / 100;
                }

                rent.Car.PeriodProfit -= (rent.Car.PeriodProfit * fullPercentDiscount) / 100;
                fullPercentDiscount = 0;
            }

            int days = (report.EndDate - report.StartDate).Days;

            return View(new ReportDetailsViewModel {
                Report = report,
                Days = days,
                FullProfit = fullProfit,
                Rents = rents,
                Cars = cars,
                Options = options
            });
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