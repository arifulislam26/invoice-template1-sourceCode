using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace invoice_aia
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
        }
        
    }
    
    public  class  printf
    {
        public static string cust_name { get; set; }
        public static string cust_phone { get; set; }
        public static string cust_address { get; set; }
        public static string invo { get; set; }
        public static string holder { get; set; }
        public static string time { get; set; }
        public static string subtotal { get; set; }
        public static string vatrate { get; set; }
        public static string totalvat { get; set; }
        public static string total { get; set; }
    }
}
