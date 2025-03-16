using System.Collections.Generic;

namespace anhemtoicodeweb.Enums
{
    public static class OrderState
    {
        public const string New = "Mới";
        public const string Processing = "Đang xử lý";
        public const string Shipping = "Đang giao hàng";
        public const string Completed = "Hoàn thành";
        public const string Canceled = "Đã hủy";
        public const string Refunded = "Đã hoàn tiền";
        public const string Failed = "Thất bại";
        public const string Pending = "Đang chờ";
        public const string OnHold = "Đang giữ";
        public const string WaitingForPayment = "Đang chờ thanh toán";
        public const string PaymentReceived = "Đã nhận thanh toán";

        public static List<string> GetAllStates()
        {
            return new List<string>
                {
                    New,
                    Processing,
                    Shipping,
                    Completed,
                    Canceled,
                    Refunded,
                    Failed,
                    Pending,
                    OnHold,
                    WaitingForPayment,
                    PaymentReceived
                };
        }
    }
}