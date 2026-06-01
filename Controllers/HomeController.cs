using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doantotnghiep.Data;
using Doantotnghiep.Models;
using Doantotnghiep.Models.ViewModel;

namespace Doantotnghiep.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel
        {
            DichVuNoiBats = await _context.DichVus
                .Where(x => x.TrangThaiDV == true && x.IsDeleted == false)
                .OrderByDescending(x => x.IdDichVu)
                .Take(4)
                .ToListAsync()
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}