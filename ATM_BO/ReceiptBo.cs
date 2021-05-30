using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_BO
{
    public class ReceiptBo
    {
        public const int ifError = 0;
        public const int transactionLimit = 1;
        public const int notEnoughCash = 2;

        private int error;
       private UserTransactionBo user;
       private Transaction transaction;
       private DateTime date;

        public int Error => this.error;
        public UserTransactionBo User => this.user;
        public Transaction Transaction => this.transaction;
        public DateTime Date => this.date;


        public ReceiptBo(UserTransactionBo user, Transaction transaction, int error = ifError, DateTime? date = null)
        {
            this.user = user;
            this.transaction = transaction;
            this.date = date == null ? DateTime.Now.Date : (DateTime)date;
            this.error = error;
        }

        public string ConvertToExcel()
        {
            return $"{transaction.TransactionType},{user.Id},{user.HolderName},{transaction.Amount},{date.ToString("dd/MM/yyyy")}";
        }

        public string PrintReceipt()
        {
            string result = "";
            if (transaction.TransactionType == Transaction.deposit)
                result = "Deposited";
            if (transaction.TransactionType == Transaction.withdraw)
                result = "Withdrawn";
            if (transaction.TransactionType == Transaction.transfer)
                result = "Amount Transferred";

            return $"Account #{user.Id}\nDate: {date.ToString("dd/MM/yyyy")}\n\n{result}: {transaction.Amount}\nBalance: {user.Assets}";
        }

      

    }
}
