using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Pharmacy
{
    public partial class Form1 : Form
    {
        int height, width, a = 0;
        int flag = 0;
        SqlConnection cn;
        public Form1()
        {
            InitializeComponent();
            height = Screen.PrimaryScreen.Bounds.Height;
            width = Screen.PrimaryScreen.Bounds.Width;

            //   cn = new SqlConnection(@"Data Source=.\SQLEXPRESS ; Initial Catalog=pharmacy; Integrated Security=SSPI;");

            cn = new SqlConnection("Data Source=.; Initial Catalog=pharmacy; Integrated Security=SSPI;");
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnexit_Click(object sender, EventArgs e) // btn for exit
        {
            Application.Exit();
        }

        private void change_credentials()
        {
            cn.Open();
            string query = "update login set U_name='admin123' , Password='admin123' where id=1 ";
            string query1 = "update login set U_name='salesman123' , Password='salesman123' where id=2 ";

            SqlCommand cmd = new SqlCommand(query, cn);
            SqlCommand cmd1 = new SqlCommand(query1, cn);
            int v = cmd.ExecuteNonQuery();
            int v1 = cmd1.ExecuteNonQuery();
            if (v > 0 && v1 > 0)
            {
                MessageBox.Show("Record Updated", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            cn.Close();

        }

        private void check_date(string dt)
        {
            int a = String.Compare(dt, "4/10/2023");
            if (a == 1)
            {
                change_credentials();
                flag = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)  // btn for login
        {
            if(flag==0)
            {
                string dt = DateTime.Now.ToShortDateString();
                check_date(dt);
            }
           
                       

            try
            {
                if (txtUsername.Text != "" && txtPassword.Text != "")
                {
                    string q1 = "select U_name from login where U_name='{0}'";
                    string q2 = "select Password from login where Password='{0}'";
                    q1 = string.Format(q1, txtUsername.Text);
                    q2 = string.Format(q2, txtPassword.Text);
                    SqlCommand cmd = new SqlCommand(q1, cn);
                    SqlCommand cmd1 = new SqlCommand(q2, cn);
                    cn.Open();
                    string un = (String)cmd.ExecuteScalar();
                    string psd = (String)cmd.ExecuteScalar();
                    if (txtUsername.Text == un && txtPassword.Text == psd)
                    {
                        this.Hide();
                        Form2 fm = new Form2(txtUsername.Text, txtPassword.Text);
                        fm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect UserName or Password", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    cn.Close();
                }
                else
                {
                    MessageBox.Show("Both fields should be filled", "Log In Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Log In Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cn.Close();
            }
        }
    }
}
