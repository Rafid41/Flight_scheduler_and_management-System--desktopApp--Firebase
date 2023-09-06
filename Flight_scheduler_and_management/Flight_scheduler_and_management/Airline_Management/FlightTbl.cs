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
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;



namespace Airline_Management
{
    public partial class FlightTbl : Form
    {


        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Firebase auth",
            BasePath = "database url"
        };
        IFirebaseClient client;


        public FlightTbl()
        {
            InitializeComponent();
            DepHCb.SelectedIndex = 0;
            DepMCb.SelectedIndex = 0;
            ArHCb.SelectedIndex = 1;
            ArMCb.SelectedIndex = 0;
        }

        //SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\AirlineDb.mdf;Integrated Security=True;Connect Timeout=30");

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void FlightTbl_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                /* if (client != null)
                {
                    MessageBox.Show("Connection Success");
                }
               */
            }
            catch
            {
                MessageBox.Show("Connection Failed");
            }
        }

        string arrivalTime, departureTime, duration;
        int depH, depM, arH, arM, dep, ar;

        int durM, durH, c;
       


        private void TimeCalculation()
        {
            depH = int.Parse(DepHCb.SelectedItem.ToString());
            depM = int.Parse(DepMCb.SelectedItem.ToString());
            arH = int.Parse(ArHCb.SelectedItem.ToString());
            arM = int.Parse(ArMCb.SelectedItem.ToString());

            dep = depH * 60 + depM;
            ar = arH * 60 + arM;

            c = ar - dep;

            departureTime = DepHCb.SelectedItem.ToString() + " : " + DepMCb.SelectedItem.ToString();
            arrivalTime = ArHCb.SelectedItem.ToString() + " : " + ArMCb.SelectedItem.ToString();

            durH = (int)(c / 60.0);
            durM = c - (durH * 60);

            duration = durH.ToString() + " : " + durM.ToString();          
        }

        private void button1_Click(object sender, EventArgs e)
        {

            TimeCalculation();

            if (FcodeTb.Text == "" || Fsrc.SelectedItem.ToString() == "" || FDest.SelectedItem.ToString() == "" || FDate.Text == "" || SeatNum.Text == "" || FstatusCb.Text=="")
            {
                MessageBox.Show("Missing Information");
            }
            else if (Fsrc.SelectedItem.ToString() == FDest.SelectedItem.ToString())
            {
                MessageBox.Show("Source and Destination can't be same");
            }
            else if (dep >= ar)
            {
                MessageBox.Show("Arrival Time will have to be greater than Departure Time");
            }
            else
            {
                try
                {
                   

                    AddFlight_class adp = new AddFlight_class()
                    {
                        Fcode = FcodeTb.Text,
                        Fsrc = Fsrc.SelectedItem.ToString(),
                        FDest = FDest.SelectedItem.ToString(),
                        FDate = FDate.Value.ToString(),
                        FDepartureTime = departureTime,
                        FArrivalTime = arrivalTime,
                        FCap = SeatNum.Text,
                        FStatus = FstatusCb.SelectedItem.ToString(),
                        FDuration= duration

                    };

                    //For Unique id use Push() instead of Set()
                    FirebaseResponse response = client.Set("ScheduleTbl/" + FcodeTb.Text, adp);
                    MessageBox.Show("Save Successful");
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FcodeTb.Text = "";
            SeatNum.Text = "";
            FDest.Text = "";
            Fsrc.Text = "";
            FDate.Text = "";
            FstatusCb.Text = "";


        }

        private void button3_Click(object sender, EventArgs e)
        {
            ViewFlights viewflt = new ViewFlights();
            viewflt.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void FDest_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FDate_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
