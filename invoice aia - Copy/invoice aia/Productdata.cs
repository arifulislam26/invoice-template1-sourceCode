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
    public partial class Productdata : Form
    {
        public Productdata()
        {
            InitializeComponent();
        }
        private void tabledesign()
        {

            orderlistData.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            orderlistData.Columns[0].MinimumWidth = 140;
            orderlistData.Columns[0].DividerWidth = 0;
            orderlistData.Columns[0].HeaderText = "Batch Number";
            orderlistData.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            orderlistData.Columns[1].HeaderText = "Description";
            orderlistData.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            orderlistData.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            orderlistData.Columns[2].MinimumWidth = 100;
            orderlistData.Columns[2].DividerWidth = 0;
            orderlistData.Columns[2].HeaderText = "Discount";
            orderlistData.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            orderlistData.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            orderlistData.Columns[3].MinimumWidth = 100;
            orderlistData.Columns[3].DividerWidth = 0;
            orderlistData.Columns[3].HeaderText = "Price";
            orderlistData.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            orderlistData.Columns[4].HeaderText = "Comment";
            orderlistData.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

        }
        private void productList()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\Product_Database.accdb";

            OleDbConnection connection = new OleDbConnection();

            connection.ConnectionString =
                @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";
            connection.Open();

            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            string nik = "SELECT pro_batch,pro_desc,pro_disc,pro_price,pro_comment FROM Products";
            // pro_batch,pro_desc,pro_comment,pro_price,pro_disc

            command.CommandText = nik;
            OleDbDataAdapter da = new OleDbDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            orderlistData.DataSource = dt;
            tabledesign();
            connection.Close();
        }

        private void Productdata_Load(object sender, EventArgs e)
        {
            productList();
        }

        private void orderlistData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                DataGridViewRow select = orderlistData.Rows[e.RowIndex];
                batchnumberBox.Text = select.Cells[0].Value.ToString();
                nameBox.Text = select.Cells[1].Value.ToString();
                discountBox.Text = select.Cells[2].Value.ToString();
                priceBox.Text = select.Cells[3].Value.ToString();
                commentBox.Text = select.Cells[4].Value.ToString();
                batchnumberBox.ReadOnly = true;
                saveBtn.Text = "Update";
                newBtn.Visible = true;
            }
        }
        private void datadlt()
        {
            string deletebatch = batchnumberBox.Text;
            try
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\Product_Database.accdb";
                OleDbConnection connection = new OleDbConnection();
                connection.ConnectionString =
                    @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";
                connection.Open();
                string command = "DELETE * FROM Products WHERE pro_batch  LIKE '%" + deletebatch + "'";
                OleDbCommand cmdd = new OleDbCommand(command, connection);
                cmdd.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
               
                MessageBox.Show("Batch Number doesn't find", "Delete", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (DialogResult == DialogResult.Cancel)
                {
                    batchnumberBox.Clear();
                    nameBox.Clear();
                    discountBox.Clear();
                    priceBox.Clear();
                    commentBox.Clear();
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            datadlt();
            productList();
        }

        private void updatedata()
        {
           
            try
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\Product_Database.accdb";
                OleDbConnection connection = new OleDbConnection();
                connection.ConnectionString =
                    @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";
                connection.Open();
                OleDbCommand cmdd = new OleDbCommand(@"UPDATE Products
                                                    SET pro_desc = @Pname,
                                                        pro_comment = @Pcmnt,
                                                        pro_price = @Proprice,
                                                        pro_disc = @Pdisc
                                                    WHERE pro_batch = @BID", connection);

               
                cmdd.Parameters.AddWithValue("@Pname", nameBox.Text);
                cmdd.Parameters.AddWithValue("@Pcmnt", commentBox.Text);
                cmdd.Parameters.AddWithValue("@Proprice", priceBox.Text);
                cmdd.Parameters.AddWithValue("@Pdisc", discountBox.Text);
                cmdd.Parameters.AddWithValue("@BID", batchnumberBox.Text);
                cmdd.ExecuteNonQuery();
                connection.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void newproAdd()
        {
           
            
            try
            {

                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\Product_Database.accdb";
                OleDbConnection connection = new OleDbConnection();
                connection.ConnectionString =
              @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";
                connection.Open();
                string command = "insert into" +
                    " Products (pro_batch,pro_desc,pro_comment,pro_price,pro_disc) " +
                    "values(@BID,@Pname,@Pcmnt,@Proprice,@Pdisc) ";

                OleDbCommand cmdd = new OleDbCommand(command, connection);
                cmdd.Parameters.AddWithValue("@BID", batchnumberBox.Text);
                cmdd.Parameters.AddWithValue("@Pname", nameBox.Text);
                cmdd.Parameters.AddWithValue("@Pcmnt", commentBox.Text);
                cmdd.Parameters.AddWithValue("@Proprice", priceBox.Text);
                cmdd.Parameters.AddWithValue("@Pdisc", discountBox.Text);
                cmdd.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(saveBtn.Text== "Update")
            {
                updatedata();
                productList();
            }
            else if(saveBtn.Text=="Save")
            {
                newproAdd();
                productList();
            }
            
        }

        private void newBtn_Click(object sender, EventArgs e)
        {
            newBtn.Visible = false;
            batchnumberBox.ReadOnly = false;
            saveBtn.Text = "Save";
            batchnumberBox.Clear();
            nameBox.Clear();
            discountBox.Clear();
            priceBox.Clear();
            commentBox.Clear();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\Product_Database.accdb";

                OleDbConnection connection = new OleDbConnection();

                connection.ConnectionString =
                        @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";

                connection.Open();
                string query = "SELECT pro_batch,pro_desc,pro_disc,pro_price,pro_comment FROM Products WHERE pro_batch LIKE '%" + searchBox.Text.ToString() + "'";

                OleDbCommand cmdd = new OleDbCommand(query, connection);
                cmdd.CommandText = query;
                OleDbDataAdapter da = new OleDbDataAdapter(cmdd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                orderlistData.DataSource = null;
                orderlistData.DataSource = dt;
                connection.Close();
                tabledesign();
            }
            catch
            {
                //silance is golden
            }
           
        }

        private void batchnumberBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void priceBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
    }// main class
}
