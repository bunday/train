using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Train_Ticketting
{
    public partial class Form2 : Form
    {
        SqlConnection con;
        SqlCommand cmd = new SqlCommand();
        string sql = null;
        string name, pswd;
        public Form2()
        {
            InitializeComponent();

            try
            {
                con = new SqlConnection(Properties.Settings.Default.trainerConnectionString);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            //addAdmin();
        }

        private void addAdmin()
        {
            sql = "INSERT into login ([id],[uname],[pass]) values (@r,@a,@b)";
            con.Open();
            using (SqlCommand cd = new SqlCommand(sql, con))
            {
                
                cd.Parameters.AddWithValue("@r", 1);
                cd.Parameters.AddWithValue("@a", "silent");
                cd.Parameters.AddWithValue("@b", "added");
                cd.ExecuteNonQuery();
                MessageBox.Show("Done");
            }
            con.Close();

        }

        private void seal(object sender, FormClosedEventArgs e)
        {
            Form1 a = new Form1();
            a.Close();
        }

        private void bLogin_Click(object sender, EventArgs e)
        {
            name = uname.Text.ToString();
            pswd = pass.Text.ToString();
            sql = "SELECT * FROM login WHERE uname = @u AND pass = @p";
           
            con.Open();
            using (SqlCommand cd = new SqlCommand(sql, con))
            {
                
                cd.Parameters.AddWithValue("@u", name);
                cd.Parameters.AddWithValue("@p", pswd);
                cd.ExecuteNonQuery();
                try
                {
                    String d = cd.ExecuteScalar().ToString();
                    DialogResult dr = MessageBox.Show("Welcome " + name+". Proceed???","Administrative",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        Form3 k = new Form3();
                        k.Show();
                        this.Hide();
                    }
                }
                catch (Exception te)
                {
                    MessageBox.Show(te.ToString(),"Invalid Login",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
                }
             
            }
            con.Close();

        }
    }
}
