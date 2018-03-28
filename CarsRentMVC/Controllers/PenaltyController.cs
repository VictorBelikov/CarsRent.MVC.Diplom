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
    public class PenaltyController : Controller
    {
        private CarsRentEntities _db = new CarsRentEntities();

        public ActionResult PenaltyListReturnHelp(IEnumerable<Penalty> penaltyList, int pageSize, int page)
        {
            return View("Index", new PenaltyListViewModel {
                Penalties = penaltyList,
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = pageSize
                }
            });
        }

        // GET: Penalty
        public ActionResult Index(string filter = null, int page = 1)
        {
            List<Penalty> penalties = new List<Penalty>();
            penalties = _db.Штрафы.ToList();
            int pageSize = 10;

            return View("Index", new PenaltyListViewModel {
                Penalties = penalties.OrderBy(r => r.СуммаШтрафа).Skip((page - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = penalties.Count
                }
            });
        }

        [HttpPost]
        public ActionResult Index(string filter = null)
        {
            List<Penalty> penalties = new List<Penalty>();
            penalties = _db.Штрафы.ToList();
            int pageSize = 10;
            int page = 1;

            IEnumerable<Penalty> penaltyList;

            if (filter == "") {
                return View("Index", new PenaltyListViewModel {
                    Penalties = penalties.OrderBy(r => r.СуммаШтрафа).Skip((page - 1) * pageSize).Take(pageSize),
                    PagingInfo = new PagingInfo {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = penalties.Count
                    }
                });
            }

            penaltyList = penalties.Where(x => x.НаименованиеШтрафа.ToLower().Contains(filter.ToLower()));
            if (penaltyList.Count() != 0)
                return PenaltyListReturnHelp(penaltyList, pageSize, page);

            penaltyList = penalties.Where(x => x.СуммаШтрафа.ToString().ToLower().Contains(filter.ToLower()));
            if (penaltyList.Count() != 0)
                return PenaltyListReturnHelp(penaltyList, pageSize, page);

            return PenaltyListReturnHelp(penaltyList, pageSize, page);
        }

        // GET: Penalty/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Penalty penalty = await _db.Штрафы.FindAsync(id);
            if (penalty == null) {
                return HttpNotFound();
            }
            return View(penalty);
        }

        // GET: Penalty/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Penalty/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "НаименованиеШтрафа,СуммаШтрафа")] Penalty penalty)
        {
            if (ModelState.IsValid) {
                if (penalty.СуммаШтрафа <= 0) {
                    ModelState.AddModelError(string.Empty,
                        @"Сумма штрафа не может быть отрицательной либо равныой 0.");
                    return View(penalty);
                }
                _db.Штрафы.Add(penalty);
                await _db.SaveChangesAsync();
                TempData["message"] = $"Штраф \"{penalty.НаименованиеШтрафа}\" сохранен в БД.";
                return RedirectToAction("Index");
            }

            return View(penalty);
        }

        // GET: Penalty/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Penalty penalty = await _db.Штрафы.FindAsync(id);
            if (penalty == null) {
                return HttpNotFound();
            }
            return View(penalty);
        }

        // POST: Penalty/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ШтрафID,НаименованиеШтрафа,СуммаШтрафа,Timestamp")] Penalty penalty)
        {
            if (ModelState.IsValid) {
                if (penalty.СуммаШтрафа <= 0) {
                    ModelState.AddModelError(string.Empty,
                        @"Сумма штрафа не может быть отрицательной либо равныой 0.");
                    return View(penalty);
                }
                _db.Entry(penalty).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                TempData["message"] = $"Штраф \"{penalty.НаименованиеШтрафа}\" сохранен в БД.";
                return RedirectToAction("Index");
            }
            return View(penalty);
        }

        // GET: Penalty/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Penalty penalty = await _db.Штрафы.FindAsync(id);
            if (penalty == null) {
                return HttpNotFound();
            }
            return View(penalty);
        }

        // POST: Penalty/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Penalty penalty = await _db.Штрафы.FindAsync(id);
            _db.Штрафы.Remove(penalty);
            await _db.SaveChangesAsync();
            TempData["message"] = $"Штраф \"{penalty.НаименованиеШтрафа}\" удален из БД.";
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
