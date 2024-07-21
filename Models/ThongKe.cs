using anhemtoicodeweb.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace anhemtoicodeweb.Models.ThongKe
{
    public class ChiTiet
    {
        public ChiTiet(int idsp, int? tongSo, decimal? tongThu, string tenSP)
        {
            IDSP = idsp;
            TongSo = tongSo;
            TongThu = tongThu;
            TenSP = tenSP;
        }

        public int IDSP { get; set; }
        public string TenSP { get; set; }
        public int? TongSo { get; set; }
        public string DonViTinh { get; set; }
        public decimal? TongThu { get; set; }
    }

    public class DanhSachChiTiet
    {
        public DanhSachChiTiet() { }
        public DanhSachChiTiet(decimal tongThu, List<ChiTiet> chiTiet)
        {
            TongThu = tongThu;
            ChiTiet = chiTiet;
        }

        public decimal TongThu { get; set; }
        public List<ChiTiet> ChiTiet { get; set; } = new List<ChiTiet>();
    }

    public class ThongKeNgay
    {
        public ThongKeNgay() { }
        public ThongKeNgay(decimal tongDoanhThu, int tongKhacHang, int tongDatHang)
        {
            TongDoanhThu = tongDoanhThu;
            TongKhacHang = tongKhacHang;
            TongDatHang = tongDatHang;

            ChiTietSanPham = new DanhSachChiTiet();
        }

        public decimal TongDoanhThu { get; set; }
        public int TongKhacHang { get; set; }
        public int TongDatHang { get; set; }

        public DanhSachChiTiet ChiTietSanPham { get; set; } = new DanhSachChiTiet();

        public static ThongKeNgay UseDB(Model1 db)
        {
            var self = new ThongKeNgay();

            db.OrderProes.Where(x => x.DateOrder.Day == DateTime.Now.Day).ForEach(item =>
            {
                self.TongDoanhThu += item.TotalMoney;
                self.TongKhacHang += 1;
                db.OrderDetails.Where(x => x.IDOrder == item.ID)
                    .ForEach(x =>
                    {
                        self.TongDatHang += 1;
                        self.ChiTietSanPham.TongThu += x.Total;
                        var isSet = self.ChiTietSanPham.ChiTiet.Where(
                            sp => sp.IDSP == x.IDProduct).FirstOrDefault().IfNotNull(sp =>
                            {
                                sp.TongSo += x.Quantity;
                                sp.TongThu += x.Total;
                                return true;
                            }, false);
                        if (!isSet)
                        {
                            db.Products.Where(
                                sp => sp.ProductID == x.IDProduct).FirstOrDefault().IfNotNull(sp =>
                                {
                                    self.ChiTietSanPham.ChiTiet.Add(
                                        new ChiTiet((int)x.IDProduct, x.Quantity, x.Total, sp.NamePro));
                                });
                        }
                    });
            });
            return self;
        }
    }

    public class ThongKeThang
    {
        public ThongKeThang() { }
        public ThongKeThang(decimal tongDoanhThu, int tongKhacHang, int tongDatHang)
        {
            TongDoanhThu = tongDoanhThu;
            TongKhacHang = tongKhacHang;
            TongDatHang = tongDatHang;

            ChiTietSanPham = new DanhSachChiTiet();
        }

        public decimal TongDoanhThu { get; set; }
        public int TongKhacHang { get; set; }
        public int TongDatHang { get; set; }

        public DanhSachChiTiet ChiTietSanPham { get; set; } = new DanhSachChiTiet();

        public static ThongKeThang UseDB(Model1 db, int? monthGet = null, int? yearGet = null)
        {
            var self = new ThongKeThang();

            int month = DateTime.Now.Month;
            if (monthGet.HasValue)
            {
                month = monthGet.Value;
            }

            int year = DateTime.Now.Year;
            if (yearGet.HasValue)
            {
                year = yearGet.Value;
            }

            db.OrderProes.Where(x => x.DateOrder.Month == month && x.DateOrder.Year == year).ForEach(item =>
            {
                self.TongDoanhThu += item.TotalMoney;
                self.TongKhacHang += 1;
                db.OrderDetails.Where(x => x.IDOrder == item.ID)
                    .ForEach(x =>
                    {
                        self.TongDatHang += 1;
                        self.ChiTietSanPham.TongThu += x.Total;
                        var isSet = self.ChiTietSanPham.ChiTiet.Where(
                            sp => sp.IDSP == x.IDProduct).FirstOrDefault().IfNotNull(sp =>
                            {
                                sp.TongSo += x.Quantity;
                                sp.TongThu += x.Total;
                                return true;
                            }, false);
                        if (!isSet)
                        {
                            db.Products.Where(
                                sp => sp.ProductID == x.IDProduct).FirstOrDefault().IfNotNull(sp =>
                                {
                                    self.ChiTietSanPham.ChiTiet.Add(
                                        new ChiTiet((int)x.IDProduct, x.Quantity, x.Total, sp.NamePro));
                                });
                        }
                    });
            });
            return self;
        }
    }

    public class ThongKeNam
    {
        public ThongKeNam() { }
        public ThongKeNam(decimal tongDoanhThu, int tongKhacHang, int tongDatHang)
        {
            TongDoanhThu = tongDoanhThu;
            TongKhacHang = tongKhacHang;
            TongDatHang = tongDatHang;

            ChiTietSanPham = new DanhSachChiTiet();
        }

        public decimal TongDoanhThu { get; set; }
        public int TongKhacHang { get; set; }
        public int TongDatHang { get; set; }

        public DanhSachChiTiet ChiTietSanPham { get; set; } = new DanhSachChiTiet();

        public static ThongKeNam UseDB(Model1 db, int? yearGet = null)
        {
            var self = new ThongKeNam();

            int year = DateTime.Now.Year;
            if (yearGet.HasValue)
            {
                year = yearGet.Value;
            }

            db.OrderProes.Where(x => x.DateOrder.Year == year).ForEach(item =>
            {
                self.TongDoanhThu += item.TotalMoney;
                self.TongKhacHang += 1;
                db.OrderDetails.Where(x => x.IDOrder == item.ID)
                    .ForEach(x =>
                    {
                        self.TongDatHang += 1;
                        self.ChiTietSanPham.TongThu += x.Total;
                        var isSet = self.ChiTietSanPham.ChiTiet.Where(
                            sp => sp.IDSP == x.IDProduct).FirstOrDefault().IfNotNull(sp =>
                            {
                                sp.TongSo += x.Quantity;
                                sp.TongThu += x.Total;
                                return true;
                            }, false);
                        if (!isSet)
                        {
                            db.Products.Where(
                                sp => sp.ProductID == x.IDProduct).FirstOrDefault().IfNotNull(sp =>
                                {
                                    self.ChiTietSanPham.ChiTiet.Add(
                                        new ChiTiet((int)x.IDProduct, x.Quantity, x.Total, sp.NamePro));
                                });
                        }
                    });
            });
            return self;
        }
    }
}