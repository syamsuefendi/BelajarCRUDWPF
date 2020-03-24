using BelajarCRUDWPF.MyContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        myContext connection = new myContext();

        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text;
            string email = TxtEmail.Text;
            var data = connection.Users.Where(S => S.Email == email).SingleOrDefault();
            if (data != null)
            {
                MessageBox.Show("You have been Register");
            }
            else
            {
                string password = Guid.NewGuid().ToString();
                var insert = new Model.User(name, email, password);
                connection.Users.Add(insert);
                connection.SaveChanges();
                MessageBox.Show("Register Successful");
                try
                {
                    Microsoft.Office.Interop.Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    mail.To = TxtEmail.Text;
                    mail.Subject = "Successful Register" + DateTime.Now.ToString("dd/MM/yyyy"); ;
                    mail.Body = "Hi " + TxtName.Text + ", this is your password : " + password;
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                    TxtName.Text = "";
                    TxtEmail.Text = "";
                    MessageBox.Show("Your Message has been successfully sent.", "Message", MessageBoxButton.OKCancel);
                    Login log = new Login();
                    log.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK);
                }

                Login log_ = new Login();
                log_.Show();
                this.Close();
            }
        }
    }
}
