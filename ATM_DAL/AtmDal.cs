using ATM_BO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_DAL
{
    public class AtmDal : BaseDal
    {
        public UserTransactionBo SearchById(UserIdBo id)
        {
            List<String> data = ReadData();

            foreach (string user in data)
            {
                string[] arr = Decrypt(user).Split(',');
                if (Convert.ToInt32(arr[0]) == id.UserId)
                {
                    return new UserTransactionBo(Convert.ToInt32(arr[0]), arr[1], arr[2], Convert.ToInt32(arr[3]),
                        Convert.ToBoolean(arr[4]), Convert.ToDecimal(arr[5]),
                        DateTime.ParseExact(arr[6], "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture),
                        Convert.ToDecimal(arr[7]), arr[8], arr[9]);
                }
            }
            return null;
        }

        public UserTransactionBo SearchByUsername(UsernameBo username)
        {
            List<String> data = ReadData();

            foreach (string user in data)
            {

                string[] arr = Decrypt(user).Split(',');
                if (arr[1] == username.Username)
                {
                    return new UserTransactionBo(Convert.ToInt32(arr[0]), arr[1], arr[2], Convert.ToInt32(arr[3]),
                        Convert.ToBoolean(arr[4]), Convert.ToDecimal(arr[5]),
                        DateTime.ParseExact(arr[6], "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture),
                        Convert.ToDecimal(arr[7]), arr[8], arr[9]);
                }
            }
            return null;
        }

      

        public bool UpdateData(UserTransactionBo oldU, UserTransactionBo newU)
        {
            List<String> data = ReadData();

            for (int i = 0; i < data.Count; i++)
            {
                string[] arr = Decrypt(data[i]).Split(',');
                if (arr[1] == oldU.Username)
                {
                    data[i] = EncryptData(newU.ConvertToExcel());
                }
            }

            return WriteToData(data);
        }

        public string EncryptData(string inp)
        {
            string returnVal = "";

            foreach (char c in inp)
            {
                if (c >= 'a' && c <= 'z')
                {
                    returnVal += Convert.ToChar(219 - c);
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    returnVal += Convert.ToChar(155 - c);
                }
                else if (c >= '0' && c <= '9')
                {
                    returnVal += Convert.ToChar(105 - c);
                }
                else
                {
                    returnVal += c;
                }
            }

            return returnVal;
        }

        public string Decrypt(string inp)
        {
            return EncryptData(inp);
        }

        public UserIdBo CreateNewEntry(UserTransactionBo user)
        {
            List<string> data = ReadData();
            string lastEntry = Decrypt(data[data.Count - 1]);

            int lastID = Convert.ToInt32(lastEntry.Split(',')[0]);

            UserTransactionBo finalUser = new UserTransactionBo(user,
                id: lastID + 1
            );

            data.Add(EncryptData(finalUser.ConvertToExcel()));

            WriteToData(data);
            return (new UserIdBo(lastID + 1));
        }

        public bool DeleteEntry(UserIdBo id)
        {
            List<string> data = ReadData();


            for (int i = 0; i < data.Count; i++)
            {
                if (Convert.ToInt32(Decrypt(data[i]).Split(',')[0]) == id.UserId)
                {
                    data.RemoveAt(i);
                    break;
                }
            }

            WriteToData(data);
            return true;
        }

        public List<UserTransactionBo> GetUsersBySearchQuery(SearchByAmount query)
        {
            List<string> data = ReadData();
            List<UserTransactionBo> result = new List<UserTransactionBo>();

            foreach (string line in data)
            {
                string[] arr = Decrypt(line).Split(',');
                if (
                    (query.Id == arr[0] || query.Id == "") &&
                    ((query.Status == "active" ? "False" : "True") == arr[4] || query.Status == "") &&
                    (query.Min == "" || Convert.ToInt32(query.Min) <= Convert.ToInt32(arr[5])) &&
                    (query.Max == "" || Convert.ToInt32(query.Max) >= Convert.ToInt32(arr[5])) &&
                    (query.CardHolder == arr[8] || query.CardHolder == "") &&
                    (query.Type == arr[9] || query.Type == "")
                )
                    result.Add(new UserTransactionBo(Convert.ToInt32(arr[0]), arr[1], arr[2], Convert.ToInt32(arr[3]),
                        Convert.ToBoolean(arr[4]), Convert.ToDecimal(arr[5]),
                        DateTime.ParseExact(arr[6], "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture),
                        Convert.ToDecimal(arr[7]), arr[8], arr[9]));
            }

            return result;
        }

        public void SaveReceipt(ReceiptBo receipt)
        {

            List<String> transactions = ReadTransactions();
            transactions.Add(EncryptData(receipt.ConvertToExcel()));

            WriteToTransactions(transactions);
        }

        public List<ReceiptBo> GetReceiptsBySearchQuery(SearchByDateBo query)
        {
            List<String> transactions = ReadTransactions();
            List<ReceiptBo> result = new List<ReceiptBo>();

            foreach (string receipt in transactions)
            {
                string[] split = Decrypt(receipt).Split(',');
                DateTime date = DateTime.ParseExact(split[4], "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                if (date >= query.StartingDate && date <= query.EndingDate)
                    result.Add(new ReceiptBo(
                        new UserTransactionBo(  Convert.ToInt32(split[1]),"","00000",UserTransactionBo.user,false, 0,DateTime.Now,0,split[2],UserTransactionBo.current),
                        new Transaction(Convert.ToDecimal(split[3]), Convert.ToInt32(split[0])),
                        date: date)
                    );
            }
            return result;
        }
    }
}

