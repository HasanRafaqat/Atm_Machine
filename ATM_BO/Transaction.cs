using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_BO
{
    public class Transaction
    {
        public const int deposit = 0;
        public const int withdraw = 1;
        public const int transfer = 2;

        private decimal amount;
        private int transactiontype;
        public decimal Amount => this.amount;

        public int TransactionType => this.transactiontype;

        public Transaction(decimal amount, int transactiontype)
        {
            this.amount = amount;
            this.transactiontype = transactiontype;
        }
    }
}
