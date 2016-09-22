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
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd = new SqlCommand();
        string sql = null;
        SqlDataReader reader;
        DateTime dating;
        bool booked=false, isB=false, isC=false;
        Random rd = new Random();
        int tick;
        string [] loc =   { "Ijoko","Agege","Ibadan","Oshogbo","Ilorin"};
        string[] tim = { "Economy", "Business Class", "First Class" };
        public Form1()
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
        string departure, arrival, timer, travelType, cardtype;
        int cardcvv, seatno;
        Int64 cardno;
        private void label11_Click(object sender, EventArgs e)
        {
            
            Form2 f = new Form2();
            f.Show();
            this.Hide();
        }

        private void proceed_Click(object sender, EventArgs e)
        {
            getAllFields();
            if (String.IsNullOrEmpty(departure) || String.IsNullOrEmpty(arrival) || String.IsNullOrEmpty(timer) || String.IsNullOrEmpty(travelType) || String.IsNullOrEmpty(cardtype))
            {
                MessageBox.Show("One of the list is not selected","ATTENTION!!!",MessageBoxButtons.RetryCancel,MessageBoxIcon.Error);
            }
            else if (String.Equals(departure, arrival))
            {
                MessageBox.Show("Same Location Selected, Not Possible", "ATTENTION!!!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else if (seatno == 0)
            {
                MessageBox.Show("Seat Number cant be Zero", "ATTENTION!!!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else if (cardno.ToString().Length != 16)
            {
                MessageBox.Show("Card Number Must be 16 digits", "ATTENTION!!!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else if (cardcvv.ToString().Length != 3)
            {
                MessageBox.Show("CVV must be 3 digits", "ATTENTION!!!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else
            {
                getAmount(departure, arrival, travelType);
                TravelTypeAdded(travelType);
                getArrivalTime(departure, arrival);
                insertIntoBooking();
                
            }
//            MessageBox.Show(seatno.ToString());
            
        }
        
        private void insertIntoBooking()
        {
            con.Open();
            reader = new SqlCommand("SELECT * from booking", con).ExecuteReader();
            int k = 0;
            if (reader.HasRows)
            {
                while (reader.Read())
                    k++;
            }
            con.Close();
            try
            {
                sql = "INSERT into booking ([id],[fromm],[to],[time],[type],[seat],[date],[ticket],[arrival],[amount]) values (@r,@a,@b,@c,@d,@e,@f,@g,@h,@i)";
                con.Open();
                using (SqlCommand cd = new SqlCommand(sql, con))
                {
                    k++;
                    cd.Parameters.AddWithValue("@r", k);
                    cd.Parameters.AddWithValue("@a", departure);
                    cd.Parameters.AddWithValue("@b", arrival);
                    cd.Parameters.AddWithValue("@c", timer);
                    cd.Parameters.AddWithValue("@d", travelType);
                    cd.Parameters.AddWithValue("@e", seatno);
                    cd.Parameters.AddWithValue("@f", dating);
                    cd.Parameters.AddWithValue("@g",tick);
                    cd.Parameters.AddWithValue("@h",finalTime);
                    cd.Parameters.AddWithValue("@i",amt);
                    cd.ExecuteNonQuery();
                    isB = true;
                }
                con.Close();
                insertIntoCard();
                isBooked(isB, isC);
                generateSlip(booked);
            }
            catch (Exception e)
            {
                con.Close();
                MessageBox.Show("Sorry, The Following has been booked by another customer:\nDeparture: " + departure + "\nArrival: " + arrival + "\nTime:"+
                    timer + "\nClass: " + travelType + "\nSeat Number: " + seatno + "\nStatus: Allocated.\nPlease kindly book a different request.");
            }
            
        }
        double finalTime = 0;
        private void getArrivalTime(string departure, string arrival)
        {
            int pre = 0, post = 0, dist;
            for (int i = 0; i < loc.Length; i++)
            {
                if (String.Equals(loc[i], departure))
                    pre = i + 1;
                if (String.Equals(loc[i], arrival))
                    post = i + 1;
            }
            if (pre > post)
            {
                dist = pre - post;
            }
            else
            {
                dist = post - pre;
            }
            finalTime = dist * 90;
        }

        private void TravelTypeAdded(string travelType)
        {
            int added = 0;
            for (int i = 0; i < tim.Length; i++)
            {
                if (String.Equals(tim[i], travelType))
                    added = i ;
            }
            int accm = added * 400;
            amt += accm;
        }
        double amt = 0.0;
        private void getAmount(string departure, string arrival, string travelType)
        {
            int pre=0,post=0,dist;
            for (int i = 0; i < loc.Length; i++)
            {
                if (String.Equals(loc[i], departure))
                    pre = i+1;
                if (String.Equals(loc[i], arrival))
                    post = i+1;
            }
            if (pre > post)
            {
                dist = pre - post;
            }
            else
            {
                dist = post - pre;
            }
            amt = dist * 625;
        }

        private void generateSlip(bool booked)
        {
            
            if (booked)
            {
                DialogResult dr = MessageBox.Show("Click here to confirm your payment and generate receipt","Please...",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {

                    string res = "******************************************************************************\n* Ticket No: " + tick + "\t\t\t\tSeat Number: " + seatno + "\n*\n* Departure: " +
                        departure + " \t\t\t\tArrival: " + arrival + "\n*\n* Train Take Off: " + timer + " \tArrival Time: " + finalTime + "min.\n*\n*Amount: " + amt + "\tDate and Time of Booking: " + dating.ToString() + "\n*" +
                        "\n*Kindly Screenshot or Print this slip as a means of payment. Clicking OK discards this message,Proceed?\n*\n*******************************************************************************";
                   DialogResult dd =  MessageBox.Show(res,"Success",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
                   if (dd == System.Windows.Forms.DialogResult.Cancel)
                   {
                       string fin = "******************************************************************************\n* Ticket No: " + tick + "\t\t\t\tSeat Number: " + seatno + "\n*\n* Departure: " +
                       departure + " \t\t\t\tArrival: " + arrival + "\n*\n* Train Take Off: " + timer + " \tArrival Time: " + finalTime + "min.\n*\n*Amount: " + amt + "\tDate and Time of Booking: " + dating.ToString() + "\n*" +
                       "\n*Kindly Screenshot or Print this slip as a means of payment.\n*\n*******************************************************************************";
                       DialogResult d = MessageBox.Show(fin, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   }
                }
            }
        }

        private void isBooked(bool isB, bool isC)
        {
            if (isB && isC)
                booked = true;
        }

        private void insertIntoCard()
        {
            con.Open();
            reader = new SqlCommand("SELECT * from card", con).ExecuteReader();
            int j = 0;
            if (reader.HasRows)
            {
                while (reader.Read())
                    j++;
            }
            con.Close();
            sql = "INSERT into card ([id],[type],[number],[cvv],[date],[ticket],[amount]) values (@e,@a,@b,@c,@d,@f,@g)";
            con.Open();
            using (SqlCommand cd = new SqlCommand(sql, con))
            {
                cd.Parameters.AddWithValue("@e", j);
                cd.Parameters.AddWithValue("@a", cardtype);
                cd.Parameters.AddWithValue("@b", cardno);
                cd.Parameters.AddWithValue("@c", cardcvv);
                cd.Parameters.AddWithValue("@d", dating);
                cd.Parameters.AddWithValue("@f", tick);
                cd.Parameters.AddWithValue("@g",amt);
                cd.ExecuteNonQuery();
                isC = true;
            }
            con.Close();
        }

        private void getAllFields()
        {
            departure = depart.Text.ToString();
            arrival = arrive.Text.ToString();
            timer = time.Text.ToString();
            travelType = classT.Text.ToString();
            cardtype = cardT.Text.ToString();
            seatno = Convert.ToInt16(seat.Value);
            cardcvv = Convert.ToInt16(cvvNumber.Text);
            cardno = Convert.ToInt64(cardNumber.Text);
            dating = DateTime.Now;
            tick = (int)(rd.NextDouble() * 1000);
            
        }

        private void cardNumber_TextChanged(object sender, EventArgs e)
        {
            string num = cardNumber.Text;
            for (int i = 0; i <  num.Length; i++)
            {
                char a = num[i];
                if (!char.IsDigit(a)){
                    MessageBox.Show("Only Numbers Allowed");
                }
            }
        }

        private void cvvNumber_TextChanged(object sender, EventArgs e)
        {
            string num = cardNumber.Text;
            for (int i = 0; i < num.Length; i++)
            {
                char a = num[i];
                if (!char.IsDigit(a))
                {
                    MessageBox.Show("Only Numbers Allowed");
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("You are about to exit the application, Proceed???", "Attention!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label11_Click_1(object sender, EventArgs e)
        {
            Form4 r = new Form4();
            r.Show();
            this.Hide();
        }
    }
}
