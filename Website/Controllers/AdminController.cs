using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Models;
using Website.Models.Context;


namespace Website.Controllers
{
    public class AdminController : Controller
    {
        private ContextNews db;


        public AdminController(ContextNews context)
        {
            db = context;
        }


        public async Task<IActionResult> Index(string adm)
        {
            if (adm == "Hfass454")
            {
                return View(await db.News.ToListAsync());
            }
            return View();
        }

        public ViewResult Create()
        {
            return View("Edit", new News());
        }

        public ViewResult Edit(int Id)
        {
            News news = db.News
                .FirstOrDefault(g => g.Id == Id);
            return View(news);
        }

        [HttpPost]
        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                Save(news);
                TempData["message"] = string.Format("Изменения в новости \"{0}\" были сохранены", news.Title);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(news);
            }
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            News deleted = DeleteNews(Id);
            if (deleted != null)
            {
                TempData["message"] = string.Format("Новость \"{0}\" была удалена",
                    deleted.Title);
            }
            return RedirectToAction("Index");
        }

        public void Save(News news)
        {
            if (news.Id == 0)
                db.Add(news);
            else
            {
                News dbEntry = db.News.Find(news.Id);
                if (dbEntry != null)
                {
                    dbEntry.Title = news.Title;
                    dbEntry.Name = news.Name;
                    dbEntry.Images = news.Images;
                    dbEntry.Organization = news.Organization;
                    dbEntry.Position = news.Position;
                    dbEntry.DateStart = news.DateStart;
                    dbEntry.DateFinish = news.DateFinish;
                }
            }
            db.SaveChanges();
        }

        public News DeleteNews(int Id)
        {
            News dbEntry = db.News.Find(Id);
            if (dbEntry != null)
            {
                db.News.Remove(dbEntry);
                db.SaveChanges();
            }
            return dbEntry;
        }
    }
}