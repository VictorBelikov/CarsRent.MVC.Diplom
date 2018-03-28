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
    [Authorize(Roles = "admin")]
    public class OptionController : Controller
    {
        private CarsRentEntities _db = new CarsRentEntities();

        public ActionResult DiscountListReturnHelp(IEnumerable<Option> optionList, int pageSize, int page)
        {
            return View("Index", new OptionListViewModel() {
                Options = optionList,
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = pageSize
                }
            });
        }

        // GET: Option
        public ActionResult Index(string filter = null, int page = 1)
        {
            List<Option> options = new List<Option>();
            options = _db.Options.ToList();
            int pageSize = 10;

            return View("Index", new OptionListViewModel() {
                Options = options.OrderBy(r => r.RentDayCost).Skip((page - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = options.Count
                }
            });
        }

        [HttpPost]
        public ActionResult Index(string filter = null)
        {
            List<Option> options = new List<Option>();
            options = _db.Options.ToList();
            int pageSize = 10;
            int page = 1;

            IEnumerable<Option> optionList;

            if (filter == "") {
                return View("Index", new OptionListViewModel() {
                    Options = options.OrderBy(r => r.RentDayCost).Skip((page - 1) * pageSize).Take(pageSize),
                    PagingInfo = new PagingInfo {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = options.Count
                    }
                });
            }

            optionList = options.Where(x => x.Name.ToLower().Contains(filter.ToLower()));
            if (optionList.Count() != 0)
                return DiscountListReturnHelp(optionList, pageSize, page);

            optionList = options.Where(x => x.InvNumber.ToString().ToLower().Contains(filter.ToLower()));
            if (optionList.Count() != 0)
                return DiscountListReturnHelp(optionList, pageSize, page);

            optionList = options.Where(x => x.RentDayCost.ToString().ToLower().Contains(filter.ToLower()));
            if (optionList.Count() != 0)
                return DiscountListReturnHelp(optionList, pageSize, page);

            optionList = options.Where(x => x.FullCost.ToString().ToLower().Contains(filter.ToLower()));
            if (optionList.Count() != 0)
                return DiscountListReturnHelp(optionList, pageSize, page);

            return DiscountListReturnHelp(optionList, pageSize, page);
        }

        // GET: Option/Details
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Option option = await _db.Options.FindAsync(id);
            if (option == null) {
                return HttpNotFound();
            }
            return View(option);
        }

        // GET: Option/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Option/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Option option)
        {
            if (ModelState.IsValid) {
                if (option.RentDayCost <= 0) {
                    ModelState.AddModelError(string.Empty,
                        @"Стоимость не может быть отрицательной либо равной 0.");
                    return View(option);
                }

                _db.Options.Add(option);
                await _db.SaveChangesAsync();
                TempData["message"] = $"Опция \"{option.Name}\" сохранена в БД.";
                return RedirectToAction("Index");
            }

            return View(option);
        }

        // GET: Option/Edit/
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Option option = await _db.Options.FindAsync(id);
            if (option == null) {
                return HttpNotFound();
            }
            return View(option);
        }

        // POST: Option/Edit/
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Option option)
        {
            if (ModelState.IsValid) {
                if (option.RentDayCost <= 0) {
                    ModelState.AddModelError(string.Empty,
                        @"Стоимость не может быть отрицательной либо равной 0.");
                    return View(option);
                }

                _db.Entry(option).State = EntityState.Modified;
                TempData["message"] = $"Опция \"{option.Name}\" сохранена в БД.";
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(option);
        }

        // GET: Option/Delete/
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Option option = await _db.Options.FindAsync(id);
            if (option == null) {
                return HttpNotFound();
            }
            return View(option);
        }

        // POST: Option/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Option option = await _db.Options.FindAsync(id);
            _db.Options.Remove(option);
            await _db.SaveChangesAsync();
            TempData["message"] = $"Опция \"{option.Name}\" удалена из БД.";
            return RedirectToAction("Index");
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
