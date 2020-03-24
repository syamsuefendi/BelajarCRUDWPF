using BelajarCRUDWPF.MyContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace BelajarCRUDWPF
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class ForgetPassword : Window
    {
        myContext connection = new myContext();


        public ForgetPassword()
        {
            InitializeComponent();
        }

        private void Btn_SendEmail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TBEmail_Forget.Text == "")
                {
                    MessageBox.Show("Email is required", "Warning", MessageBoxButton.OK);
                    TBEmail_Forget.Focus();
                }
                else
                {
                    var check_email = connection.Users.FirstOrDefault(S => S.Email == TBEmail_Forget.Text);

                    if (check_email != null)
                    {
                        var this_email = check_email.Email;
                        if (TBEmail_Forget.Text == this_email)
                        {
                            string New_Password = Guid.NewGuid().ToString();
                            var check_user = connection.Users.FirstOrDefault(S => S.Email == TBEmail_Forget.Text);
                            check_user.Password = New_Password;
                            var chagepassword = connection.SaveChanges();

                            if (chagepassword >= 1)
                            {
                                MessageBox.Show("Password has been updated");

                            }

                            Outlook._Application openapp = new Outlook.Application();
                            Outlook.MailItem sentmail = (Outlook.MailItem)openapp.CreateItem(Outlook.OlItemType.olMailItem);
                            sentmail.To = TBEmail_Forget.Text;
                            sentmail.Subject = "Forgot Password " + DateTime.Now.ToString("dd/MM/yyyy");
                            sentmail.Body = "Dear " + TBEmail_Forget.Text + "\nThis Is Your Password : " + New_Password;
                            sentmail.Importance = Outlook.OlImportance.olImportanceNormal;
                            ((Outlook._MailItem)sentmail).Send();
                            MessageBox.Show("Check Your Email for Your New Password", "Message", MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("That Email Not Registered Yet!", "Warning", MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void Btn_BackLogin_Click(object sender, RoutedEventArgs e)
        {
            Login window_login = new Login();
            window_login.Show();
            this.Close();
        }

    }
}
