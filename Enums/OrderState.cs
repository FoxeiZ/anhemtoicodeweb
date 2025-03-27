using System.Collections.Generic;

namespace anhemtoicodeweb.Enums
{
    public class State : CustomEnum
    {
        public State(string name) : base(name) { }

        public State(string name, string value) : base(name, value) { }
    }

    public static class OrderState
    {
        public static readonly State New = new State("New", "Mới");
        public static readonly State Processing = new State("Processing", "Đang xử lý");
        public static readonly State Shipping = new State("Shipping", "Đang giao hàng");
        public static readonly State Completed = new State("Completed", "Hoàn thành");
        public static readonly State Canceled = new State("Canceled", "Đã hủy");
        public static readonly State Refunded = new State("Refunded", "Đã hoàn tiền");
        public static readonly State Failed = new State("Failed", "Thất bại");
        public static readonly State Pending = new State("Pending", "Đang chờ");
        public static readonly State OnHold = new State("OnHold", "Đang giữ");
        public static readonly State WaitingForPayment = new State("WaitingForPayment", "Đang chờ thanh toán");
        public static readonly State PaymentReceived = new State("PaymentReceived", "Đã nhận thanh toán");
        public static readonly IReadOnlyList<State> AllState = new List<State> {
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

        public static State FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            foreach (var state in AllState)
            {
                if (state.Name.ToLower() == name.ToLower())
                {
                    return state;
                }
            }
            return null;
        }
    }
}