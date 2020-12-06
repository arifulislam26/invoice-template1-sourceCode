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
    public partial class History : Form
    {
        public History()
        {
            InitializeComponent();
            DateTime now = DateTime.Now;
        }

        private void History_Load(object sender, EventArgs e)
        {
            listload();
            tabledesign();
        }
        private void tabledesign()
        {

            historyList.Columns[0].HeaderText = "Product Name";
            historyList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //historyList.Columns[0].MinimumWidth = 150;
            //historyList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

            historyList.Columns[1].HeaderText = "Quantity";
           // historyList.Columns[1].m ;
            historyList.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            historyList.Columns[1].MinimumWidth = 70;
            historyList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

            historyList.Columns[2].HeaderText = "Discount";
            historyList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            historyList.Columns[2].MinimumWidth = 70;
            historyList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

            historyList.Columns[3].HeaderText = "Price";
            historyList.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            historyList.Columns[3].MinimumWidth = 100;
            historyList.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            
            historyList.Columns[4].HeaderText = "Customer Name";
            historyList.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            historyList.Columns[4].MinimumWidth = 150;
            historyList.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

            historyList.Columns[5].HeaderText = "Customer Address";
            historyList.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            historyList.Columns[5].MinimumWidth = 150;
            historyList.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

            historyList.Columns[6].HeaderText = "Customer Phone";
            historyList.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            historyList.Columns[6].MinimumWidth = 130;
            historyList.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

            historyList.Columns[7].HeaderText = "Invoice SL";
            historyList.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            historyList.Columns[7].MinimumWidth = 130;
            historyList.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

            historyList.Columns[8].HeaderText = "Date";
            historyList.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            historyList.Columns[8].MinimumWidth = 50;
            historyList.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

            historyList.Columns[9].HeaderText = "Holder Name";
            historyList.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            historyList.Columns[9].MinimumWidth = 100;
            historyList.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

            historyList.Columns[10].HeaderText = "VAT %";
            historyList.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            historyList.Columns[10].MinimumWidth = 50;
            historyList.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;



        }
        private void listload()
        {
            try
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\history.accdb";

                OleDbConnection connection = new OleDbConnection();

                connection.ConnectionString =
                        @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";

                connection.Open();
                string query = "SELECT pro_name,pro_quan,pro_disc,pro_price," +
                    "customer_name,customer_address,customer_phone,invoice_serial,esu_date,holder_name,vat_rate" +
                    " FROM invoice_history";

                OleDbCommand cmdd = new OleDbCommand(query, connection);
                cmdd.CommandText = query;
                OleDbDataAdapter da = new OleDbDataAdapter(cmdd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                historyList.DataSource = null;
                historyList.DataSource = dt;
                
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void searchedLoad()
        {
            try
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\history.accdb";

                OleDbConnection connection = new OleDbConnection();

                connection.ConnectionString =
                        @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";

                if (!string.IsNullOrWhiteSpace(nameBox.Text) && !string.IsNullOrWhiteSpace(invoiceBox.Text))
                {
                      connection.Open();
                        string query = "SELECT pro_name,pro_quan,pro_disc,pro_price," +
                    "customer_name,customer_phone,customer_address,invoice_serial,esu_date,holder_name,vat_rate" +
                        " FROM invoice_history WHERE pro_name =@proName AND invoice_serial =@BID AND esu_date= @date";

                        OleDbCommand cmdd = new OleDbCommand(query, connection);
                        cmdd.Parameters.AddWithValue("@proName", nameBox.Text);
                        cmdd.Parameters.AddWithValue("@BID", invoiceBox.Text);
                        cmdd.Parameters.AddWithValue("@date", dateTime.Text);
                        cmdd.CommandText = query;
                        OleDbDataAdapter da = new OleDbDataAdapter(cmdd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        historyList.DataSource = null;
                        historyList.DataSource = dt;
                    tabledesign();
                        connection.Close();
                   
                }
                else if(!string.IsNullOrWhiteSpace(nameBox.Text))
                {
                    connection.Open();
                    string query = "SELECT pro_name,pro_quan,pro_disc,pro_price," +
                "customer_name,customer_phone,customer_address,invoice_serial,esu_date,holder_name,vat_rate" +
                    " FROM invoice_history WHERE pro_name =@proName AND esu_date= @date";

                    OleDbCommand cmdd = new OleDbCommand(query, connection);
                    cmdd.Parameters.AddWithValue("@proName", nameBox.Text);
                    cmdd.Parameters.AddWithValue("@date", dateTime.Text);
                    cmdd.CommandText = query;
                    OleDbDataAdapter da = new OleDbDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    historyList.DataSource = null;
                    historyList.DataSource = dt;
                   
                    connection.Close();
                }
                else if(!string.IsNullOrWhiteSpace(invoiceBox.Text))
                {
                    connection.Open();
                    string query = "SELECT pro_name,pro_quan,pro_disc,pro_price," +
                "customer_name,customer_phone,customer_address,invoice_serial,esu_date,holder_name,vat_rate" +
                    " FROM invoice_history WHERE invoice_serial =@batch AND esu_date= @date";

                    OleDbCommand cmdd = new OleDbCommand(query, connection);
                    cmdd.Parameters.AddWithValue("@batch", invoiceBox.Text);
                    cmdd.Parameters.AddWithValue("@date", dateTime.Text);
                    cmdd.CommandText = query;
                    OleDbDataAdapter da = new OleDbDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    historyList.DataSource = null;
                    historyList.DataSource = dt;
                    
                    connection.Close();
                }
                else
                {
                    connection.Open();
                    string query = "SELECT pro_name,pro_quan,pro_disc,pro_price," +
                "customer_name,customer_phone,customer_address,invoice_serial,esu_date,holder_name,vat_rate" +
                    " FROM invoice_history WHERE  esu_date= @date";

                    OleDbCommand cmdd = new OleDbCommand(query, connection);
                    cmdd.Parameters.AddWithValue("@date", dateTime.Text);
                    cmdd.CommandText = query;
                    OleDbDataAdapter da = new OleDbDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    historyList.DataSource = null;
                    historyList.DataSource = dt;
                   
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
      
        private void searchBtn_Click_1(object sender, EventArgs e)
        {
            searchedLoad();
            tabledesign();
        }

        private void historyList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Clipboard.SetText(historyList[e.ColumnIndex, e.RowIndex].Value.ToString());
        }

        private void historyList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //silence is golden
        }

        private void invoiceBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }// main class
}
