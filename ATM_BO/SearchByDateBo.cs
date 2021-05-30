using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_BO
{
    public class SearchByDateBo
    {
        private UserTransactionBo user;
        private DateTime starting, ending;

        public UserTransactionBo User => this.user;
        public DateTime StartingDate => this.starting;
        public DateTime EndingDate => this.ending;

        public SearchByDateBo(UserTransactionBo user, DateTime starting, DateTime ending)
        {
            this.user = user;
            this.starting = starting;
            this.ending = ending;
        }
    }
}
