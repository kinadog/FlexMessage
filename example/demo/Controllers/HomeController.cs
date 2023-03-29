using Demo.Messages;
using Demo.Messages.Types;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {

        return View();
    }
}