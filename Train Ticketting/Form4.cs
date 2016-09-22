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
    public partial class Form4 : Form
    {
        Pass myP = new Pass();
        public Form4()
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
        }
        SqlConnection con;
        SqlCommand cmd = new SqlCommand();
        string sql = null;
        SqlDataReader reader;

        public string una; 
        string pswd;
        private void bLogin_Click(object sender, EventArgs e)
        {
            una = station.Text.ToString();
            pswd = passd.Text.ToString();
            if (String.IsNullOrEmpty(una) || String.IsNullOrEmpty(pswd))
            {
                MessageBox.Show("Error");
            }
            else
            {
                sql = "SELECT * FROM checker WHERE station = @u AND state = @p";

                con.Open();
                using (SqlCommand cd = new SqlCommand(sql, con))
                {

                    cd.Parameters.AddWithValue("@u", una);
                    cd.Parameters.AddWithValue("@p", pswd);
                    cd.ExecuteNonQuery();
                    try
                    {
                        myP.getStation(station.Text);
                        String d = cd.ExecuteScalar().ToString();
                        DialogResult dr = MessageBox.Show("Welcome " + una + " Train Station Officer. Proceed???", "Administrative", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            Form5 k = new Form5();
                            k.Show();
                            this.Hide();
                        }
                    }
                    catch (Exception te)
                    {
                        MessageBox.Show(te.ToString(), "Invalid Login", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    }

                }
                con.Close();

            }
            
        }
    }
}
