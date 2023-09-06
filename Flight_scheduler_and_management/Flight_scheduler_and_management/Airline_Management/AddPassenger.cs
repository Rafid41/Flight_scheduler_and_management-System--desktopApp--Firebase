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
    public partial class AddPassenger : Form
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Firebase_auth",
            BasePath = "databas url"
        };
        IFirebaseClient client;

        public AddPassenger()
        {
            InitializeComponent();
        }

        //SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\AirlineDb.mdf;Integrated Security=True;Connect Timeout=30");

        




        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AddPassenger_Load(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            if(PassId.Text == "" || PassAd.Text== "" || PassName.Text=="" || PassportTb.Text=="" || PhoneTb.Text==""  || healthTb.Text=="")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {

                
                try
                {
                    

                    AddPassenger_class adp = new AddPassenger_class()
                    {
                        PassId = PassId.Text,
                        PassName = PassName.Text,
                        Passport = PassportTb.Text,
                        PassAd = PassAd.Text,
                        PassNat = NationalityCb.SelectedItem.ToString(),
                        PassGend = GenderCb.SelectedItem.ToString(),
                        PassPhone = PhoneTb.Text,
                        PassHealth = healthTb.Text
                        
                    };

                    
                    FirebaseResponse response = client.Set("PassengerTbl/" + PassId.Text, adp);
                    MessageBox.Show("Save Successful");



                }
                catch(Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                




            }


        }

        private void GenderCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ViewPassenger viewpass = new ViewPassenger();
            viewpass.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void PhoneTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            PassId.Clear();
            PassName.Clear();
            PassportTb.Clear();
            PassAd.Clear();
            NationalityCb.ResetText();
            GenderCb.ResetText();
            PhoneTb.Clear();
            healthTb.Clear();

        }

        private void label10_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
