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
    public partial class ViewFlights : Form
    {
        public ViewFlights()
        {
            InitializeComponent();

            SrcCb.SelectedIndex = 1;
            DstCb.SelectedIndex = 2;
            DepHCb.SelectedIndex = 0;
            DepMCb.SelectedIndex = 0;
            ArHCb.SelectedIndex = 1;
            ArMCb.SelectedIndex = 0;
        }


        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Firebase auth",
            BasePath = "database url"
        };
        IFirebaseClient client;

        
     
        
        private void populate()
        {
           

            FlightDGV.DataSource = null;
            FlightDGV.Rows.Clear();

            FirebaseResponse response = client.Get("ScheduleTbl/");
            Dictionary<string, AddFlight_class> getFlight = response.ResultAs<Dictionary<string, AddFlight_class>>();

            foreach (var get in getFlight)
            {
                FlightDGV.Rows.Add(

                    get.Value.Fcode,
                    get.Value.Fsrc,
                    get.Value.FDest,
                    get.Value.FDate,
                    get.Value.FDepartureTime,
                    get.Value.FArrivalTime,
                    get.Value.FDuration,
                    get.Value.FCap,
                    get.Value.FStatus

                    );
            }

        }

        private void ViewFlights_Load(object sender, EventArgs e)
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
            populate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FlightTbl Addf1 = new FlightTbl();
            Addf1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FcodeTb.Text = "";
            Seatnum.Text = "";
        }

        private void FlightDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            FcodeTb.Text = FlightDGV.SelectedRows[0].Cells[0].Value.ToString();
            SrcCb.Text = FlightDGV.SelectedRows[0].Cells[1].Value.ToString();
            DstCb.Text = FlightDGV.SelectedRows[0].Cells[2].Value.ToString();
            Seatnum.Text = FlightDGV.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (FcodeTb.Text == "")
            {
                MessageBox.Show("Enter The Flight to Delete");
            }
            else
            {
                try
                {
                    

                    FirebaseResponse response = client.Delete("ScheduleTbl/" + FcodeTb.Text);
                    MessageBox.Show("Flight Deleted Successfully");


                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        string arrivalTime, departureTime, duration;
        int depH, depM, arH, arM, dep, ar;

        

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            populate();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {

            FlightDGV.DataSource = null;
            FlightDGV.Rows.Clear();
            int b = 0;

            FirebaseResponse response = client.Get("ScheduleTbl/");
            Dictionary<string, AddFlight_class> getData = response.ResultAs<Dictionary<string, AddFlight_class>>();

            foreach (var get in getData)
            {
                if (FcodeTb.Text == get.Value.Fcode || SrcCb.SelectedItem.ToString() == get.Value.Fsrc || DstCb.SelectedItem.ToString() == get.Value.FDest)
                {
                    b = 1;

                    FlightDGV.Rows.Add(

                         get.Value.Fcode,
                         get.Value.Fsrc,
                         get.Value.FDest,
                         get.Value.FDate,
                         get.Value.FDepartureTime,
                         get.Value.FArrivalTime,
                         get.Value.FDuration,
                         get.Value.FCap,
                         get.Value.FStatus
                   );
                }
            }

            if (b == 0)
            {
                MessageBox.Show("No Data Found");
            }
        }

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

            if (FcodeTb.Text == "" || Seatnum.Text == "" || StatusCb.SelectedItem=="" || DepHCb.SelectedItem == "" || DepMCb.SelectedItem == "" || ArHCb.SelectedItem == "" || ArMCb.SelectedItem == "")
            { 
                MessageBox.Show("Missing Information");
            }
            else if (SrcCb.SelectedItem.ToString() == DstCb.SelectedItem.ToString())
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
                   

                    var flight = new AddFlight_class
                    {
                        Fcode = FcodeTb.Text,
                        Fsrc = SrcCb.SelectedItem.ToString(),
                        FDest = DstCb.SelectedItem.ToString(),
                        FDate = FDate.Value.ToString(),
                        FCap = Seatnum.Text,
                        FStatus = StatusCb.SelectedItem.ToString(),
                        FDepartureTime = departureTime,
                        FArrivalTime = arrivalTime,
                        FDuration = duration

                    };

                    FirebaseResponse response = client.Update("ScheduleTbl/" + FcodeTb.Text, flight);
                    MessageBox.Show("Update Successful");

                    populate();


                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Missing Information");
                }
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }







        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap objbmp = new Bitmap(this.FlightDGV.Width, this.FlightDGV.Height);
            FlightDGV.DrawToBitmap(objbmp, new Rectangle(0, 0, this.FlightDGV.Width, this.FlightDGV.Height));

            e.Graphics.DrawImage(objbmp, 10, 300);
            e.Graphics.DrawString(label2.Text, new Font("Times New Roman", 26, FontStyle.Bold), Brushes.Red, new Point(260, 230));

        }

        private void Printbtn_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
    }
}
