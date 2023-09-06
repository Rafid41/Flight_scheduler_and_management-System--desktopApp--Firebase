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
    public partial class Login : Form
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "firebase auth",
            BasePath = "database url"
        };
        IFirebaseClient client;

        private void Login_Load(object sender, EventArgs e)
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
        }

        public Login()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UnameTb.Text = "";
            PassTb.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (UnameTb.Text == "" || PassTb.Text == "")
            {
                MessageBox.Show("Enter the User Id and Password");
            }
           
            else
            {
                int b = 0;
                FirebaseResponse response = client.Get("SignUpTbl/");
                Dictionary<string, signUp_class> getData = response.ResultAs<Dictionary<string, signUp_class>>();

                foreach (var get in getData)
                {
                    if (UnameTb.Text == get.Value.Username && PassTb.Text==get.Value.Password)
                    {
                        b = 1;
                        Home home = new Home();
                        home.Show();
                        this.Hide();
                    }
                }
                if (b == 0)
                {
                    MessageBox.Show("Wrong User Id or Password");
                }
            }
        }

        private void SignUpbtn_Click(object sender, EventArgs e)
        {
            SignUp sn = new SignUp();
            sn.Show();
            this.Hide();
        }

        private void resetPassbtn_Click(object sender, EventArgs e)
        {
            Reset_password rp = new Reset_password();
            rp.Show();
            this.Hide();
        }
    }
}
