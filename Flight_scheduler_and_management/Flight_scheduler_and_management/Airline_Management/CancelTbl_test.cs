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
    public partial class CancelTbl_test : Form
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Firebase_auth",
            BasePath = "database url"
        };
        IFirebaseClient client;


        public CancelTbl_test()
        {
            InitializeComponent();
        }

        private void CancelTbl_test_Load(object sender, EventArgs e)
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

        private void populate()
        {

            TicketDGV.DataSource = null;
            TicketDGV.Rows.Clear();

            try
            {
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
            catch
            {

            }
        }
        
       

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void BackBt_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void CancelBt_Click(object sender, EventArgs e)
        {
            if (CtktId.Text == "")
            {
                MessageBox.Show("Enter The Ticket id and Cancel id to Delete");
            }
            else
            {
                try
                {
                  
                    FirebaseResponse response = client.Get("TicketTbl/" + CtktId.Text);

                    Ticket_class tkt = response.ResultAs<Ticket_class>();

                    
                    

                    FirebaseResponse re = client.Delete("TicketTbl/" + CtktId.Text);
                    MessageBox.Show("Ticket Cancelled Successfully");

                   

                    populate();
              


                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
