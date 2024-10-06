using BookManagement.Models;
using BookManagement.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManagement.Controllers
{
    public class CustomerController : Controller
    {
        private BookDBEntities _db =new BookDBEntities();
        public ActionResult Index()
        {
            var customers=_db.Customers.Include(d=>d.BookEntries.Select(e=>e.Book)).OrderByDescending(e => e.CustomerId).ToList();
            return View(customers);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult AddNewBook(int?id)
        {
            ViewBag.Books=new SelectList(_db.Books.ToList(),"BookId","BookName",(id!=null)?id.ToString():"");
            return PartialView("_AddNewBook");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerViewModel vObj, int[] bookId)
        {
            if(ModelState.IsValid)
            {
                Customer customer = new Customer()
                {
                    CustomerName = vObj.CustomerName,
                    BirthDate = vObj.BirthDate,
                    IsRegular = vObj.IsRegular,

                };
                HttpPostedFileBase file = vObj.PicturePath;
                string filepath=Path.Combine("/images/",Guid.NewGuid().ToString()+Path.GetExtension(file.FileName));
                file.SaveAs(Server.MapPath(filepath));
                customer.Picture = filepath;

                foreach(var item in bookId)
                {
                    BookEntry bo = new BookEntry()
                    {
                        Customer = customer,
                        CustomerId = customer.CustomerId,
                        BookId = item
                    };
                    _db.BookEntries.Add(bo);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Customer customer=_db.Customers.First(x=>x.CustomerId == id);
            var books=_db.BookEntries.Where(x=>x.CustomerId==id).ToList();
            CustomerViewModel vObj = new CustomerViewModel()
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                BirthDate = customer.BirthDate,
                IsRegular = customer.IsRegular,
                Picture = customer.Picture,
            };
            if (books.Any())
            {
                foreach(var item in books)
                {
                    vObj.BookList.Add(item.BookId);
                }

            }
            return View(vObj);
        }
       [HttpPost]
        public ActionResult Edit(CustomerViewModel vObj, int[] bookId)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer()
                {
                    CustomerName = vObj.CustomerName,
                    BirthDate = vObj.BirthDate,
                    IsRegular = vObj.IsRegular,
                    CustomerId = vObj.CustomerId

                };
                HttpPostedFileBase file = vObj.PicturePath;
                if(file!=null)
                {
                    string filepath = Path.Combine("/images/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filepath));
                    customer.Picture = filepath;
                }
                else
                {
                    customer.Picture = vObj.Picture;
                }
                var existingBook = _db.BookEntries.Where(x => x.CustomerId == customer.CustomerId).ToList();
                foreach(var item in existingBook)
                {
                    _db.BookEntries.Remove(item);
                }
                foreach (var item in bookId)
                {
                    BookEntry bo = new BookEntry()
                    {
                        
                        CustomerId = customer.CustomerId,
                        BookId = item
                    };
                    
                    _db.BookEntries.Add(bo);
                }
                _db.Entry(customer).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Delete(int? id)
        {
            var bmp = _db.Customers.Find(id);
            var exisBook=_db.BookEntries.Where(e=>e.CustomerId==id).ToList();
            foreach (var item in exisBook)
            {
                _db.BookEntries.Remove(item);
            }
            _db.Entry(bmp).State = EntityState.Deleted;
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}