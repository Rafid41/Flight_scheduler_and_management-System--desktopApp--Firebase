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
    public partial class Ticket : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Firebase auth",
            BasePath = "database url"
        };
        IFirebaseClient client;

        public Ticket()
        {
            InitializeComponent();
        }

        private void Ticket_Load(object sender, EventArgs e)
        {

            try
            {
                client = new FireSharp.FirebaseClient(config);
                if (client != null)
                {
                    MessageBox.Show("Connection Success");
                }
            }
            catch
            {
                MessageBox.Show("Connection Failed");
            }

            fillPassenger();
            fillFlightCode();
            //populate();
        }




        //SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\AirlineDb.mdf;Integrated Security=True;Connect Timeout=30");





        private void button1_Click(object sender, EventArgs e)
        {
            if (Tid.Text == "" || PNameTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    /*
                    Con.Open();
                    string query = "insert into TicketTbl values(" + Tid.Text + ",'" + FCodeCb.SelectedValue.ToString() + "'," + PIdCb.SelectedValue.ToString() + ",'" + PNameTb.Text + "','" + PPassTb.Text + "','" + PNatTb.Text + "'," + PAmtTb.Text + ")";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Ticket Bookeded Successfully");
                    Con.Close();
                    */

                    Ticket_class Tkt = new Ticket_class()
                    {
                        TId = Tid.Text,
                        Fcode = FCodeCb.SelectedValue.ToString(),
                        PId = PIdCb.SelectedValue.ToString(),
                        PName = PNameTb.Text,
                        PPass = PPassTb.Text,
                        PNation = PNatTb.Text,
                        Amt = PAmtTb.Text

                    };

                    FirebaseResponse response = client.Set("TicketTbl/" + Tid.Text, Tkt);
                    MessageBox.Show("Ticket Booked Successfully");



                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }



        private void populate()
        {
            /*
            Con.Open();
            string query = "select * from TicketTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TicketDGV.DataSource = ds.Tables[0];
            Con.Close();
            

            Ticket_class Tkt = new Ticket_class()
            {
                TId = "1",
                Fcode = "10",
                PId = "50",
                PName = "yt",
                PPass = "6532456",
                PNation = "bd",
                Amt = "876"

            };
            
            FirebaseResponse response = client.Set("TicketTbl/" + "1", Tkt);
            */

            TicketDGV.DataSource = null;
            TicketDGV.Rows.Clear();

            
            FirebaseResponse re = client.Get("TicketTbl/");
            Dictionary<string, Ticket_class> getTicket = re.ResultAs<Dictionary<string, Ticket_class>>();

                foreach (var get in getTicket)
                {
                    TicketDGV.Rows.Add(

                        get.Value.TId,
                        get.Value.Fcode,
                        get.Value.PId,
                        get.Value.PName,
                        get.Value.PPass,
                        get.Value.PNation,
                        get.Value.Amt

                        );
                }
         
        }
        
        private void fillPassenger()
        {
            /*
            Con.Open();
            SqlCommand cmd = new SqlCommand("select PassId from PassengerTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("PassId", typeof(int));
            dt.Load(rdr);
            PIdCb.ValueMember = "PassId";
            PIdCb.DataSource = dt;
            Con.Close();
            */

            FirebaseResponse response = client.Get("PassengerTbl/");
           // AddPassenger_class fl = response.ResultAs<AddPassenger_class>();
            //DataTable dt = new DataTable();
            //dt.Columns.Add("PassId", typeof(string));

            Dictionary<string, AddPassenger_class> getPassenger = response.ResultAs<Dictionary<string, AddPassenger_class>>();

            foreach (var get in getPassenger)
            {
                string s=get.Value.PassId;

                PIdCb.Items.Add(s);
            }
            PIdCb.ValueMember = "PassId";
            //PIdCb.DataSource = dt;
            

        }

        private void fillFlightCode()
        {
            /*
            Con.Open();
            SqlCommand cmd = new SqlCommand("select Fcode from FlightTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Fcode", typeof(string));
            dt.Load(rdr);
            FCodeCb.ValueMember = "Fcode";
            FCodeCb.DataSource = dt;
            Con.Close();
            
            */
            
            FirebaseResponse response = client.Get("ScheduleTbl/");
            //AddFlight_class fl = response.ResultAs<AddFlight_class>();
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Fcode", typeof(string));

            Dictionary<string, AddFlight_class> getFlight = response.ResultAs<Dictionary<string, AddFlight_class>>();

            foreach (var get in getFlight)
            {
                string s=get.Value.Fcode;
                FCodeCb.Items.Add(s);
            }
            FCodeCb.ValueMember = "Fcode";
            //CodeCb.DataSource = dt;


        }

        string pname, ppass, pnat;
        
        
        private void fetchpassenger()
        {
            /*
            Con.Open();
            string query = "select * from PassengerTbl where PassId=" + PIdCb.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                pname = dr["PassName"].ToString();
                ppass = dr["Passport"].ToString();
                pnat = dr["PassNat"].ToString();
                PNameTb.Text = pname;
                PPassTb.Text = ppass;
                PNatTb.Text = pnat;

            }
            Con.Close();

            */

            FirebaseResponse response = client.Get("PassengerTbl/");
            // AddPassenger_class fl = response.ResultAs<AddPassenger_class>();
            //DataTable dt = new DataTable();
            //dt.Columns.Add("PassId", typeof(string));

            Dictionary<string, AddPassenger_class> getPassenger = response.ResultAs<Dictionary<string, AddPassenger_class>>();

            foreach (var get in getPassenger)
            {
                pname = get.Value.PassName;
                ppass = get.Value.Passport;
                pnat = get.Value.PassNat;
                PNameTb.Text = pname;
                PPassTb.Text = ppass;
                PNatTb.Text = pnat;

            }

        }
    

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            PNameTb.Text = "";
            PPassTb.Text = "";
            PNatTb.Text = "";
            PAmtTb.Text = "";
            Tid.Text = "";
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            populate();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        
        private void PIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            fetchpassenger();
        }
        
    }

}
