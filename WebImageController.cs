using DAL;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace News.Controllers
{
    public class WebImageController : Controller
    {
        // GET: Home  
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] files)
        {
            ViewBag.files = fileUpload(files);
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase[] files)
        {
            ViewBag.files = fileUpload(files);
            return View();
        }

        [HttpPost]
        public JsonResult UploadJson(HttpPostedFileBase[] files)
        {
            return Json(fileUpload(files).Count>0? "Başarılı.":"Başarısız.");
        }
        private List<string> fileUpload(HttpPostedFileBase[] files)
        {
            string Name = "";
            string filePath = "";
            ContentRepository ctx = new ContentRepository();
            List<string> names = new List<string>();
            if (ModelState.IsValid && files != null)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        Name = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        filePath = Path.Combine(Server.MapPath("~/Images"), Name);

                        names.Add(Name);
                        ctx.UploadImageInDataBase(file, new DAL.Image() { Name = Name, FileUrl = filePath });
                        file.SaveAs(filePath);
                        ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                    }
                }
            }
            return names;
        }

        public ActionResult List()
        {
            ContentRepository ctx = new ContentRepository();
            return View(ctx.List());
        }
        [HttpPost]
        public ActionResult List( int id)
        {
            ContentRepository ctx = new ContentRepository();
            DAL.Image ımage = new DAL.Image();
            if (id != 0)
            {
                ımage = ctx.GetId(id);
                string filePath = Path.Combine(Server.MapPath("~/Images"), ımage.Name);

                if (System.IO.File.Exists(ımage.FileUrl))
                {
                    System.IO.File.Delete(ımage.FileUrl);
                    ctx.Delete(ımage.Id);
                }
            }
            return View(ctx.List());
        }
    }
}