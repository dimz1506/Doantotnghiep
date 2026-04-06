using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Doantotnghiep.Models.Entities
{
public class TaiKhoan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTaiKhoan { get; set; }

        [Required]
        public string TenTaiKhoan { get; set; } = string.Empty;

        [Required]
        public string Passwordhash { get; set; } = string.Empty;

        public string? Salt { get; set; } = string.Empty;

        [Required]
        public int IdVaiTro { get; set; }

        [ForeignKey("IdVaiTro")]
        public VaiTro? VaiTro { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        public string SoDienThoai { get; set; } = string.Empty;

        [Required]
        public bool TrangThaiTK { get; set; }

        [Required]
        public DateTime NgayTaoTaiKhoan { get; set; } = DateTime.Now;

        public DateTime? NgayXoaTaiKhoan { get; set; }

        public bool IsDeleted { get; set; } = false;

        public KhachHang? KhachHang { get; set; }
        public NhanVien? NhanVien { get; set; }
    }
}
