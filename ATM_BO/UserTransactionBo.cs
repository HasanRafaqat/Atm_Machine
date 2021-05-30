using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_BO
{
    public class UserTransactionBo
    {

        public const int user = 1;
        public const int admin = 2;
        public const string savings = "savings";
        public const string current = "current";

       

        public string Pin { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string HolderName { get; set; }
        public string Type { get; set; }
        public int AccessLevel { get; set; }
        public bool Blocked { get; set; }
        public decimal Assets { get; set; }
        public decimal Withdraw { get; set; }
        public DateTime Date { get; set; }


        public UserTransactionBo(UserTransactionBo mainUser, int id = -1, string username = null, string pin = null, int accessLevel = -1,bool? blocked = null, decimal? assets = null, DateTime? lastTransactionDate = null,decimal? withdrawAmount = null, string holderName = null, string type = null)
        {
            this.Id = id == -1 ? mainUser.Id : id;
            this.Username = username == null ? mainUser.Username : username;
            this.Pin = pin == null ? mainUser.Pin : pin;
            this.AccessLevel = accessLevel == -1 ? mainUser.AccessLevel : accessLevel;
            this.Blocked = blocked == null ? mainUser.Blocked : (bool)blocked;
            this.Assets = assets == null ? mainUser.Assets : (decimal)assets;
            this.Date = lastTransactionDate == null ? mainUser.Date : (DateTime)lastTransactionDate;
            this.Withdraw = withdrawAmount == null ? mainUser.Withdraw : (decimal)withdrawAmount;
            this.HolderName = holderName == null ? mainUser.HolderName : holderName;
            this.Type = type == null ? mainUser.Type : type;
        }

      

        public UserTransactionBo(int id, string username, string pin, int accessLevel, bool blocked, decimal assets,
            DateTime lastTransactionDate, decimal withdrawAmount, string holderName, string type)
        {
            if (pin.Length != 5)
                throw new ArgumentException("PIN length is not 5 digits");
            if (accessLevel != admin && accessLevel != user)
                throw new ArgumentOutOfRangeException("Unknown access level");
            if (!type.Equals(current) && !type.Equals(savings))
                throw new ArgumentException("Unknown account type");

            this.id = id;
            this.username = username;
            this.pin = pin;
            this.accesslevel = accessLevel;
            this.blocked = blocked;
            this.assets = assets;
            this.date = lastTransactionDate;
            this.withdrawAmount = withdrawAmount;
            this.holdername = holderName;
            this.type = type;
        }

        public string ConvertToExcel()
        {
            return $"{id},{username},{pin},{accesslevel},{blocked},{assets},{date.ToString("dd/MM/yyyy")},{withdrawAmount},{holdername},{type}";
        }


    }
}
