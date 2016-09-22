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
    public partial class Form5 : Form
    {
        string stat;
        SqlConnection con;
        SqlCommand cmd = new SqlCommand();
        string sql = null;
        public Form5()
        {
            InitializeComponent();
            Pass myP = new Pass();
            stat = myP.returnStation();
            //MessageBox.Show("This is "+stat);
            try
            {
                con = new SqlConnection(Properties.Settings.Default.trainerConnectionString);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
        }

        private void bookingForStationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = "SELECT * FROM booking WHERE fromm = @s";
             con.Open();
             SqlCommand cd = new SqlCommand(sql, con);
                 cd.Parameters.AddWithValue("@s", stat);
                 cd.ExecuteNonQuery();
                 SqlDataAdapter myAdp = new SqlDataAdapter();
                 myAdp.SelectCommand = cd;
                 DataTable myDt = new DataTable();
                 myAdp.Fill(myDt);
                 dataGridView1.DataSource = myDt;

                 con.Close();
        }

        private void quiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("You are about to exit the application, youd be logged out and the application will terminate. Proceed???", "Attention!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }

        }
    }
}
