using ATM_BO;
using ATM_BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_View
{
    public class AtmView
    {
        private  AtmBll Bll = new AtmBll();

        public void Login()
        {
            Console.Clear();

            while (true)
            {
                // Username input
                Console.Write("Enter Login: ");
                String username = Console.ReadLine();

                UsernameBo name = new UsernameBo(username);

                if (Bll.IsUser(name))
                {

                    if (Bll.IsLockedOut(name))
                    {
                        Console.WriteLine("You have been locked out of the system until further notice.");
                        System.Environment.Exit(0);
                    }

                    // PIN input
                    Console.Write("Enter Pin code: ");
                    String pin = Console.ReadLine();
                    LoginBo login = new LoginBo(username, pin);

                    int result;
                    while ((result = Bll.LoginRequest(login)) == 0)
                    {
                        Console.Write("Re-enter pin code: ");
                        pin = Console.ReadLine();
                        login = new LoginBo(username, pin);
                    }

                    // Lockout response
                    if (result == -1)
                    {
                        Console.WriteLine("You have been locked out of the system until further notice.");
                        System.Environment.Exit(0);
                    }
                    break;
                }
                Console.Write("Invalid Login. Re-");
            }

            if (Bll.GetLoggedInUser().AccessLevel == UserTransactionBo.user)
            {
                ShowUserMenu();
            }
            else
            {
                ShowAdminMenu();
            }
        }

        public  void ShowUserMenu()
        {
            bool invalid_option = false;
            while (true)
            {
                Console.Clear();
                if (invalid_option)
                    Console.WriteLine("Invalid Option");
                Console.Write("1----Withdraw Cash\n2----Cash Transfer\n3----Deposit Cash\n4----Display Balance\n5----Exit\n\nPlease select one of the above options:");
                string input = Console.ReadLine();

                invalid_option = false;
                switch (input)
                {
                    case "1":
                        WithdrawCash();
                        break;
                    case "2":
                        CashTransfer();
                        break;
                    case "3":
                        DepositCash();
                        break;
                    case "4":
                        DisplayBalance();
                        break;
                    case "5":
                        Environment.Exit(0);
                        break;
                    default:
                        invalid_option = true;
                        break;
                }

            }
        }

        private  void WithdrawCash()
        {
            bool invalid_option = false;
            while (true)
            {
                Console.Clear();
                if (invalid_option)
                    Console.WriteLine("Invalid Option");
                Console.Write("1) Fast Cash\n2) Normal Cash\n\nPlease select a mode of withdrawal:");
                string input = Console.ReadLine();

                invalid_option = false;
                switch (input)
                {
                    case "1":
                        FastCash();
                        return;
                    case "2":
                        NormalCash();
                        return;
                    default:
                        invalid_option = true;
                        break;
                }
            }
        }

        private  void FastCash()
        {
            bool invalid_option = false;
            while (true)
            {
                Console.Clear();
                if (invalid_option)
                    Console.WriteLine("Invalid Option");
                Console.Write("1----500\n2----1000\n3----2000\n4----5000\n5----10000\n6----15000\n7----20000\n\nSelect one of the denominations of money:");
                string input = Console.ReadLine();

                invalid_option = false;
                int cash = 0;
                switch (input)
                {
                    case "1":
                        cash = 500;
                        break;
                    case "2":
                        cash = 1000;
                        break;
                    case "3":
                        cash = 2000;
                        break;
                    case "4":
                        cash = 5000;
                        break;
                    case "5":
                        cash = 10000;
                        break;
                    case "6":
                        cash = 15000;
                        break;
                    case "7":
                        cash = 20000;
                        break;
                    default:
                        invalid_option = true;
                        break;
                }
                if (cash != 0)
                {
                    Console.Write($"Are you sure you want to withdraw Rs.{cash} (Y/N)? ");
                    input = Console.ReadLine();
                    if (input == "Y" || input == "y")
                    {
                        ReceiptBo receipt = Bll.RequestTransaction(new Transaction(cash, Transaction.withdraw));
                        if (receipt.Error == ReceiptBo.notEnoughCash)
                        {
                            Console.WriteLine("You don't have enough money in your account");
                            Console.ReadLine();
                        }
                        else if (receipt.Error == ReceiptBo.transactionLimit)
                        {
                            Console.WriteLine("Cant't Withdraw more money. You will surpass your daily limit of 20'000");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.Write("Do you wish to print a receipt (Y/N)? ");
                            input = Console.ReadLine();
                            if (input == "Y" || input == "y")
                            {
                                Console.WriteLine(receipt.PrintReceipt());
                                Console.ReadLine();
                            }
                        }
                    }
                    return;
                }
            }
        }

        private  void NormalCash()
        {
            bool invalid_option = false;
            while (true)
            {
                Console.Clear();
                if (invalid_option)
                    Console.WriteLine("Invalid Amount");
                Console.Write("Enter the withdrawal amount: ");
                string input = Console.ReadLine();
                int cash = 0;
                try
                {
                    cash = Convert.ToInt32(input);
                    if (cash <= 0)
                    {
                        invalid_option = true;
                        continue;
                    }
                }
                catch
                {
                    invalid_option = true;
                    continue;
                }

                invalid_option = false;
                Console.Write($"Are you sure you want to withdraw Rs.{cash} (Y/N)? ");
                input = Console.ReadLine();
                if (input == "Y" || input == "y")
                {
                    ReceiptBo receipt = Bll.RequestTransaction(new Transaction(cash, Transaction.withdraw));
                    if (receipt.Error == ReceiptBo.notEnoughCash)
                    {
                        Console.WriteLine("You don't have enough money in your account");
                        Console.ReadLine();
                    }
                    else if (receipt.Error == ReceiptBo.transactionLimit)
                    {
                        Console.WriteLine("Cant't Withdraw more money. You will surpass your daily limit of 20'000");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.Write("Do you wish to print a receipt (Y/N)? ");
                        input = Console.ReadLine();
                        if (input == "Y" || input == "y")
                        {
                            Console.WriteLine(receipt.PrintReceipt());
                            Console.ReadLine();
                        }
                    }
                }
                return;
            }
        }

        private  void CashTransfer()
        {
            bool invalid_option = false;
            while (true)
            {
                Console.Clear();
                if (invalid_option)
                    Console.WriteLine("Invalid Amount");
                Console.Write("Enter amount in multiples of 500: ");
                string input = Console.ReadLine();
                int cash = 0;
                try
                {
                    cash = Convert.ToInt32(input);
                    if (cash <= 0 || cash % 500 != 0)
                    {
                        invalid_option = true;
                        continue;
                    }
                }
                catch
                {
                    invalid_option = true;
                    continue;
                }

                invalid_option = false;
                UserTransactionBo userToTransfer = null;
                while (true)
                {
                    if (invalid_option)
                        Console.WriteLine("Invalid ID");
                    invalid_option = false;
                    Console.Write("Enter the account number to which you want to transfer: ");
                    input = Console.ReadLine();
                    try
                    {
                        userToTransfer = Bll.GetUserById(new UserIdBo(Convert.ToInt32(input)));
                        if (userToTransfer == null || Bll.GetLoggedInUser().Id == userToTransfer.Id)
                        {
                            invalid_option = true;
                            continue;
                        }
                    }
                    catch
                    {
                        invalid_option = true;
                        continue;
                    }
                    break;
                }

                invalid_option = false;
                Console.Write($"You wish to deposit Rs {cash} in account held by {userToTransfer.HolderName};\n If this information is correct please re-enter the account number: ");
                string input2 = Console.ReadLine();
                if (input2.Equals(input))
                {
                    ReceiptBo receipt = Bll.RequestTransaction(new Transaction(cash, Transaction.transfer), userToTransfer);
                    if (receipt.Error == ReceiptBo.notEnoughCash)
                    {
                        Console.WriteLine("You don't have enough money in your account");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.Write("Do you wish to print a receipt (Y/N)? ");
                        input = Console.ReadLine();
                        if (input == "Y" || input == "y")
                        {
                            Console.WriteLine(receipt.PrintReceipt());
                            Console.ReadLine();
                        }
                    }
                }
                return;
            }
        }

        private  void DepositCash()
        {
            bool invalid_option = false;
            while (true)
            {
                Console.Clear();
                if (invalid_option)
                    Console.WriteLine("Invalid Amount");
                Console.Write("Enter the cash amount to deposit: ");
                string input = Console.ReadLine();
                int cash = 0;
                try
                {
                    cash = Convert.ToInt32(input);
                    if (cash <= 0)
                    {
                        invalid_option = true;
                        continue;
                    }
                }
                catch
                {
                    invalid_option = true;
                    continue;
                }

                invalid_option = false;

                ReceiptBo receipt = Bll.RequestTransaction(new Transaction(cash, Transaction.deposit));
                Console.Write("Do you wish to print a receipt (Y/N)? ");
                input = Console.ReadLine();
                if (input == "Y" || input == "y")
                {
                    Console.WriteLine(receipt.PrintReceipt());
                    Console.ReadLine();
                }
                return;
            }
        }

        private  void DisplayBalance()
        {
            Console.Clear();
            UserTransactionBo user = Bll.GetLoggedInUser();
            Console.WriteLine($"Account #{user.Id}\nDate: {DateTime.Now.Date.ToString("dd/MM/yyyy")}\n\nBalance: {user.Assets}");
            Console.ReadLine();
        }

        public  void ShowAdminMenu()
        {
            bool invalid_option = false;
            while (true)
            {
                Console.Clear();
                if (invalid_option)
                    Console.WriteLine("Invalid Option");
                Console.Write("1----Create New Account.\n2----Delete Existing Account.\n3----Update Account Information.\n4----Search for Account.\n5----View Reports\n6----Exit\n\nPlease select one of the above options:");
                string input = Console.ReadLine();

                invalid_option = false;
                switch (input)
                {
                    case "1":
                        CreateNewAccount();
                        break;
                    case "2":
                        DeleteExistingAccount();
                        break;
                    case "3":
                        UpdateAccountInformation();
                        break;
                    case "4":
                        SearchForAccount();
                        break;
                    case "5":
                        ViewReports();
                        break;
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        invalid_option = true;
                        break;
                }
            }
        }

        private  void CreateNewAccount()
        {
            Console.Clear();
            Console.Write("Login: ");
            string login = Console.ReadLine();

            string pin = "";
            while (true)
            {
                Console.Write("PIN Code: ");
                pin = Console.ReadLine();
                try
                {
                    Convert.ToInt32(pin);
                    if (pin.Length != 5)
                        throw new Exception();
                }
                catch
                {
                    Console.Write("Invalid pin. Re-enter ");
                    continue;
                }
                break;
            }

            Console.Write("Holder's Name: ");
            string holder = Console.ReadLine();

            string type = "";
            while (true)
            {
                Console.Write("Type (savings, current): ");
                type = Console.ReadLine();
                if (type != UserTransactionBo.savings && type != UserTransactionBo.current)
                {
                    Console.Write("Invalid type. Re-enter ");
                    continue;
                }
                break;
            }

            string assets = "";
            while (true)
            {
                Console.Write("Starting Balance: ");
                assets = Console.ReadLine();
                try
                {
                    int con = Convert.ToInt32(assets);
                    if (con <= 0)
                        throw new Exception();
                }
                catch
                {
                    Console.Write("Invalid starting balance. Re-enter ");
                    continue;
                }
                break;
            }

            string active = "";
            while (true)
            {
                Console.Write("Status (active, blocked): ");
                active = Console.ReadLine();
                if (!active.Equals("active") && !active.Equals("blocked"))
                {
                    Console.Write("Invalid status. Re-enter ");
                    continue;
                }
                break;
            }

            UserIdBo createdAccount = Bll.CreateNewAccount(
                new UserTransactionBo( 0,login, pin,UserTransactionBo.user,(active.Equals("active") ? false : true),Convert.ToDecimal(assets), DateTime.Now.Date, 0,holder,type)
            );
            if (createdAccount != null)
                Console.WriteLine($"\nAccount Successfully Created – the account number assigned is: {createdAccount.UserId}");
            else
                Console.WriteLine("\nAccount could not be created");
            Console.ReadLine();
        }

        private  void DeleteExistingAccount()
        {
            Console.Clear();
            bool invalid_option = false;
            UserTransactionBo userToDelete = null;
            string input = "";
            while (true)
            {
                if (invalid_option)
                    Console.WriteLine("Invalid ID");
                invalid_option = false;
                Console.Write("Enter the account number which you want to delete: ");
                input = Console.ReadLine();
                try
                {
                    userToDelete = Bll.GetUserById(new UserIdBo(Convert.ToInt32(input)));
                    if (userToDelete == null)
                    {
                        invalid_option = true;
                        continue;
                    }
                }
                catch
                {
                    invalid_option = true;
                    continue;
                }
                break;
            }

            invalid_option = false;
            Console.Write($"You wish to delete the account held by {userToDelete.HolderName};\n If this information is correct please re-enter the account number: ");
            string input2 = Console.ReadLine();
            if (input2.Equals(input))
            {
                bool deletedUser = Bll.DeleteAccount(new UserIdBo(Convert.ToInt32(input)));
                if (deletedUser)
                {
                    Console.WriteLine("\nAccount Deleted Successfully");
                }
                else
                {
                    Console.WriteLine("\nAccount Deletion Failed");
                }
                Console.ReadLine();
            }
            return;
        }

        private  void UpdateAccountInformation()
        {
            Console.Clear();
            bool invalid_option = false;
            string input = "";
            UserTransactionBo userToChange = null;
            while (true)
            {
                if (invalid_option)
                    Console.WriteLine("Invalid ID");
                invalid_option = false;
                Console.Write("Enter the account number: ");
                input = Console.ReadLine();
                try
                {
                    userToChange = Bll.GetUserById(new UserIdBo(Convert.ToInt32(input)));
                    if (userToChange == null)
                    {
                        invalid_option = true;
                        continue;
                    }
                }
                catch
                {
                    invalid_option = true;
                    continue;
                }
                break;
            }

            string status = userToChange.Blocked ? "Disabled" : "Active";
            Console.WriteLine($"\nAccount # {userToChange.Id}\nType: {userToChange.Type}\nHolder: {userToChange.HolderName}\nBalance: {userToChange.Assets}\nStatus: {status}");
            Console.WriteLine($"\nPlease enter in the fields you wish to update (leave blank otherwise):\n");

            Console.Write("Login: ");
            string login = Console.ReadLine();

            string pin = "";
            while (true)
            {
                Console.Write("PIN Code: ");
                pin = Console.ReadLine();
                try
                {
                    if (pin.Length != 0)
                    {
                        Convert.ToInt32(pin);
                        if (pin.Length != 5)
                            throw new Exception();
                    }
                }
                catch
                {
                    Console.Write("Invalid pin. Re-enter ");
                    continue;
                }
                break;
            }

            Console.Write("Holder's Name: ");
            string holder = Console.ReadLine();

            string type = "";
            while (true)
            {
                Console.Write("Type (savings, current): ");
                type = Console.ReadLine();
                if (type != "" && type != UserTransactionBo.savings && type != UserTransactionBo.current)
                {
                    Console.Write("Invalid type. Re-enter ");
                    continue;
                }
                break;
            }

            string active = "";
            while (true)
            {
                Console.Write("Status (active, blocked): ");
                active = Console.ReadLine();
                if (!active.Equals("") && !active.Equals("active") && !active.Equals("blocked"))
                {
                    Console.Write("Invalid status. Re-enter ");
                    continue;
                }
                break;
            }

            bool changedAccount = Bll.UpdateAccount(userToChange,
                new UserTransactionBo(
                    userToChange.Id,
                    login == "" ? userToChange.Username : login,
                    pin == "" ? userToChange.Pin : pin,
                    userToChange.AccessLevel,
                    ((active == "" ? !userToChange.Blocked : active.Equals("active")) ? false : true),
                    userToChange.Assets,
                    DateTime.Now.Date,
                    0,
                    holder == "" ? userToChange.HolderName : holder,
                    type == "" ? userToChange.Type : type
                )
            );
            if (changedAccount == true)
                Console.WriteLine($"\nAccount successfully updated.");
            else
                Console.WriteLine("\nAccount could not be updated.");
            Console.ReadLine();
        }

        private  void SearchForAccount()
        {
            Console.Clear();
            Console.WriteLine("SEARCH MENU:\n\n");

            Console.Write("Account ID: ");
            string id = Console.ReadLine();

            Console.Write("User ID: ");
            string login = Console.ReadLine();

            Console.Write("Holders Name: ");
            string holder = Console.ReadLine();

            string type = "";
            while (true)
            {
                Console.Write("Type (savings, current): ");
                type = Console.ReadLine();
                if (type != "" && type != UserTransactionBo.savings && type != UserTransactionBo.current)
                {
                    Console.Write("Invalid type. Re-enter ");
                    continue;
                }
                break;
            }

            Console.Write("Balance: ");
            string assets = Console.ReadLine();

            string active = "";
            while (true)
            {
                Console.Write("Status (active, blocked): ");
                active = Console.ReadLine();
                if (!active.Equals("") && !active.Equals("active") && !active.Equals("blocked"))
                {
                    Console.Write("Invalid status. Re-enter ");
                    continue;
                }
                break;
            }

            List<UserTransactionBo> searchResults = Bll.SearchUsers(
                new SearchByAmount(
                    id,
                    holder,
                    type,
                    assets,
                    assets,
                    active
                )
            );

            Console.WriteLine("==== SEARCH RESULTS ======\n\nAccount ID\t\tHolders Name\t\tType\t\tBalance\t\tStatus");
            foreach (UserTransactionBo result in searchResults)
            {
                string currStatus = result.Blocked ? "Disabled" : "Active";
                Console.WriteLine($"{result.Id}\t\t{result.HolderName}\t\t{result.Type}\t\t{result.Assets}\t\t{currStatus}");
            }

            Console.ReadLine();
        }

        private  void ViewReports()
        {
            bool invalid_option = false;
            while (true)
            {
                Console.Clear();
                if (invalid_option)
                    Console.WriteLine("Invalid Option");
                Console.Write("1---Accounts By Amount\n2---Accounts By Date\n\nPlease select a mode of withdrawal:");
                string input = Console.ReadLine();

                invalid_option = false;
                switch (input)
                {
                    case "1":
                        AccountsByAmount();
                        return;
                    case "2":
                        AccountsByDate();
                        return;
                    default:
                        invalid_option = true;
                        break;
                }
            }
        }

        private  void AccountsByAmount()
        {
            Console.Clear();
            string min = "";
            while (true)
            {
                Console.Write("Enter the minimum amount: ");
                min = Console.ReadLine();
                try
                {
                    Convert.ToInt32(min);
                }
                catch
                {
                    Console.Write("Invalid Amount. Re-enter ");
                    continue;
                }
                break;
            }

            string max = "";
            while (true)
            {
                Console.Write("Enter the maximum amount: ");
                max = Console.ReadLine();
                try
                {
                    int con = Convert.ToInt32(max);
                    if (con < Convert.ToInt32(min))
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    Console.Write("Invalid Amount. Re-enter ");
                    continue;
                }
                break;
            }

            List<UserTransactionBo> searchResults = Bll.SearchUsers(new SearchByAmount(
                "",
                "",
                "",
                min,
                max,
                ""
            ));

            Console.WriteLine("==== SEARCH RESULTS ======\n\nAccount ID\t\tHolders Name\t\tType\t\tBalance\t\tStatus");
            foreach (UserTransactionBo result in searchResults)
            {
                string currStatus = result.Blocked ? "Disabled" : "Active";
                Console.WriteLine($"{result.Id}\t\t{result.HolderName}\t\t{result.Type}\t\t{result.Assets}\t\t{currStatus}");
            }

            Console.ReadLine();
        }

        private void AccountsByDate()
        {
            Console.Clear();
            string input = "";
            UserTransactionBo userToSearch = null;
            while (true)
            {
                Console.Write("Enter the account number: ");
                input = Console.ReadLine();
                try
                {
                    userToSearch = Bll.GetUserById(new UserIdBo(Convert.ToInt32(input)));
                    if (userToSearch == null)
                    {
                        Console.Write("Invalid user id. Re-");
                        continue;
                    }
                }
                catch
                {
                    Console.Write("Invalid user id. Re-");
                    continue;
                }
                break;
            }


            DateTime start, end;
            string min = "";
            while (true)
            {
                Console.Write("Enter the starting Date: ");
                min = Console.ReadLine();
                try
                {
                    start = DateTime.ParseExact(min, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                }
                catch
                {
                    Console.Write("Invalid date. Re-enter ");
                    continue;
                }
                break;
            }

            string max = "";
            while (true)
            {
                Console.Write("Enter the ending Date: ");
                max = Console.ReadLine();
                try
                {
                    end = DateTime.ParseExact(max, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    if (end < start)
                        throw new Exception();
                }
                catch
                {
                    Console.Write("Invalid date. Re-enter ");
                    continue;
                }
                break;
            }

            List<ReceiptBo> searchResults = Bll.SearchTransactions(new SearchByDateBo(
                userToSearch,
                start,
                end
            ));

            Console.WriteLine("==== SEARCH RESULTS ======\n\nTransaction Type\t\tAccount ID\t\tHolders Name\t\tAmount\t\tDate");
            foreach (ReceiptBo result in searchResults)
            {
                string currStatus = "";
                switch (result.Transaction.TransactionType)
                {
                    case Transaction.withdraw:
                        currStatus = "Cash Withdrawn";
                        break;
                    case Transaction.deposit:
                        currStatus = "Cash Deposited";
                        break;
                    case Transaction.transfer:
                        currStatus = "Cash Transfer";
                        break;
                }

                Console.WriteLine($"{currStatus}\t\t{result.User.Id}\t\t{result.User.HolderName}\t\t{result.Transaction.Amount}\t\t{result.Date.ToString("dd/MM/yyyy")}");
            }

            Console.ReadLine();

        }
    }
}
