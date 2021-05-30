using ATM_BO;
using ATM_DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_BLL
{
    public class AtmBll
    {

        int attempts = 0;
        UsernameBo loggedInUser = null; // The user who is currently logged in
        AtmDal dataController = new AtmDal();

        public bool IsUser(UsernameBo username)
        {
            UserTransactionBo user = dataController.SearchByUsername(username);
            return user != null ? true : false;
        }

        public UserTransactionBo GetLoggedInUser()
        {
            return (loggedInUser == null ? null : dataController.SearchByUsername(loggedInUser));
        }

        public UserTransactionBo GetUserById(UserIdBo id)
        {
            return dataController.SearchById(id);
        }

        public int LoginRequest(LoginBo user)
        {
            // Returns 1 for successful login
            // Returns 0 for invalid pin
            // Returns -1 for lockout
            UserTransactionBo userData = dataController.SearchByUsername(new UsernameBo(user.Username));

            if (userData == null)
                return 0;
            else if (userData.Blocked)
                return -1;
            else if (userData.Username == user.Username && userData.Pin == user.Userpin)
            {
                attempts = 0;
                loggedInUser = new UsernameBo(userData.Username);
                return 1;
            }
            else
            {
                attempts += 1;
                if (attempts == 3)
                {
                    BlockUser(userData);
                    return -1;
                }
                return 0;
            }
        }

        public bool IsLockedOut(UserTransactionBo user)
        {
            return IsLockedOut(new UsernameBo(user.Username));
        }

        public bool IsLockedOut(UsernameBo username)
        {
            UserTransactionBo user = dataController.SearchByUsername(username);
            return user != null ? user.Blocked : false;
        }

        public bool BlockUser(UserTransactionBo user)
        {
            return dataController.UpdateData(
                user,
                new UserTransactionBo(
                    user,
                    blocked: true
                )
            );
        }

        public ReceiptBo RequestTransaction(Transaction transaction, UserTransactionBo to = null)
        {
            UserTransactionBo user = dataController.SearchByUsername(loggedInUser);

            if (!(transaction.TransactionType == Transaction.deposit) &&
                user.Assets < transaction.Amount)
                return new ReceiptBo(user, transaction, ReceiptBo.notEnoughCash);
            if (transaction.TransactionType == Transaction.withdraw &&
                user.Withdraw + transaction.Amount > 20000 &&
                DateTime.Now.Date <= user.Date)
                return new ReceiptBo(user, transaction, ReceiptBo.transactionLimit);

            UserTransactionBo newVer = null;
            if (transaction.TransactionType == Transaction.withdraw )
            {
                newVer = new UserTransactionBo(user,
                    assets: user.Assets - transaction.Amount,
                    lastTransactionDate: DateTime.Now.Date,
                    withdrawAmount: (DateTime.Now.Date > user.Date ? 0 : user.Withdraw) + transaction.Amount);
            }
            else if (transaction.TransactionType == Transaction.deposit)
            {
                newVer = new UserTransactionBo(user,
                    assets: user.Assets + transaction.Amount);
            }
            else if (transaction.TransactionType == Transaction.transfer)
            {
                newVer = new UserTransactionBo(user,
                    assets: user.Assets - transaction.Amount);

                UserTransactionBo newTo = new UserTransactionBo(to,
                    assets: to.Assets + transaction.Amount);

                dataController.UpdateData(to, newTo);
            }

            dataController.UpdateData(user, newVer);
            ReceiptBo receipt = new ReceiptBo(newVer, transaction);
            if (receipt.Error == ReceiptBo.ifError)
                dataController.SaveReceipt(receipt);
            return receipt;
        }

        public UserIdBo CreateNewAccount(UserTransactionBo user)
        {
            return dataController.CreateNewEntry(user);
        }

        public bool DeleteAccount(UserIdBo id)
        {
            return dataController.DeleteEntry(id);
        }

        public bool UpdateAccount(UserTransactionBo oldUser, UserTransactionBo newUser)
        {
            return dataController.UpdateData(oldUser, newUser);
        }

        public List<UserTransactionBo> SearchUsers(SearchByAmount query)
        {
            return dataController.GetUsersBySearchQuery(query);
        }

        public List<ReceiptBo> SearchTransactions(SearchByDateBo query)
        {
            return dataController.GetReceiptsBySearchQuery(query);
        }
    }
}
