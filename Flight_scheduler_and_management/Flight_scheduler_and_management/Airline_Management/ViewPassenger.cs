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
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;


namespace Airline_Management
{
    public partial class ViewPassenger : Form
    {


        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Firebase auth",
            BasePath = "database url"
        };
        IFirebaseClient client;


        private void ViewPassenger_Load(object sender, EventArgs e)
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


        public ViewPassenger()
        {
            InitializeComponent();

            //initialization
            natcb.SelectedIndex = 0;


        }

        //SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\AirlineDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            



            PassengerDGV.DataSource = null;
            PassengerDGV.Rows.Clear();

            FirebaseResponse response = client.Get("PassengerTbl/");
            Dictionary<string, AddPassenger_class> getPassenger = response.ResultAs<Dictionary<string, AddPassenger_class>>();

            foreach (var get in getPassenger)
            {
                PassengerDGV.Rows.Add(

                    get.Value.PassId,
                    get.Value.PassName,
                    get.Value.Passport,
                    get.Value.PassAd,
                    get.Value.PassNat,
                    get.Value.PassGend,
                    get.Value.PassPhone,
                    get.Value.PassHealth

                    );
            }
            

        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        

        private void button4_Click(object sender, EventArgs e)
        {
            AddPassenger addpas = new AddPassenger();
            addpas.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(PidTb.Text=="")
            {
                MessageBox.Show("Enter The Passenger to Delete");
            }
            else
            {
                try
                {
                   

                    FirebaseResponse response = client.Delete("PassengerTbl/" + PidTb.Text);
                    MessageBox.Show("Passenger Deleted Successfully");

                    
                    populate();


                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void PassengerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            PidTb.Text = PassengerDGV.SelectedRows[0].Cells[0].Value.ToString();
            PnameTb.Text = PassengerDGV.SelectedRows[0].Cells[1].Value.ToString();
            PpassTb.Text = PassengerDGV.SelectedRows[0].Cells[2].Value.ToString();
            PaddTb.Text = PassengerDGV.SelectedRows[0].Cells[3].Value.ToString();
            natcb.SelectedItem = PassengerDGV.SelectedRows[0].Cells[4].Value.ToString();
            GendCb.SelectedItem = PassengerDGV.SelectedRows[0].Cells[5].Value.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            PidTb.Text = "";
            PnameTb.Text = "";
            PphoneTb.Text = "";
            PpassTb.Text = "";
            PaddTb.Text = "";
            healthTb.Text = "";
            //natcb.SelectedItem = "";
            GendCb.SelectedItem = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(PidTb.Text=="" || PnameTb.Text == "" || PpassTb.Text == "" ||PaddTb.Text == "" || healthTb.Text=="")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                   

                    var passn = new AddPassenger_class
                    {
                        PassId = PidTb.Text,
                        PassName = PnameTb.Text,
                        Passport = PpassTb.Text,
                        PassAd = PaddTb.Text,
                        PassNat = natcb.SelectedItem.ToString(),
                        PassGend = GendCb.SelectedItem.ToString(),
                        PassPhone = PphoneTb.Text,
                        PassHealth = healthTb.Text
                    };

                    FirebaseResponse response = client.Update("PassengerTbl/" + PidTb.Text, passn);
                    MessageBox.Show("Update Successful");

                   

                    populate();

                }
                catch(Exception Ex)
                {
                    MessageBox.Show("Missing Information");
                }
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void healthTb_TextChanged(object sender, EventArgs e)
        {

        }




        private void SearchBtn_Click(object sender, EventArgs e)
        {
            PassengerDGV.DataSource = null;
            PassengerDGV.Rows.Clear();
            int b = 0;

            FirebaseResponse response = client.Get("PassengerTbl/");
            Dictionary<string, AddPassenger_class> getData = response.ResultAs<Dictionary<string, AddPassenger_class>>();

            if_null();  //jodi null input hoy, seta avoid korbe 

            foreach (var get in getData)
            {
                if (PidTb.Text == get.Value.PassId || PpassTb.Text == get.Value.Passport || natcb.SelectedItem.ToString() == get.Value.PassNat || PphoneTb.Text == get.Value.PassPhone || PnameTb.Text == get.Value.PassName || PaddTb.Text == get.Value.PassAd || healthTb.Text == get.Value.PassHealth)
                {
                    b = 1;

                    PassengerDGV.Rows.Add(

                         get.Value.PassId,
                         get.Value.PassName,
                         get.Value.Passport,
                         get.Value.PassAd,
                         get.Value.PassNat,
                         get.Value.PassGend,
                         get.Value.PassPhone,
                         get.Value.PassHealth
                   );
                }
            }

            if (b == 0)
            {
                MessageBox.Show("No Data Found");
            }

            PidTb.Text = "";
            PnameTb.Text = "";
            PphoneTb.Text = "";
            PpassTb.Text = "";
            PaddTb.Text = "";
            healthTb.Text = "";
            //natcb.SelectedItem = "";
            GendCb.SelectedItem = "";
        }


        private void if_null()
        {
            if (PidTb.Text == "") PidTb.Text = " ";
            if (PpassTb.Text == "") PpassTb.Text = " ";
            if (PnameTb.Text == "") PnameTb.Text = " ";
            if (PaddTb.Text == "") PaddTb.Text = " ";
            if (PphoneTb.Text == "") PphoneTb.Text = " ";
            if (healthTb.Text == "") healthTb.Text = " ";
            //if (natcb.SelectedItem.ToString() == "") natcb.SelectedIndex = 0;
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            populate();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap objbmp = new Bitmap(this.PassengerDGV.Width, this.PassengerDGV.Height);
            PassengerDGV.DrawToBitmap(objbmp, new Rectangle(0, 0, this.PassengerDGV.Width, this.PassengerDGV.Height));

            e.Graphics.DrawImage(objbmp, 40, 300);
            e.Graphics.DrawString(label2.Text, new Font("Times New Roman", 26, FontStyle.Bold), Brushes.Red, new Point(260, 230));
        }

        private void Printbtn_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
    }
}
