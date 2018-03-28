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
    public class ClientController : Controller
    {
        private readonly ClientRepo _repo = new ClientRepo();

        public ActionResult ClientListReturnHelp(IEnumerable<Client> clientList, int pageSize, int page)
        {
            return View("Index", new ClientListViewModel {
                Clients = clientList,
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = pageSize
                }
            });
        }

        // GET: Client
        public ActionResult Index(string filter = null, int page = 1)
        {
            List<Client> clients = new List<Client>();
            clients = _repo.GetAll();
            int pageSize = 10;

            return View("Index", new ClientListViewModel {
                Clients = clients.OrderBy(r => r.ФИО).Skip((page - 1) * pageSize).Take(pageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = clients.Count
                }
            });
        }

        [HttpPost]
        public ActionResult Index(string filter = null)
        {
            List<Client> clients = new List<Client>();
            clients = _repo.GetAll();
            int pageSize = 10;
            int page = 1;

            IEnumerable<Client> clientList;

            if (filter == "") {
                return View("Index", new ClientListViewModel {
                    Clients = clients.OrderBy(r => r.ФИО).Skip((page - 1) * pageSize).Take(pageSize),
                    PagingInfo = new PagingInfo {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = clients.Count
                    }
                });
            }

            clientList = clients.OrderBy(r => r.ФИО).Where(x => x.ФИО.ToLower().Contains(filter.ToLower()));
            if (clientList.Count() != 0)
                return ClientListReturnHelp(clientList, pageSize, page);

            clientList = clients.OrderBy(r => r.ФИО).Where(x => x.Адрес.ToLower().Contains(filter.ToLower()));
            if (clientList.Count() != 0)
                return ClientListReturnHelp(clientList, pageSize, page);

            clientList = clients.OrderBy(r => r.ФИО).Where(x => x.Телефон.ToLower().Contains(filter.ToLower()));
            if (clientList.Count() != 0)
                return ClientListReturnHelp(clientList, pageSize, page);

            clientList = clients.OrderBy(r => r.ФИО).Where(x => x.НомерВодУд.ToLower().Contains(filter.ToLower()));
            if (clientList.Count() != 0)
                return ClientListReturnHelp(clientList, pageSize, page);

            clientList = clients.OrderBy(r => r.ФИО).Where(x => x.НомерПаспорта.ToLower().Contains(filter.ToLower()));
            if (clientList.Count() != 0)
                return ClientListReturnHelp(clientList, pageSize, page);

            return ClientListReturnHelp(clientList, pageSize, page);
        }

        // GET: Client/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Client client = await _repo.GetOneAsync(id);

            if (client == null)
                return HttpNotFound();

            // если имя представления в методе не указано, то вернуть одноименное методу действия
            return View(client);
        }

        // GET: Client/Create
        public ActionResult Create()
        {
            ViewBag.BlackList = new SelectList(new List<Status> {
                new Status {Name = "Нет", Value = "Нет"},
                new Status {Name = "Да", Value = "Да"}
            }, "Value", "Name");

            return View(new Client {
                ЧерныйСписок = "Нет"
            });
        }

        // POST: Client/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Client client, string blacklist, HttpPostedFileBase imageUpload = null)
        {
            ViewBag.BlackList = new SelectList(new List<Status> {
                new Status {Name = "Нет", Value = "Нет"},
                new Status {Name = "Да", Value = "Да"}
            }, "Value", "Name");

            if (!ModelState.IsValid) {
                ModelState.AddModelError(string.Empty,
                    @"Ошибка в данных. Пожалуйста проверьте все поля.");
                return View(client);
            }

            client.ЧерныйСписок = blacklist;
            if (client.ЧерныйСписок.ToLower() != "нет") {
                ModelState.AddModelError(string.Empty, @"Новый клиент не может находиться в черном списке.");
                return View(client);
            }

            if (imageUpload != null) {
                var count = imageUpload.ContentLength;
                client.Image = new byte[count];
                imageUpload.InputStream.Read(client.Image, 0, (int)count);
                client.MimeType = imageUpload.ContentType;
            }

            try {
                await _repo.AddAsync(client);
                TempData["message"] = $"Клиент {client.ФИО} сохранен в БД.";
                return RedirectToAction("Details", new {id = client.КлиентID});

            } catch (Exception ex) {
                ModelState.AddModelError(string.Empty, $"Невозможно сохранить объект в БД: {ex.Message}");
                return View(client);
            }
        }

        // GET: Client/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Client client = await _repo.GetOneAsync(id);

            if (client == null)
                return HttpNotFound();

            ViewBag.BlackList = new SelectList(new List<Status> {
                new Status {Name = "Нет", Value = "Нет"},
                new Status {Name = "Да", Value = "Да"}
            }, "Value", "Name");

            return View(client);
        }

        // POST: Client/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Client client, string blacklist, HttpPostedFileBase imageUpload = null)
        {
            ViewBag.BlackList = new SelectList(new List<Status> {
                new Status {Name = "Нет", Value = "Нет"},
                new Status {Name = "Да", Value = "Да"}
            }, "Value", "Name");

            if (!ModelState.IsValid)
                return View(client);

            if (imageUpload != null) {
                var count = imageUpload.ContentLength;
                client.Image = new byte[count];
                imageUpload.InputStream.Read(client.Image, 0, (int)count);
                client.MimeType = imageUpload.ContentType;
            }
            
            client.ЧерныйСписок = blacklist;

            try {
                await _repo.SaveAsync(client);
                TempData["message"] = $"Клиент {client.ФИО} сохранен в БД.";
                return RedirectToAction("Details", new {id = client.КлиентID});

            } catch (DbUpdateConcurrencyException ex) {
                ModelState.AddModelError(string.Empty,
                    "Unable to save record. Another user updated the record");
            } catch (Exception ex) {
                ModelState.AddModelError(string.Empty, $"Невозможно сохранить объект в БД: {ex.Message}");
            }
            return View(client);
        }

        // GET: Client/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Client client = await _repo.GetOneAsync(id);

            if (client == null)
                return HttpNotFound();

            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind(Include = "КлиентID,Timestamp")] Client client)
        {
            try {
                await _repo.DeleteAsync(client);
                TempData["message"] = $"Клиент удален из БД.";
                return RedirectToAction("Index");

            } catch (DbUpdateConcurrencyException) {
                ModelState.AddModelError(string.Empty,
                    "Unable to delete record. Another user updated the record");
            } catch (Exception ex) {
                ModelState.AddModelError(string.Empty, $"Unable to delete record: {ex.Message}");
            }
            return View(client);
        }

        public async Task<FileResult> GetImage(int клиентId)
        {
            Client client = await _repo.GetOneAsync(клиентId);
            if (client != null) {
                return new FileContentResult(client.Image, client.MimeType);
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
