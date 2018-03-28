using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using CarsRentEF.EF;
using CarsRentEF.Models;
using CarsRentMVC.Models.ViewModels;

namespace CarsRentMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class DiscountController : Controller
    {
        private CarsRentEntities _db = new CarsRentEntities();

        public ActionResult DiscountListReturnHelp(IEnumerable<Discount> discountList, int pageSize, int page)
        {
            return View("Index", new DiscountListViewModel() {
                Discounts = discountList,
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = pageSize
                }
            });
        }

        // GET: Discount
        public ActionResult Index(string filter = null, int page = 1)
        {
            List<Discount> discounts = new List<Discount>();
            discounts = _db.Скидки.ToList();
            int pageSize = 10;

            return View("Index", new DiscountListViewModel() {
                Discounts = discounts.OrderBy(r => r.Процент).Skip((page - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = discounts.Count
                }
            });
        }

        [HttpPost]
        public ActionResult Index(string filter = null)
        {
            List<Discount> discounts = new List<Discount>();
            discounts = _db.Скидки.ToList();
            int pageSize = 10;
            int page = 1;

            IEnumerable<Discount> discountList;

            if (filter == "") {
                return View("Index", new DiscountListViewModel() {
                    Discounts = discounts.OrderBy(r => r.Процент).Skip((page - 1) * pageSize).Take(pageSize),
                    PagingInfo = new PagingInfo {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = discounts.Count
                    }
                });
            }

            discountList = discounts.Where(x => x.НаименованиеСкидки.ToLower().Contains(filter.ToLower()));
            if (discountList.Count() != 0)
                return DiscountListReturnHelp(discountList, pageSize, page);

            discountList = discounts.Where(x => x.Процент.ToString().ToLower().Contains(filter.ToLower()));
            if (discountList.Count() != 0)
                return DiscountListReturnHelp(discountList, pageSize, page);

            return DiscountListReturnHelp(discountList, pageSize, page);
        }

        // GET: Discount/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = await _db.Скидки.FindAsync(id);
            if (discount == null) {
                return HttpNotFound();
            }
            return View(discount);
        }

        // GET: Discount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Discount/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "СкидкаID,НаименованиеСкидки,Процент,Timestamp")] Discount discount)
        {
            if (ModelState.IsValid) {
                if (discount.Процент <= 0) {
                    ModelState.AddModelError(string.Empty,
                        @"Процент у скидки не может быть отрицательным либо равным 0.");
                    return View(discount);
                }

                _db.Скидки.Add(discount);
                await _db.SaveChangesAsync();
                TempData["message"] = $"Скидка \"{discount.НаименованиеСкидки}\" сохранена в БД.";
                return RedirectToAction("Index");
            }

            return View(discount);
        }

        // GET: Discount/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = await _db.Скидки.FindAsync(id);
            if (discount == null) {
                return HttpNotFound();
            }
            return View(discount);
        }

        // POST: Discount/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "СкидкаID,НаименованиеСкидки,Процент,Timestamp")] Discount discount)
        {
            if (ModelState.IsValid) {
                if (discount.Процент <= 0) {
                    ModelState.AddModelError(string.Empty,
                        @"Процент у скидки не может быть отрицательным либо равным 0.");
                    return View(discount);
                }

                _db.Entry(discount).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                TempData["message"] = $"Скидка \"{discount.НаименованиеСкидки}\" сохранена в БД.";
                return RedirectToAction("Index");
            }
            return View(discount);
        }

        // GET: Discount/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = await _db.Скидки.FindAsync(id);
            if (discount == null) {
                return HttpNotFound();
            }
            return View(discount);
        }

        // POST: Discount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Discount discount = await _db.Скидки.FindAsync(id);
            _db.Скидки.Remove(discount);
            await _db.SaveChangesAsync();
            TempData["message"] = $"Скидка \"{discount.НаименованиеСкидки}\" удалена из БД.";
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
