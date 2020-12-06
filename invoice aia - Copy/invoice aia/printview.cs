using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace invoice_aia
{
    public partial class printview : Form
    {
        public printview()
        {
            InitializeComponent();
        }
        private void order_list()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\nika.accdb";

            OleDbConnection connection = new OleDbConnection();

            connection.ConnectionString =
                @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";
            connection.Open();

            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            string nik = "SELECT pro_name,pro_quan,pro_disc,pro_price FROM orderList";
            // Pro_name,Pro_price,Pro_serial,Pro_desc)

            command.CommandText = nik;
            OleDbDataAdapter da = new OleDbDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            orderlistData.DataSource = dt;

            orderlistData.ClearSelection();
            orderlistData.Columns[0].HeaderText = "Description";
            orderlistData.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            orderlistData.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            orderlistData.Columns[1].MinimumWidth = 75;
            orderlistData.Columns[1].DividerWidth = 0;
            orderlistData.Columns[1].HeaderText = "Quantity";
            orderlistData.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            orderlistData.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            orderlistData.Columns[2].MinimumWidth = 75;
            orderlistData.Columns[2].DividerWidth = 0;
            orderlistData.Columns[2].HeaderText = "Discount";
            orderlistData.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            orderlistData.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            orderlistData.Columns[3].MinimumWidth = 100;
            orderlistData.Columns[3].DividerWidth = 0;
            orderlistData.Columns[3].HeaderText = "Total Price";
            orderlistData.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            connection.Close();

        }
        private void dataload()
        {
            Namelabel.Text = printf.cust_name;
            phonelabel.Text = printf.cust_phone;
            addresslabel.Text = printf.cust_address;
            date.Text = printf.time;
            invoiceno.Text = printf.invo;
            sub.Text = printf.subtotal;
            rate.Text = printf.vatrate;
            vat.Text = printf.totalvat;
            amnt.Text = printf.total;
        }


        private void printview_Load(object sender, EventArgs e)
        {
            order_list();
            dataload();
        }
    }//main class
}
