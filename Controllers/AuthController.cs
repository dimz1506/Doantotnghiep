using Doantotnghiep.Models.ViewModel;
using Doantotnghiep.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
//danh cho khach hang dang ky
namespace Doantotnghiep.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        //Get
        [HttpGet]
        public IActionResult DangKy()
        {
            if(User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DangKy(RegisterCustomerVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            var result = await _authService.RegisterCustomerAsync(registerVM);
            if (result.Success)
            {
                ViewBag.SuccessMessage = result.Message;
                return View(registerVM);
            }
            return RedirectToAction("DangNhap");
        }
        [HttpGet]
        public IActionResult DangNhap()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM loginVM)
        {
            var user = await _authService.LoginAsync(loginVM);
            if (user == null) {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View(loginVM);
            }
            var roleName = user.IdVaiTro switch
            {
                1 => "Admin",
                2 => "NhanVien",
                3 => "KhachHang",
                _ => "KhachHang"
            };
            var claims = new List<Claim>()
           {
               new Claim(ClaimTypes.NameIdentifier, user.IdTaiKhoan.ToString()),
                new Claim(ClaimTypes.Name, user.TenTaiKhoan),
                new Claim(ClaimTypes.Role, roleName)
           };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("DangNhap");
        }
    }
}
