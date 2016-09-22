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
    public partial class Form3 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        string sql = null;
        public Form3()
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

        private void bookingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = "SELECT * FROM booking";
            cmd = new SqlCommand(sql,con);
            SqlDataAdapter myAdapter = new SqlDataAdapter();
            myAdapter.SelectCommand = cmd;
            DataTable myDt = new DataTable();
            myAdapter.Fill(myDt);
            bookingView.DataSource = myDt;
        }

        private void cardsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = "SELECT * FROM card";
            cmd = new SqlCommand(sql, con);
            SqlDataAdapter myAdapter = new SqlDataAdapter();
            myAdapter.SelectCommand = cmd;
            DataTable myDt = new DataTable();
            myAdapter.Fill(myDt);
            cardView.DataSource = myDt;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("You are about to exit the application, youd be logged out and the application will terminate. Proceed???", "Attention!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
            
        }
    }
}
