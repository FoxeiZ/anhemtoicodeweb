using System.Collections.Generic;

namespace anhemtoicodeweb.Enums
{
    public class State : CustomEnum
    {
        public int _compareValue;

        public State(string name, int compareValue) : base(name)
        {
            _compareValue = compareValue;
        }

        public State(string name, string value, int compareValue) : base(name, value)
        {
            _compareValue = compareValue;
        }
    }

    public static class OrderState
    {
        public static readonly State New = new State("New", "Mới", 0);
        public static readonly State Processing = new State("Processing", "Đang xử lý", 1);
        public static readonly State Pending = new State("Pending", "Đang chờ", 1);
        public static readonly State OnHold = new State("OnHold", "Đang giữ", 1);
        public static readonly State WaitingForPayment = new State("WaitingForPayment", "Đang chờ thanh toán", 2);
        public static readonly State PaymentReceived = new State("PaymentReceived", "Đã nhận thanh toán", 3);
        public static readonly State Shipping = new State("Shipping", "Đang giao hàng", 4);
        public static readonly State Completed = new State("Completed", "Hoàn thành", 5);
        public static readonly State Refunded = new State("Refunded", "Đã hoàn tiền", 5);
        public static readonly State Failed = new State("Failed", "Thất bại", 5);
        public static readonly State Canceled = new State("Canceled", "Đã hủy", 6);
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

        public static bool CanMoveToState(State fromState, State toState)
        {
            return fromState._compareValue <= toState._compareValue;
        }

        public static bool CanMoveToState(string fromStateStr, string toStateStr)
        {
            var fromState = FindByName(fromStateStr) ?? FindByValue(fromStateStr);
            var toState = FindByName(toStateStr) ?? FindByValue(toStateStr);
            if (fromState == null || toState == null)
            {
                return false;
            }

            return CanMoveToState(fromState, toState);
        }

        public static State FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return New;
            }

            foreach (var state in AllState)
            {
                if (state.Name.ToLower() == name.ToLower())
                {
                    return state;
                }
            }
            return New;
        }

        public static State FindByValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return New;
            }

            foreach (var state in AllState)
            {
                if (state.Value.ToLower() == value.ToLower())
                {
                    return state;
                }
            }
            return New;
        }
    }
}