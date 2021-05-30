using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ATM_DAL
{
    public class BaseDal
    {

        StreamWriter sw;
        FileStream fs;
        StreamReader sr;

        public List<String> ReadData()
        {

            fs = new FileStream("Record.csv", FileMode.Open,FileAccess.Read);
            sr = new StreamReader(fs);

            List<String> temp = new List<String>();
            while (!sr.EndOfStream)
            {
                temp.Add(sr.ReadLine());
            }

            sr.Close();
            fs.Close();

            return temp;
        }

        public bool WriteToData(List<String> data)
        {
            fs = new FileStream("data.csv", FileMode.Truncate, FileAccess.Write);
            sw = new StreamWriter(fs);

            foreach (string text in data)
            {
                sw.WriteLine(text);
            }

            sw.Close();
            fs.Close();

            return true;
        }

        public List<String> ReadTransactions()
        {
            fs = new FileStream("Transactions.csv", FileMode.OpenOrCreate,FileAccess.Read);
            sr = new StreamReader(fs);

            List<String> temp = new List<String>();
            while (!sr.EndOfStream)
            {
                temp.Add(sr.ReadLine());
            }

            sr.Close();
            fs.Close();

            return temp;
        }

       
        public bool WriteToTransactions(List<String> data)
        {
            fs = new FileStream("transactions.csv", FileMode.Truncate);
            sw = new StreamWriter(fs);

            foreach (string text in data)
            {
                sw.WriteLine(text);
            }

            sw.Close();
            fs.Close();

            return true;
        }
    }
}
