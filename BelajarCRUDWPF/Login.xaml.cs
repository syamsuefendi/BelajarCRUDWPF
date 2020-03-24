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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Login : Window
    {
        myContext connection = new myContext();


        public Login()
        {
            InitializeComponent();



        }

        private void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var verication = connection.Users.FirstOrDefault(S => S.Email == TB_Email.Text);
                if (verication == null)
                {
                    MessageBox.Show("Your email is wrong!", "Warning", MessageBoxButton.OK);
                    TB_Email.Focus();
                    return;
                }

                else if (verication.Password != TB_Password.Password)

                {
                    MessageBox.Show("Your password is wrong!", "Warning", MessageBoxButton.OK);
                    TB_Password.Focus();
                    return;
                }
                //                   MainWindow window_main = new MainWindow();    //  Gunakan ini untuk database baru dan buat User terlebih dahulu
                MainWindow window_main = new MainWindow(verication.Email);  //  Gunakan parameter untuk bisa login  
                window_main.Show();
                this.Close();


            }
            catch (Exception)
            {

            }

        }

        private void Button_Forgot_Click(object sender, RoutedEventArgs e)
        {
            ForgetPassword window_forget = new ForgetPassword();
            window_forget.Show();
            this.Close();
        }


        private void TB_Email_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9@-_.]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TB_Email_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TB_Email_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {

        }



        private void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            Register reg = new Register();
            reg.Show();
            this.Close();
        }
    }
}
