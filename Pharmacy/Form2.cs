using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;


namespace Pharmacy
{

    public partial class Form2 : Form
    {
        int height, width, a = 0;
        float total = 0;
        int dis = 0;
        int chk = 0;
        string med = "";
        SqlDataAdapter adapt1, adapt2;
        DataTable tbl1, tbl2;
        SqlCommandBuilder cmd1, cmd2;
        int R_Id;
        int r_qty;
        float r_st, r_up;
        string r_med;
        int reader1;
        SqlConnection cn;
        public Form2(string un, string psd)
        {
            InitializeComponent();
            createTable();

            dataGridView3.AutoGenerateColumns = false;
            dataGridView3.Refresh();

            // cn = new SqlConnection(@"Data Source=.\SQLEXPRESS ; Initial Catalog=pharmacy; Integrated Security=SSPI;");

            cn = new SqlConnection("Data Source=.; Initial Catalog=pharmacy; Integrated Security=SSPI;");

            height = Screen.PrimaryScreen.Bounds.Height;
            width = Screen.PrimaryScreen.Bounds.Width;

            //   button20.Hide();
            //    button22.Hide();

            lb_staffattend_name.Hide();
            lb_staffattend_id.Hide();
            label20.Hide();
            label21.Hide();


            if (un == "salesman" && psd == "salesman")
            {


                label34.Hide();
                label35.Hide();
                button19.Hide();

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Normal;
            a++;
            if (FormWindowState.Normal == 0 && a == 2)
            {
                this.WindowState = FormWindowState.Maximized;
                a--;
                a--;
            }

        }

        private void button38_Click(object sender, EventArgs e)
        {


        }

        private void button12_Click(object sender, EventArgs e)   //btn to delete medicine from medicne record. 
        {
            try
            {

                int rowindex = dataGridView1.CurrentCell.RowIndex;
                // int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                string value = dataGridView1.Rows[rowindex].Cells[0].Value.ToString();
                string q1 = "Delete from medicine_record where id=" + value;

                cn.Open();
                SqlCommand cmd = new SqlCommand(q1, cn);
                int v = cmd.ExecuteNonQuery();
                if (v > 0)
                {
                    MessageBox.Show("Record Deleted", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                dataGridView1.Refresh();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)    // btn to update medicine name , price and stock
        {
            try
            {

                /*cmd1 = new SqlCommandBuilder(adapt1);
                 adapt1.Update(tbl1);
                 MessageBox.Show("Updated"); */
                cn.Open();
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                // int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                string value = dataGridView1.Rows[rowindex].Cells[0].Value.ToString();
                string medi = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();
                string unit_p = dataGridView1.Rows[rowindex].Cells[2].Value.ToString();
                string loc = dataGridView1.Rows[rowindex].Cells[3].Value.ToString();
                string stck = dataGridView1.Rows[rowindex].Cells[4].Value.ToString();
                string manf = dataGridView1.Rows[rowindex].Cells[5].Value.ToString();
                string supp = dataGridView1.Rows[rowindex].Cells[6].Value.ToString();
                string mfg = dataGridView1.Rows[rowindex].Cells[7].Value.ToString();
                string exp = dataGridView1.Rows[rowindex].Cells[8].Value.ToString();

                DateTime ed = DateTime.Parse(exp);


                string q1 = "Update medicine_record set Expiry_Date='" + DateTime.Parse(exp).ToString("MM/dd/yyyy") + "', Mfg_Date='" + DateTime.Parse(mfg).ToString("MM/dd/yyyy") + "', supplier='" + supp + "', Manufacturer='" + manf + "',  location='" + loc + "',medicine='" + medi + "', unit_price='" + unit_p + "', stock='" + stck + "' where id=" + value;
                //MessageBox.Show(Convert.ToDateTime(exp).ToString());
                SqlCommand cmd = new SqlCommand(q1, cn);
                int v = cmd.ExecuteNonQuery();
                if (v > 0)
                {
                    MessageBox.Show("Record Updated", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                dataGridView1.Refresh();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cn.Close();
            }
        }

        private void button11_Click(object sender, EventArgs e)   // btn_POS
        {
            tabcontrol1.SelectedTab = Dashboard;
        }

        private void button10_Click(object sender, EventArgs e)   //btn_Medicine
        {
            tabcontrol1.SelectedTab = tabPage2;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabcontrol1.SelectedTab = tabPage1;
        }

        private void button19_Click(object sender, EventArgs e) //btn_monthly sales
        {
            try
            {
                label20.Text = "";
                label21.Text = "";
                label22.Text = "";
                cn.Open();
                string query = "select rec1.R_Id , rec1.Customer_Name , rec1.date , rec2.Medicine , rec2.Unit_Price , rec2.Qty , rec2.Sub_Total , rec1.Total , rec1.disc , rec1.disc_percentage , rec1.Grand_Total from rec1 inner join rec2 on rec1.R_Id=rec2.R_Id where month(rec1.date)='" + comboBox1.Text + "' and YEAR(rec1.date)='" + comboBox2.Text + "'";
                SqlCommand cmd = new SqlCommand(query, cn);
                adapt1 = new SqlDataAdapter(cmd);
                tbl1 = new DataTable();
                adapt1.Fill(tbl1);
                dgv4.DataSource = tbl1;
                string query1 = "select sum(Grand_Total) from rec1 where month(rec1.date)='" + comboBox1.Text + "' and YEAR(rec1.date)='" + comboBox2.Text + "'";
                string query2 = "select sum(Grand_Total) from rec1 where Customer_Name='321' and month(rec1.date)='" + comboBox1.Text + "' and YEAR(rec1.date)='" + comboBox2.Text + "'";
                SqlCommand cmd1 = new SqlCommand(query1, cn);
                SqlCommand cmd2 = new SqlCommand(query2, cn);
                float reader = float.Parse(cmd1.ExecuteScalar().ToString());
                float reader1 = 0;
                if (cmd2.ExecuteScalar().ToString() == "")
                { reader1 = 0; }
                else
                {
                    reader1 = float.Parse(cmd2.ExecuteScalar().ToString());
                }
                float gen = reader - reader1;
                label20.Text = gen.ToString();
                label21.Text = reader1.ToString();
                label22.Text = reader.ToString();
                cn.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Please Enter The Valid Date OR THERE WILL BE NO SALE");
                cn.Close();
            }


        }
        private void button7_Click(object sender, EventArgs e)  //btn  to insert medicine
        {
            try
            {
                if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
                {




                    String query1 = "insert into Medicine_record(Medicine,Unit_Price,Location,Stock,Manufacturer,Supplier,mfg_date,expiry_date) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";

                    query1 = String.Format(query1, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox17.Text, textBox16.Text);
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(query1, cn);
                    int val = cmd.ExecuteNonQuery();
                    if (val > 0)
                    {
                        MessageBox.Show("Record Inserted", "Inserted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = ""; textBox5.Text = ""; textBox6.Text = ""; textBox7.Text = ""; textBox17.Text = ""; textBox16.Text = "";
                    }

                    cn.Close();


                }
                else
                {
                    MessageBox.Show("All fields must be filled.", "Insertion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Please Fill the Appropriate Values ");
                MessageBox.Show(ex.Message, " Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cn.Close();

            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e) //txtBox to search medicine in Medicne Tab  
        {
            try
            {
                String query1 = "select * from medicine_record where medicine like '%" + textBox1.Text + "%'";
                cn.Open();
                SqlCommand cmd = new SqlCommand(query1, cn);
                adapt1 = new SqlDataAdapter(cmd);
                tbl1 = new DataTable();
                adapt1.Fill(tbl1);

                dataGridView1.DataSource = tbl1;
                dataGridView1.Columns[7].DefaultCellStyle.Format = "MM/dd/yyyy";
                dataGridView1.Columns[8].DefaultCellStyle.Format = "MM/dd/yyyy";
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e) //txtBox to search medicine in POS 
        {
            try
            {
                cn.Open();
                String query1 = "select * from medicine_record where medicine like '%" + textBox8.Text + "%'";
                //cn.Open();
                SqlCommand cmd = new SqlCommand(query1, cn);
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataTable tbl = new DataTable();
                adapt.Fill(tbl);
                dataGridView2.DataSource = tbl;
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                cn.Close();
            }
        }

        private void button13_Click(object sender, EventArgs e) //btn to add medicine in receipt
        {
            try
            {

                if (textBox9.Text == "")
                {
                    MessageBox.Show("Please Enter the Item Quantity.", "Insertion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int qty = Convert.ToInt32(textBox9.Text);
                    int rowindex = dataGridView2.CurrentCell.RowIndex;
                    // int columnindex = dataGridView2.CurrentCell.ColumnIndex;
                    string values = dataGridView2.Rows[rowindex].Cells[0].Value.ToString();
                    cn.Open();
                    string q = "select stock from medicine_record where id=" + values;
                    SqlCommand cm = new SqlCommand(q, cn);
                    int stock = (int)cm.ExecuteScalar();
                    cn.Close();
                    if (stock >= qty)
                    {
                        if (dataGridView2.SelectedCells.Count > 0)
                        {
                            chk = 0;
                            cn.Open();
                            string q1 = "select medicine from medicine_record where id=" + values;
                            string q2 = "select unit_price from medicine_record where id=" + values;
                            SqlCommand cmd = new SqlCommand(q1, cn);
                            SqlCommand cmd1 = new SqlCommand(q2, cn);
                            string medicine = (String)cmd.ExecuteScalar();
                            float up = float.Parse(cmd1.ExecuteScalar().ToString());
                            cn.Close();

                            for (int rows = 0; rows < dataGridView4.Rows.Count - 1; rows++)
                            {
                                r_med = dataGridView4.Rows[rows].Cells[0].Value.ToString();

                                if (r_med == medicine)
                                { chk = 2; }
                            }
                            if (chk == 0)
                            {
                                float st = (float)qty * up;

                                total = total + st;
                                if (textBox11.Text != "")

                                { dis = Convert.ToInt32(textBox11.Text); }
                                textBox12.Text = Convert.ToString(total - dis);
                                textBox18.Text = Convert.ToString(total);

                                addToTable(medicine, up, qty, st);

                                dataGridView4.DataSource = InvoiceTable.DefaultView;
                                dataGridView4.Refresh();

                                medicine = "";
                                textBox8.Text = "";
                                textBox9.Text = "";
                                textBox11.Text = "";
                                textBox14.Text = "";
                            }
                            else if (chk == 2)
                            {
                                MessageBox.Show("Medicine is already added.", " Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Available stock is less than the entered qty.", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.Close();
                    }

                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cn.Close();
            }

        }

        private void textBox11_TextChanged(object sender, EventArgs e) //Discount TextBox 
        {
            try
            {

                if (textBox14.Text == "")
                {
                    if (textBox11.Text != "")
                    {
                        int dis = Convert.ToInt32(textBox11.Text);
                        textBox12.Text = Convert.ToString(total - dis);
                    }
                    else
                    {
                        textBox12.Text = Convert.ToString(total);
                    }
                }
                else
                {
                    // MessageBox.Show("Make % discount field empty first", " Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox11.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static DataTable InvoiceTable = new DataTable();

        public object ReceiptPrinter { get; private set; }

        public void addToTable(string med, float up, int qty, float st)
        {
            InvoiceTable.Rows.Add(med, up, qty, st);
        }

        public void createTable()
        {
            InvoiceTable.Columns.Add("Medicine", typeof(String));
            InvoiceTable.Columns.Add("Unit_Price", typeof(float));
            InvoiceTable.Columns.Add("Qty", typeof(int));
            InvoiceTable.Columns.Add("Sub_Total", typeof(float));
        }


        private void button16_Click(object sender, EventArgs e)
        {
            try
            {

                String query = "insert into rec1(Customer_Name,Date,Grand_Total,disc,disc_percentage,total) values ('{0}',GETDATE(),'{1}','{2}','{3}','{4}') ";
                String query1 = "select max(R_Id) from rec1 ";
                query = String.Format(query, textBox10.Text, textBox12.Text, textBox11.Text, textBox14.Text, textBox18.Text);
                cn.Open();

                SqlCommand cmd = new SqlCommand(query, cn);

                cmd.ExecuteNonQuery();
                SqlCommand cmd1 = new SqlCommand(query1, cn);
                SqlDataReader reader = cmd1.ExecuteReader();

                if (reader.Read())
                {
                    R_Id = Convert.ToInt32(reader[0]);
                }


                cn.Close();

                for (int rows = 0; rows < dataGridView4.Rows.Count - 1; rows++)
                {
                    cn.Open();

                    r_med = dataGridView4.Rows[rows].Cells[0].Value.ToString();
                    r_up = float.Parse(dataGridView4.Rows[rows].Cells[1].Value.ToString());
                    r_qty = Convert.ToInt32(dataGridView4.Rows[rows].Cells[2].Value);
                    r_st = float.Parse(dataGridView4.Rows[rows].Cells[3].Value.ToString());
                    String query2 = "insert into rec2(R_Id,Medicine,Unit_Price,Qty,Sub_Total) values ('{0}','{1}',{2},'{3}','{4}') ";
                    query2 = string.Format(query2, R_Id, r_med, r_up, r_qty, r_st);
                    SqlCommand cmd2 = new SqlCommand(query2, cn);
                    cmd2.ExecuteNonQuery();

                    string q3 = "select stock from medicine_record where medicine='" + r_med + "'";
                    SqlCommand cmd3 = new SqlCommand(q3, cn);
                    int qt = (int)cmd3.ExecuteScalar();
                    qt = qt - r_qty;
                    string q4 = "update medicine_record set stock='" + qt + "' where medicine='" + r_med + "'";
                    SqlCommand cmd4 = new SqlCommand(q4, cn);
                    cmd4.ExecuteNonQuery();
                    cn.Close();
                }

                if (MessageBox.Show("Do you want to print the receipt?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    POS ps = new POS();
                    ps.print(textBox10.Text, textBox11.Text, textBox14.Text, textBox12.Text, R_Id.ToString(), textBox18.Text, dateTimePicker2.Value.ToString(), InvoiceTable);

                    /*
                    DGVPrinter printer = new DGVPrinter();
                    printer.Title = "\r\n\r\n IHS Medical Store ";
                    printer.SubTitle = "Commercial 2, Naval Anchorage, Islamabad \r\n Cell:0333-5604378\n\n\n" + "Customer Name: " + textBox10.Text + "\r\n Bill Id: " + R_Id + "\n\n";
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = "Discount : " + textBox11.Text + " _Rs \r\n" + "Discount % : " + textBox14.Text + " _% \r\n" + "\r\nGrand Total : " + textBox12.Text + " \r\n\n" + dateTimePicker2.Value + "\r\n\n FRIDGE ITEMS ARE NOT REFUNDABLE \n\r All ITEMS ARE RETURNABLE WITHIN 3 DAYS \n\r Home Delivery Seervice is also available";
                    printer.FooterSpacing = 15;
                    printer.PrintDataGridView(dataGridView4);
                    */
                }

                InvoiceTable.Clear();
                dataGridView4.Refresh();
                textBox14.Text = Convert.ToString(dis = 0);
                textBox12.Text = Convert.ToString(total = 0);
                textBox11.Text = Convert.ToString(dis = 0);

                textBox18.Text = "0";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cn.Close();
            }

        } // print button  

        private void checkBox1_CheckedChanged(object sender, EventArgs e) //Alert CheckBox
        {
            try
            {

                if (checkBox1.Checked == true)
                {
                    String query1 = "select * from medicine_record where stock <= 5 ";
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(query1, cn);
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    DataTable tbl = new DataTable();
                    adapt.Fill(tbl);
                    dataGridView1.DataSource = tbl;
                    dataGridView1.Columns[7].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dataGridView1.Columns[8].DefaultCellStyle.Format = "MM/dd/yyyy";

                    cn.Close();
                }
                else if (checkBox1.Checked == false)
                {
                    dataGridView1.DataSource = null;

                    dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)  // Discout %age
        {
            try
            {

                if (textBox11.Text == "")
                {
                    if (textBox14.Text != "" || textBox14.Text == "0")
                    {
                        int dis = Convert.ToInt32(textBox14.Text);
                        textBox12.Text = Convert.ToString(total - (total * dis / 100));
                    }
                    else
                    {
                        textBox12.Text = Convert.ToString(total);

                    }
                }
                else
                {
                    // MessageBox.Show("Make discount field empty first", " Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox14.Text = "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button21_Click(object sender, EventArgs e) // btn to del medicine from receipt  
        {
            try
            {
                if (dataGridView4.SelectedCells.Count > 0)
                {
                    cn.Open();
                    int rowindex = dataGridView4.CurrentCell.RowIndex;
                    int columnindex = dataGridView4.CurrentCell.ColumnIndex;
                    string val = dataGridView4.Rows[rowindex].Cells[0].Value.ToString();
                    string value = dataGridView4.Rows[rowindex].Cells[3].Value.ToString();

                    foreach (DataRow orow in InvoiceTable.Select())
                    {
                        if (orow["Medicine"].ToString().Equals(val))
                        {
                            InvoiceTable.Rows.Remove(orow);
                        }
                    }
                    total = total - float.Parse(value);
                    textBox18.Text = total.ToString();
                    textBox12.Text = total.ToString();
                    InvoiceTable.AcceptChanges();
                    dataGridView4.DataSource = InvoiceTable.DefaultView;
                    dataGridView4.Refresh();
                    textBox11.Text = "0";
                    if (textBox11.Text != "")
                    {
                        int dis = Convert.ToInt32(textBox11.Text);
                        textBox12.Text = Convert.ToString(total - dis);
                    }
                    else
                    {
                        textBox12.Text = Convert.ToString(total);

                    }

                    if (textBox14.Text != "")
                    {
                        int dis = Convert.ToInt32(textBox14.Text);
                        textBox12.Text = Convert.ToString(total - (total * dis / 100));
                    }
                    else
                    {

                        textBox12.Text = Convert.ToString(total);

                    }
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tabcontrol1.SelectedTab = tabPage3;
        }

        private void TextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button20_Click(object sender, EventArgs e) //Save Btn in Billing Record
        {
            try
            {

                String query1 = "select * from rec2 where R_Id = '{0}' ";
                query1 = string.Format(query1, textBox13.Text);

                textBox15.Text = "";
                label25.Text = "";
                label26.Text = "";


                cn.Open();
                SqlCommand cmd = new SqlCommand(query1, cn);
                adapt2 = new SqlDataAdapter(cmd);
                tbl2 = new DataTable();
                adapt2.Fill(tbl2);
                dataGridView5.DataSource = tbl2;

                String query2 = "select Customer_Name from rec1 where R_Id = '{0}'";
                query2 = String.Format(query2, textBox13.Text);
                string q3 = "select date from rec1 where R_Id = " + textBox13.Text;
                string q4 = "select Grand_Total from rec1 where R_Id = " + textBox13.Text;
                SqlCommand cmd1 = new SqlCommand(query2, cn);
                SqlCommand cmd2 = new SqlCommand(q3, cn);
                SqlCommand cmd3 = new SqlCommand(q4, cn);
                string reader = (string)cmd1.ExecuteScalar();
                DateTime reader1 = (DateTime)cmd2.ExecuteScalar();
                float reader2 = float.Parse(cmd3.ExecuteScalar().ToString());

                string q5 = "select disc from rec1 where R_Id = " + textBox13.Text;
                SqlCommand cmd5 = new SqlCommand(q5, cn);
                string disc = cmd5.ExecuteScalar().ToString();

                string q6 = "select disc_percentage from rec1 where R_Id = " + textBox13.Text;
                SqlCommand cmd6 = new SqlCommand(q6, cn);
                string discp = cmd6.ExecuteScalar().ToString();

                string q7 = "select total from rec1 where R_Id = " + textBox13.Text;
                SqlCommand cmd7 = new SqlCommand(q7, cn);
                string total = cmd7.ExecuteScalar().ToString();


                label26.Text = reader;
                label25.Text = reader1.ToString();
                textBox15.Text = reader2.ToString();
                label41.Text = disc;
                label39.Text = discp;
                label43.Text = total;


                POS ps = new POS();
                ps.print(reader, disc, discp, reader2.ToString(), textBox13.Text, total, reader1.ToString(), tbl2);



                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                cn.Close();
            }
        }

        private void Button22_Click(object sender, EventArgs e) //Del Btn in Billing Record
        {
            try
            {
                int rowindex = dataGridView5.CurrentCell.RowIndex;
                int columnindex = dataGridView5.CurrentCell.ColumnIndex;
                string value = dataGridView5.Rows[rowindex].Cells[0].Value.ToString();
                string q1 = "Delete from rec2 where S_No=" + value;
                cn.Open();
                SqlCommand cmd = new SqlCommand(q1, cn);
                int v = cmd.ExecuteNonQuery();
                if (v > 0)
                {
                    MessageBox.Show("Record Deleted", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cn.Close();
                }

                String query1 = "select * from rec2 where R_Id = '{0}' ";
                query1 = string.Format(query1, textBox13.Text);

                cn.Open();
                SqlCommand cmd1 = new SqlCommand(query1, cn);
                adapt2 = new SqlDataAdapter(cmd1);
                tbl2 = new DataTable();
                adapt2.Fill(tbl2);
                dataGridView5.DataSource = tbl2;

                dataGridView5.Refresh();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cn.Close();
            }
        }

        private void Label34_Click(object sender, EventArgs e)  //Lbl to show amount of available Stock
        {
            try
            {
                string query = "select sum(stock*Unit_price) from medicine_record";
                cn.Open();
                SqlCommand cmd = new SqlCommand(query, cn);
                float total_sum = float.Parse(cmd.ExecuteScalar().ToString());
                label35.Text = total_sum.ToString();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (checkBox2.Checked == true)
                {
                    dataGridView1.DataSource = null;
                    String query1 = " select * from medicine_record where DATEDIFF(month,CONVERT(date, getdate()),expiry_date)<=6";
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(query1, cn);
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    DataTable tbl = new DataTable();
                    adapt.Fill(tbl);
                    dataGridView1.DataSource = tbl;
                    dataGridView1.Columns[7].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dataGridView1.Columns[8].DefaultCellStyle.Format = "MM/dd/yyyy";

                    cn.Close();
                }
                else if (checkBox2.Checked == false)
                {
                    dataGridView1.DataSource = null;

                    dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TextBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void dgv4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void Dashboard_Click(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void label43_Click(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)   //Daily Sales Button
        {
            try
            {
                cn.Open();
                string query = "select rec1.R_Id , rec1.Customer_Name , rec1.date , rec2.Medicine , rec2.Unit_Price , rec2.Qty , rec2.Sub_Total , rec1.Total , rec1.disc , rec1.disc_percentage , rec1.Grand_Total from rec1 inner join rec2 on rec1.R_Id=rec2.R_Id where  day(rec1.date)='" + comboBox3.Text + "'and " + "month(rec1.date)='" + comboBox4.Text + "' and YEAR(rec1.date)='" + comboBox5.Text + "'";
                SqlCommand cmd = new SqlCommand(query, cn);
                adapt1 = new SqlDataAdapter(cmd);
                tbl1 = new DataTable();
                adapt1.Fill(tbl1);
                dgv4.DataSource = tbl1;
                string query1 = "select sum(Grand_Total) from rec1 where  day(rec1.date)='" + comboBox3.Text + "'and " + "month(rec1.date)='" + comboBox4.Text + "' and YEAR(rec1.date)='" + comboBox5.Text + "'";
                string query2 = "select sum(Grand_Total) from rec1 where Customer_Name='321' and day(rec1.date)='" + comboBox3.Text + "'and " + "month(rec1.date)='" + comboBox4.Text + "' and YEAR(rec1.date)='" + comboBox5.Text + "'";
                SqlCommand cmd1 = new SqlCommand(query1, cn);
                SqlCommand cmd2 = new SqlCommand(query2, cn);
                float reader = float.Parse(cmd1.ExecuteScalar().ToString());

                float reader1 = 0;
                if (cmd2.ExecuteScalar().ToString() == "")
                { reader1 = 0; }
                else
                {
                    reader1 = float.Parse(cmd2.ExecuteScalar().ToString());
                }

                float gen = reader - reader1;
                label20.Text = gen.ToString();
                label21.Text = reader1.ToString();
                label22.Text = reader.ToString();
                cn.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Please Enter The Valid Date OR THERE WILL BE NO SALE");
                cn.Close();
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)  //Btn to find Receipt Record  
        {
            try
            {

                String query1 = "select * from rec2 where R_Id = '{0}' ";
                query1 = string.Format(query1, textBox13.Text);

                textBox15.Text = "";
                label25.Text = "";
                label26.Text = "";
                label39.Text = "";
                label43.Text = "";


                cn.Open();
                SqlCommand cmd = new SqlCommand(query1, cn);
                adapt2 = new SqlDataAdapter(cmd);
                tbl2 = new DataTable();
                adapt2.Fill(tbl2);
                dataGridView5.DataSource = tbl2;

                String query2 = "select Customer_Name from rec1 where R_Id = '{0}'";
                query2 = String.Format(query2, textBox13.Text);
                string q3 = "select date from rec1 where R_Id = " + textBox13.Text;
                string q4 = "select Grand_Total from rec1 where R_Id = " + textBox13.Text;
                SqlCommand cmd1 = new SqlCommand(query2, cn);
                SqlCommand cmd2 = new SqlCommand(q3, cn);
                SqlCommand cmd3 = new SqlCommand(q4, cn);
                string reader = (string)cmd1.ExecuteScalar();
                DateTime reader1 = (DateTime)cmd2.ExecuteScalar();
                float reader2 = float.Parse(cmd3.ExecuteScalar().ToString());

                string q5 = "select disc from rec1 where R_Id = " + textBox13.Text;
                SqlCommand cmd5 = new SqlCommand(q5, cn);
                string disc = cmd5.ExecuteScalar().ToString();

                string q6 = "select disc_percentage from rec1 where R_Id = " + textBox13.Text;
                SqlCommand cmd6 = new SqlCommand(q6, cn);
                string discp = cmd6.ExecuteScalar().ToString();

                string q7 = "select total from rec1 where R_Id = " + textBox13.Text;
                SqlCommand cmd7 = new SqlCommand(q7, cn);
                string total = cmd7.ExecuteScalar().ToString();


                label26.Text = reader;
                label25.Text = reader1.ToString();
                textBox15.Text = reader2.ToString();
                label41.Text = disc;
                label39.Text = discp;
                label43.Text = total;

                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please Enter The Valid Recepit ID ");
                //MessageBox.Show(ex.ToString());
                cn.Close();
            }

        }

        private void button18_Click_1(object sender, EventArgs e) // Updating Receipt in case of return   
        {
            try
            {

                int rowindex = dataGridView5.CurrentCell.RowIndex;
                int columnindex = dataGridView5.CurrentCell.ColumnIndex;
                string value = dataGridView5.Rows[rowindex].Cells[0].Value.ToString();
                string unit_price = dataGridView5.Rows[rowindex].Cells[3].Value.ToString();
                string qty = dataGridView5.Rows[rowindex].Cells[4].Value.ToString();
                float sub_total = (float.Parse(unit_price) * float.Parse(qty));
                //  int sub_total = 50;
                string q1 = "update rec2 set qty='" + qty + "', sub_total='" + sub_total + "' where S_No=" + value;
                cn.Open();
                SqlCommand cmd = new SqlCommand(q1, cn);
                int v = cmd.ExecuteNonQuery();
                if (v > 0)
                {
                    MessageBox.Show("Record Updated", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cn.Close();
                }

                String query1 = "select * from rec2 where R_Id = '{0}' ";
                query1 = string.Format(query1, textBox13.Text);

                cn.Open();
                SqlCommand cmd1 = new SqlCommand(query1, cn);
                adapt2 = new SqlDataAdapter(cmd1);
                tbl2 = new DataTable();
                adapt2.Fill(tbl2);
                dataGridView5.DataSource = tbl2;
                dataGridView5.Refresh();

                string q4 = "select sum(sub_total) from rec2 where R_Id=" + textBox13.Text;
                SqlCommand cmd3 = new SqlCommand(q4, cn);
                float reader = float.Parse(cmd3.ExecuteScalar().ToString());
                textBox15.Text = reader.ToString();

                label43.Text = reader.ToString();

                string q5 = "update rec1 set Total='" + reader + "', Grand_Total ='" + reader + "' where R_Id=" + textBox13.Text;
                SqlCommand cmd4 = new SqlCommand(q5, cn);
                cmd4.ExecuteNonQuery();



                cn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                cn.Close();
            }
        }
    }
}

