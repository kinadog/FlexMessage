using System.Net.WebSockets;
using System.Text;
using FlexMessage.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}