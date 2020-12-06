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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            times.Text = "" + DateTime.Now.ToString("dd/MM/yyyy");
            invoice_total_price.Start();
            batch_search_timer.Start();
            
            


        }
       public int invoiceNumber = 001;
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            times.Text = "" + dateTime.Value.ToString("dd/MM/yyyy");
        }

        private void total_price_invoice()
        {
            if(!string.IsNullOrWhiteSpace(unitprice_box.Text) && !string.IsNullOrWhiteSpace(quantity_box.Text))
            {
                double unitprice = double.Parse(unitprice_box.Text);
                double quantity = double.Parse(quantity_box.Text);
                double total_price = (unitprice * quantity);
                if(!string.IsNullOrWhiteSpace(discount_box.Text))
                {
                    double discount = double.Parse(discount_box.Text);
                    discount = discount / 100;
                    total_price = total_price-(total_price * discount);
                    total_price_box.Text = total_price.ToString("0.00");
                }
                else
                    total_price_box.Text = total_price.ToString("0.00");
            }
            else
            {
                total_price_box.Clear();
            }  
        }

        private void invoice_total_price_Tick(object sender, EventArgs e)
        {
            total_price_invoice();
        }

        private void search_box_Enter(object sender, EventArgs e)
        {
            if (search_box.Text == "search")
                search_box.Clear();
            batch_search_timer.Start();
        }
        private void search_box_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(search_box.Text))
                search_box.Text = "search";
        }

        private void data_search()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\Product_Database.accdb";

            OleDbConnection connection = new OleDbConnection();

            connection.ConnectionString =
                    @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";

            connection.Open();
            string query = "SELECT * FROM Products WHERE pro_batch LIKE '%" + search_box.Text.ToString() + "'";

            OleDbCommand cmdd = new OleDbCommand(query, connection);

            cmdd.CommandType = CommandType.Text;
            OleDbDataReader dr = cmdd.ExecuteReader();
            if (dr.Read())
            {
                desc_box.Text = dr["pro_desc"].ToString();
                unitprice_box.Text = dr["pro_price"].ToString();
                discount_box.Text = dr["pro_disc"].ToString();
                comnt_label.Text = "Note:"+dr["pro_comment"].ToString();
                comnt_label.Visible = true;
                connection.Close();
            }
            else
            {
                desc_box.Clear();
                unitprice_box.Clear();
                discount_box.Clear();
                comnt_label.Visible = false;
                connection.Close();
            }
           
        }
        private void batch_search_timer_Tick(object sender, EventArgs e)
        {
            if(search_box.TextLength>=3)
                data_search();
            else
            {
                desc_box.Clear();
                unitprice_box.Clear();
                discount_box.Clear();
            }


        }

        private void unitprice_box_KeyDown(object sender, KeyEventArgs e)
        {
            if(search_box.Text=="search" || string.IsNullOrWhiteSpace(search_box.Text))
                batch_search_timer.Stop();
           
        }

        private void tem_product_insert()
        {

            string name = desc_box.Text;
            string quantity = quantity_box.Text;
            string disc = discount_box.Text;
            string price = total_price_box.Text;

            try
            {
               
                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\nika.accdb";
                OleDbConnection connection = new OleDbConnection();
                connection.ConnectionString =
              @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";
                connection.Open();

               // string command = "insert into orderList(pro_price,pro_disc,pro_quan,pro_name)  values('" + price + "','" + disc + "','" + quantity + "','" + name + "') ";

                string command = "insert into" +
                    " orderList (pro_price,pro_disc,pro_quan,pro_name) " +
                    "values(@price,@disc,@quantity,@name) ";

                OleDbCommand cmdd = new OleDbCommand(command, connection);
                cmdd.Parameters.AddWithValue("@price", total_price_box.Text);
                cmdd.Parameters.AddWithValue("@disc", discount_box.Text);
                cmdd.Parameters.AddWithValue("@quantity", quantity_box.Text);
                cmdd.Parameters.AddWithValue("@name", desc_box.Text);
                cmdd.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void item_order_btn_Click(object sender, EventArgs e)
        {
            tem_product_insert();
            order_list();
            total();
        }
 
        private void order_list()
        {
            try
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\nika.accdb";

                OleDbConnection connection = new OleDbConnection();

                connection.ConnectionString =
                    @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";
                connection.Open();

                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                string nik = "SELECT pro_price,pro_disc,pro_quan,pro_name FROM orderList";
                // Pro_name,Pro_price,Pro_serial,Pro_desc)

                command.CommandText = nik;
                OleDbDataAdapter da = new OleDbDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                orderlistData.DataSource = dt;

                orderlistData.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                orderlistData.Columns[1].MinimumWidth = 100;
                orderlistData.Columns[1].DividerWidth = 1;
                orderlistData.Columns[1].HeaderText = "Total Price";
                orderlistData.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                orderlistData.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                orderlistData.Columns[2].MinimumWidth = 75;
                orderlistData.Columns[2].DividerWidth = 1;
                orderlistData.Columns[2].HeaderText = "Discount";
                orderlistData.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                orderlistData.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                orderlistData.Columns[3].MinimumWidth = 75;
                orderlistData.Columns[3].DividerWidth = 1;
                orderlistData.Columns[3].HeaderText = "Quantity";
                orderlistData.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                orderlistData.Columns[4].HeaderText = "Description";
                orderlistData.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                connection.Close();
            }
            catch
            {
                //silence is golden
            }


        }
        private string invoice ;
       

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                dateTime.Value = DateTime.Today; ;
                order_list();
                loadinvoice();
            }
            catch
            {
                MessageBox.Show("Mother fuck!","Check the database file is ok or not",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
           
        }

        private void orderlistData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string name = Convert.ToString(orderlistData.Rows[e.RowIndex].Cells["pro_name"].Value);
                try
                {
                    string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\nika.accdb";
                    OleDbConnection connection = new OleDbConnection();
                    connection.ConnectionString =
                        @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";
                    connection.Open();
                    string command = "DELETE * FROM orderList WHERE pro_name LIKE '%" + name + "'";
                    OleDbCommand cmdd = new OleDbCommand(command, connection);
                    cmdd.ExecuteNonQuery();
                    connection.Close();
                    order_list();
                    total();
                }
                catch
                {
                    //silence is golden
                }

            }
        }
        private void total()
        {
            double subtotal = 0;

            for (int i = 0; i < orderlistData.Rows.Count; i++)
            {
                subtotal += Convert.ToDouble(orderlistData.Rows[i].Cells["pro_price"].Value);
            }
            subtotalBox.Text = "" + subtotal.ToString("0.00");
            if(!string.IsNullOrWhiteSpace(taxrateBox.Text))
            {
                double taxpar = double.Parse(taxrateBox.Text);
                double tax = (taxpar / 100) * subtotal;
                taxBox.Text=  tax.ToString("0.00");
                totalBox.Text = (tax + subtotal).ToString("0.00");
            }
        }

        private void taxrateBox_TextChanged(object sender, EventArgs e)
        {
           
            if(string.IsNullOrWhiteSpace(taxrateBox.Text))
            {
                taxrateBox.Text = "0";
            }
            total();
        }

        private void sadedataload()
        {

            string customername = customernameBox.Text;
            string customerphone = customerphoneBox.Text;
            string customeraddress = customeraddressBox.Text;
            string invoice_Number = invoicenumberBox.Text;
            string datetimes = times.Text;
            string holdername = holdernameBox.Text;
            string taxrate = taxrateBox.Text;

            try
            {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\history.accdb";
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString =
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";

                connection.Open();

                for (int i = 0; i < orderlistData.Rows.Count; i++)
                {
                    string name = orderlistData.Rows[i].Cells["pro_name"].Value.ToString();
                    string quan = orderlistData.Rows[i].Cells["pro_quan"].Value.ToString();
                    string price = orderlistData.Rows[i].Cells["pro_price"].Value.ToString();
                    string disc = orderlistData.Rows[i].Cells["pro_disc"].Value.ToString();

                    string command = "insert into invoice_history(Pro_name,pro_quan,pro_price,pro_disc,customer_name,customer_phone,customer_address,invoice_serial,esu_date,holder_name,vat_rate)" +
                   "  values('" + name + "','" + quan + "','" + price + "','" + disc + "','" + customername
                   + "','" + customerphone + "','" + customeraddress + "','" + invoice_Number + "','" + datetimes + "','" + holdername + "','" + taxrate + "') ";
                    OleDbCommand cmdd = new OleDbCommand(command, connection);
                    cmdd.ExecuteNonQuery();

                }
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            



        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            sadedataload();

        }
        private void loadinvoice()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\invoicenumer.dat";

            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    invoice = s.ToString();
                }

            }
            invoicenumberBox.Text = invoice;
        }
        private void deletetem()
        {
            try
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\nika.accdb";
                OleDbConnection connection = new OleDbConnection();
                connection.ConnectionString =
                    @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";
                connection.Open();
                string command = "DELETE * FROM orderList";
                OleDbCommand cmdd = new OleDbCommand(command, connection);
                cmdd.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
             
        private void newbillBtn_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\invoicenumer.dat";
            FileInfo fi = new FileInfo(path);
            int newinvoicenumber = int.Parse (invoicenumberBox.Text);

            newinvoicenumber = newinvoicenumber + 1;
            if (fi.Exists)
            {
                fi.Delete();
            }
            using (StreamWriter sw = fi.CreateText())
            {
                sw.WriteLine(newinvoicenumber);
            }
                loadinvoice();
            customernameBox.Clear();
            customerphoneBox.Text = "880";
            customeraddressBox.Clear();
            subtotalBox.Clear();
            taxBox.Clear();
            deletetem();
            orderlistData.DataSource = null;

        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            printf.cust_name = customernameBox.Text;
            printf.cust_phone = customerphoneBox.Text;
            printf.cust_address = customeraddressBox.Text;
            printf.invo = invoicenumberBox.Text;
            printf.holder = holdernameBox.Text;
            printf.time = times.Text;
            printf.subtotal = subtotalBox.Text;
            printf.vatrate = taxrateBox.Text;
            printf.totalvat = taxBox.Text;
            printf.total = totalBox.Text;

            printview print = new printview();
            print.Show();
            sadedataload();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            DialogResult dialogResult = MessageBox.Show("Name: Ariful Islam (Akash) \n https://www.facebook.com/webariful/",
                "check portfolio", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(dialogResult == DialogResult.OK)
            {
                System.Diagnostics.Process.Start("https://www.facebook.com/webariful/");
            }
                
        }

        private void Product_dataBtn_Click(object sender, EventArgs e)
        {
            Productdata prodatabase = new Productdata();
            prodatabase.Show();
        }

        private void historyBtn_Click(object sender, EventArgs e)
        {
            History histry = new History();
            histry.Show();
        }

        private void customerphoneBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) )
            {
                e.Handled = true;
            }
        }

        private void invoicenumberBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void unitprice_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
    }//main function
}
