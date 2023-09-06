using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class ticket_test : Form
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "firebase auth",
            BasePath = "database url"
        };
        IFirebaseClient client;


        public ticket_test()
        {
            InitializeComponent();
        }

        private void ticket_test_Load(object sender, EventArgs e)
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
                   
                    var Tkt = new Ticket_class
                    {
                        TId = Tid.Text,
                        Fcode = FcodeTb.Text,
                        PId = PIdTb.Text,
                        PName = PNameTb.Text,
                        PPass = PpassTb.Text,
                        PNation = PNatCb.Text,
                        Amt = PAmtTb.Text

                    };

                    FirebaseResponse response = client.Set("TicketTbl/" + Tid.Text, Tkt);
                    MessageBox.Show("Ticket Bookeded Successfully");



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

        private void PNatTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Tid.Text = "";
            FcodeTb.Text = "";
            PIdTb.Text = "";
            PNameTb.Text = "";
            PpassTb.Text = "";
            PNatCb.Text = "";
            PAmtTb.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Home hm = new Home();
            hm.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
