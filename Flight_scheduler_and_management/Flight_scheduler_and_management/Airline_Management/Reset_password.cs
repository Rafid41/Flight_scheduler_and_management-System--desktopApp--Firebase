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
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;

namespace Airline_Management
{
    public partial class Reset_password : Form
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Firebase auth",
            BasePath = "database url"
        };
        IFirebaseClient client;


        private void Reset_password_Load(object sender, EventArgs e)
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



        public Reset_password()
        {
            InitializeComponent();
        }

       

        private void label7_Click(object sender, EventArgs e)
        {
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        string email,name;
        private void Resetbtn_Click(object sender, EventArgs e)
        {
            Update_password();
        }


        private void Update_password()
        {
            if (PassTb.Text == "")
            {
                MessageBox.Show("Enter new Password");
            }
            else
            {
                try
                {
                    signUp_class sp = new signUp_class()
                    {
                        Username = UnameTb.Text,
                        Name = name,
                        Password = PassTb.Text,
                        Mail = email
                    };

                    FirebaseResponse response = client.Update("SignUpTbl/" + UnameTb.Text, sp);
                    MessageBox.Show("Update Password Successful");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } 
        }

        private string OTPGenerator()
        {
            Random random = new Random();
            int OTP = random.Next(1000, 9999);
            return OTP.ToString();
        }



        //mail sending
        private void mail_sending(string to, string random_OTP)
        {
            string from, pass;

            MailMessage message = new MailMessage();

            from = "email";
            pass = "key";

            message.From = new MailAddress(from);
            message.To.Add(to);

            message.Subject = "SKYWAYS: Your OTP";

            string body = "<br>Your OTP for Password Reset is:  " + random_OTP + "<br>Do not share it with anyone.";
            message.Body = body;

            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);

            try
            {
                smtp.Send(message);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }







        string random_OTP;

        private void ChkOTPbtn_Click(object sender, EventArgs e)
        {
            if (random_OTP == otpTb.Text)
            {
                MessageBox.Show("Email Verified !");
                ResetPassbtn.Enabled = true;
                sendOTPbtn.Enabled = false;
                ChkOTPbtn.Enabled = false;
            }
            else
            {
                MessageBox.Show("Wrong OTP !");
            }
        }

        bool check_user = false;

        private void sendOTPbtn_Click(object sender, EventArgs e)
        {
            Check_username();

            if (check_user == true)
            {
                ChkOTPbtn.Enabled = true;
                random_OTP = OTPGenerator();

                mail_sending(email, random_OTP); ;
                MessageBox.Show("An OTP was sent to Your given mail address.\nCheck Your Spam folder if you can't find it in your inbox.\nFill the OTP box if you got it.");
                sendOTPbtn.Enabled = false;
            }

            
        }

        private void back_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
            this.Hide();
        }

        private void otpTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void Check_username()
        {
            if (UnameTb.Text == "")
            {
                MessageBox.Show("Enter Username");
                
            }

            else
            {


                try
                {

                    signUp_class sp = new signUp_class()
                    {
                        Username = UnameTb.Text

                    };
                    int b = 1;


                    FirebaseResponse response = client.Get("SignUpTbl/");
                    Dictionary<string, signUp_class> getData = response.ResultAs<Dictionary<string, signUp_class>>();

                    foreach (var get in getData)
                    {
                        if (UnameTb.Text == get.Value.Username)
                        {
                            b = 0;
                            email = get.Value.Mail;
                            name = get.Value.Name;

                            check_user = true;
                         
                        }
                    }
                    if (b == 1)
                    {
                        
                        MessageBox.Show("No Username called " + UnameTb.Text + " found");
                        check_user = false;

                    }

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                    check_user = false;

                }

            }
        }
    }
}
