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
    public partial class SignUp : Form
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Firebase auth",
            BasePath = "database url"
        };
        IFirebaseClient client;

        public SignUp()
        {
            InitializeComponent();
        }

        private void SignUp_Load(object sender, EventArgs e)
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

        private void back_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
            this.Hide();
        }

        private void signUpbtn_Click(object sender, EventArgs e)
        {

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(mailTb.Text);


            if (nameTb.Text == "" || mailTb.Text == "" || UnameTb.Text == "" || PassTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else if (!match.Success)
            {
                MessageBox.Show("Invalid E-mail");
            }
            else
            {


                try
                {

                    signUp_class sp = new signUp_class()
                    {
                        Username = UnameTb.Text,
                        Password = PassTb.Text,
                        Name = nameTb.Text,
                        Mail = mailTb.Text
                       

                    };
                    int b = 1;
                  

                    FirebaseResponse response = client.Get("SignUpTbl/");
                    Dictionary<string, signUp_class> getData = response.ResultAs<Dictionary<string, signUp_class>>();

                    foreach (var get in getData)
                    {
                        if (UnameTb.Text == get.Value.Username)
                        {
                            b = 0;
                            MessageBox.Show("This Username Already  Exists !, Try another one");
                            break;
                        }
                    }
                    if (b == 1)
                    {
                        //upload to database
                        FirebaseResponse re = client.Set("SignUpTbl/" + UnameTb.Text, sp);
                        MessageBox.Show("Data Recorded");
                    }
                       
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }


        //Random password gen
        private string OTPGenerator()
        {
            Random random = new Random();
            int OTP = random.Next(1000, 9999);
            return OTP.ToString();
        }

        //mail sending
        private void mail_sending(string to,string random_OTP)
        {
            string  from, pass;

            MailMessage message = new MailMessage();
           
            from = "email";
            pass = "key";

            message.From = new MailAddress(from);
            message.To.Add(to);

            message.Subject = "SKYWAYS: Your OTP";

            string body = "Welcome to SKYWAYS.<br>Your OTP is:  "+random_OTP + "<br>Do not share it with anyone.";
            message.Body =body;

            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);

            try
            {
                smtp.Send(message);
                //string b = "Message sent.\nCheck your spam Folder if u can't find it in inbox";
                //DialogResult code = MessageBox.Show("Email Send Successfully","Email sent",MessageBoxButtons.OK,MessageBoxIcon.Information);
                //MessageBox.Show(b);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UnameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void mailTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
        string random_OTP;

        private void ChkOTPbtn_Click(object sender, EventArgs e)
        {
            if (otpTb.Text == "")
            {
                MessageBox.Show("Insert the OTP to Proceed !");
            }
            else if (random_OTP == otpTb.Text)
            {
                MessageBox.Show("E-mail verified !");
                signUpbtn.Enabled = true;
                mailTb.Enabled = false;
                ChkOTPbtn.Enabled = false;
            }
            else
            {
                MessageBox.Show("Wrong OTP ! \nInsert the right OTP to proceed");
            }
        }

        private void sendOTPbtn_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(mailTb.Text);


            if (match.Success)
            {
                random_OTP = OTPGenerator();

                mail_sending(mailTb.Text, random_OTP);
                MessageBox.Show("An OTP was sent to Your given mail address.\nCheck Your Spam folder if you can't find it in your inbox.\nFill the OTP box if you got it.");
                sendOTPbtn.Enabled = false;
            }
            else
            {
                MessageBox.Show("Invalid Email");
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}