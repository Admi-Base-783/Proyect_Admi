using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class ABDController : Controller
    {
        // GET: ABD
        public ActionResult Opciones()
        {
            return View();
        }
        public ActionResult CrearTabla()
        {
            return View();
        }
        public ActionResult CrearRelaciones()
        {
            return View();
        }
        public ActionResult EjecutarSentencias()
        {
            return View();
        }
    }
}