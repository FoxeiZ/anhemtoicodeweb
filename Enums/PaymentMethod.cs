using System.Collections.Generic;

namespace anhemtoicodeweb.Enums
{
    public class PaymentMethod : CustomEnum
    {
        public PaymentMethod(string name) : base(name) { }
        public PaymentMethod(string name, string value) : base(name, value) { }
    }

    public class PaymentMethodEnum
    {
        public static readonly PaymentMethod Cash = new PaymentMethod("Cash", "Tiền mặt");
        public static readonly PaymentMethod BankTransfer = new PaymentMethod("BankTransfer", "Chuyển khoản");
        public static readonly PaymentMethod CreditCard = new PaymentMethod("CreditCard", "Thẻ tín dụng");
        public static readonly PaymentMethod PayPal = new PaymentMethod("PayPal", "PayPal");
        public static readonly PaymentMethod VNPay = new PaymentMethod("VNPay");
        public static readonly IReadOnlyList<PaymentMethod> AllPaymentMethod = new List<PaymentMethod> {
            Cash,
            BankTransfer,
            CreditCard,
            PayPal,
            VNPay
        };

        public static PaymentMethod FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            foreach (var state in AllPaymentMethod)
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