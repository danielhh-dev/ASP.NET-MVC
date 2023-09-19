using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PROYECTOMVC.Models;

public class HomeController:Controller{

  private readonly ILogger<HomeController>_logger;
  
  public (ILogger<HomeController>logger){
    _logger=logger;
  }

  public IActionResult Index(){
    return View();
  }

  public IActionResult Privacy(){
    return View();
  }

  public IActionResult AboutUs(){
    return View();
  }

  [ResponseCache(Duration = 0,Location = ResponseCacheLocationNone, NoStore= true)]
  public IActionResult Error(){
    return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
  }
}