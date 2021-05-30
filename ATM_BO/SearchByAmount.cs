using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace ATM_BO
{
    public class SearchByAmount
    {
        private string id;
        private string cardholder;
        private string type;
        private string min;
        private string max;
        private string status;

        public string Id => this.id;
        public string CardHolder => this.cardholder;
        public string Type => this.type;
        public string Min => this.min;
        public string Max => this.max;
        public string Status => this.status;

        public SearchByAmount(string id, string cardholder, string type, string min , string max, string status)
        {
            this.id = id;
            this.cardholder = cardholder;
            this.type = type;
            this.min = min;
            this.max = max;
            this.status = status;
        }



    }
}
