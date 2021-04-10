using BOOKSTORE.INFRASTRUCTURE.Interfaces;
using BOOKSTORE.INFRASTRUCTURE.Services.BookServices.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BOOKSTORE.UI.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        // GET: Book
        public ActionResult Index()
        {
            var model = new BookVM();
            ViewBag.PageUrl = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(model);
        }

        public async Task<PartialViewResult> List(string searchBy = null)
        {
            var list = await _bookService.GetAllSQL(searchBy);
            ViewBag.PageUrl = this.ControllerContext.RouteData.Values["controller"].ToString();
            return PartialView(list);
        }


        public ActionResult New() {
            var model = new BookVM();
            ViewBag.PageUrl = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View("NewOrEdit", model);

        }
        public async Task<ActionResult> Edit(long id)
        {
            var entity = await _bookService.GetByIdSQL(id);
            ViewBag.PageUrl = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View("NewOrEdit", entity);
        }


        [HttpPost]
        public async Task<JsonResult> Save(BookVM model)
        {
            if (model.Id > 0)
            {
                var response = await _bookService.InsertOrUpdateSQL(model);
                return Json(response);
            }
            else
            {
                var response = await _bookService.InsertOrUpdateSQL(model);
                return Json(response);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            var response = await _bookService.DeleteSQL(id);
            ViewBag.PageUrl = this.ControllerContext.RouteData.Values["controller"].ToString();
            return Json(new { Result = response.Result, Message = response.Message, Id = response.Id });
        }

    }
}