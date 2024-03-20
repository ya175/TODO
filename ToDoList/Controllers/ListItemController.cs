using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ListItemController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public IActionResult Index()
        {

            return View();
        }


        public IActionResult Items()
        {

            ViewData["username"] = Request.Cookies["username"];

            return View(context.ListItems.ToList());
        }

        //create

        public IActionResult CreateView ()=>View();
        public IActionResult Create(string? Name)
        {

            //save Nameto cookie
            CookieOptions cookieOptions = new CookieOptions();

            cookieOptions.Expires = DateTimeOffset.Now.AddDays(1);

            Response.Cookies.Append("username", Name, cookieOptions);

            return RedirectToAction("Items");

        }


        //Create  items
        public IActionResult CreateItemView() => View();
        public IActionResult CreateItem(string Title, string Descreption, DateTime Deadline)
        {

            context.ListItems.Add(new ListItem() { Title = Title, Descreption = Descreption, Deadline = Deadline });

            context.SaveChanges();


            ViewData["Title"]=Title;
            return View(context.ListItems.ToList());

        }


        //Edit
        public IActionResult EditView(int Id) {


            ListItem item = context.ListItems.First(e => e.Id == Id);

            return View(item);

         }
        public IActionResult Edit(int Id, string Title, string Descreption, DateTime Deadline)
        {
            context.ListItems.Where(e => e.Id == Id).ExecuteUpdate(b => b.SetProperty(u => u.Title, Title).SetProperty(u=>u.Descreption,Descreption).SetProperty(u=>u.Deadline,Deadline));
            context.SaveChanges();

            return View("Items",context.ListItems.ToList());
        }


        ////Delete
        ////public IActionResult DeleteView() => View();


        public IActionResult Delete(int Id)
        {
            context.ListItems.Where(e => e.Id == Id).ExecuteDelete();

            return View("Items", context.ListItems.ToList());
        }


    }
}
