using BelajarCRUDWPF.Model;
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
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace BelajarCRUDWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {


        myContext connection = new myContext();
        public int cb_sup;
        public int cb_role;

  /*public MainWindow()           // Untuk PC yang baru menggunakan databasenya akan 0 (karena CRUD),
     {                               //  sehingga gunakan MainWindow ini, jangan lupa ubah App.xaml menjadi MainWindow.xaml
         InitializeComponent();      // dan kosongkan parameter di button login di Login.xaml 
         TabSupplier.IsEnabled = false;
         TabItem.IsEnabled = false;
         TabUser.IsEnabled = false;
         TabRole.IsEnabled = false;
         TB_M_Supplier.ItemsSource = connection.Suppliers.ToList(); //refresh datagrid supplier
         TB_M_Item.ItemsSource = connection.Items.ToList(); //refresh datagrid item
         TB_M_User.ItemsSource = connection.Users.ToList(); //refresh datagrid user
         TB_M_Role.ItemsSource = connection.Roles.ToList(); //refresh datagrid role


         Combo_Supplier.ItemsSource = connection.Suppliers.ToList(); //refresh combo box supplier
         Combo_User_Role.ItemsSource = connection.Roles.ToList(); //refresh combo box role

         Btn_Delete.IsEnabled = false;
         Btn_Update.IsEnabled = false;


         Btn_Delete_Item.IsEnabled = false;
         Btn_Update_Item.IsEnabled = false;

         TextBox_Id.IsEnabled = false;
         TextBoxItemId.IsEnabled = false;

         TabSupplier.IsEnabled = true;
         TabItem.IsEnabled = true;
         TabUser.IsEnabled = true;
         TabRole.IsEnabled = true;

     }


  */     public MainWindow(string emaillogin)       // Parameter agar bisa Login menggunakan MainWindow ini,
              {       InitializeComponent();                   //  perlu diingat harus sudah membuat User dan Role baru

                     TabSupplier.IsEnabled = false;
                     TabItem.IsEnabled = false;
                     TabUser.IsEnabled = false;
                     TabRole.IsEnabled = false;
                     TB_M_Supplier.ItemsSource = connection.Suppliers.ToList(); //refresh datagrid supplier
                     TB_M_Item.ItemsSource = connection.Items.ToList(); //refresh datagrid item
                     TB_M_User.ItemsSource = connection.Users.ToList(); //refresh datagrid user
                     TB_M_Role.ItemsSource = connection.Roles.ToList(); //refresh datagrid role


                     Combo_Supplier.ItemsSource = connection.Suppliers.ToList(); //refresh combo box supplier
                     Combo_User_Role.ItemsSource = connection.Roles.ToList(); //refresh combo box role



                     TextBox_Id.IsEnabled = false;
                     TextBoxItemId.IsEnabled = false;

                     var check_email = emaillogin;
                     Disable_Access(check_email);

                 }




        private void Disable_Access(string check_email)
        {
            var checkrole = connection.Users.FirstOrDefault(S => S.Email == check_email);
            if (checkrole.Role.Name == "Administrator")
            {
                TabSupplier.IsEnabled = true;
                TabItem.IsEnabled = true;
                TabUser.IsEnabled = true;
                TabRole.IsEnabled = true;
            }
            else
            {
                TabItem.IsEnabled = true;
            }

        }


        private void Btn_Insert_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                if (TextBox_Name.Text == "" || TextBox_Address.Text == "" || TextBox_Email.Text == "")
                {
                    if (TextBox_Name.Text == "")
                    {
                        MessageBox.Show("Name is required", "Warning", MessageBoxButton.OK);
                        TextBox_Name.Focus();
                    }
                    else if (TextBox_Address.Text == "")
                    {
                        MessageBox.Show("Address is required", "Warning", MessageBoxButton.OK);
                        TextBox_Address.Focus();
                    }
                    else if (TextBox_Email.Text == "")
                    {
                        MessageBox.Show("Email is required", "Warning", MessageBoxButton.OK);
                        TextBox_Email.Focus();
                    }

                }


                else
                {
                    var check_email = connection.Suppliers.FirstOrDefault(S => S.Email == TextBox_Email.Text);
                    if (check_email == null)
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are You Sure?", "Insert Confirmation", System.Windows.MessageBoxButton.YesNo);

                        if (messageBoxResult == MessageBoxResult.Yes)
                        {

                            var input_supplier = new Supplier(TextBox_Name.Text, TextBox_Address.Text, TextBox_Email.Text);


                            connection.Suppliers.Add(input_supplier);
                            var insert = connection.SaveChanges();
                            if (insert >= 1)
                            { MessageBox.Show(insert + " Supplier has been inserted"); }

                            TB_M_Supplier.ItemsSource = connection.Suppliers.ToList();

                        }
                    }
                    else
                    {
                        MessageBox.Show("Email has been used");
                    }
                }
            }

            catch (Exception)
            {
            }

            reset_supplier();
            Combo_Supplier.ItemsSource = connection.Suppliers.ToList();

        }

        private void reset_supplier()
        {
            TextBox_Id.Text = string.Empty;
            TextBox_Name.Text = string.Empty;
            TextBox_Address.Text = string.Empty;
            TextBox_Email.Text = string.Empty;
        }


        private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            reset_supplier();

        }


        

        private void Btn_Refresh_Item_Click(object sender, RoutedEventArgs e)
        {
            reset_item();

        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (TextBox_Id.Text == "" || TextBox_Name.Text == "" || TextBox_Address.Text == "" || TextBox_Email.Text == "")

                {
                    MessageBox.Show("Data Not Found");
                }

                else
                {
                    int Id = Convert.ToInt32(TextBox_Id.Text);
                    var checkId = connection.Suppliers.Where(S => S.Id == Id).FirstOrDefault();

                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are You Sure?", "Update Confirmation", System.Windows.MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {


                        checkId.Name = TextBox_Name.Text;
                        checkId.Address = TextBox_Address.Text;
                        checkId.Email = TextBox_Email.Text;
                        var update = connection.SaveChanges();


                        MessageBox.Show("Supplier has been updated");
                        reset_supplier();
                        TB_M_Supplier.ItemsSource = connection.Suppliers.ToList();
                    }
                }
            }

            catch (Exception)
            {

            }


        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (TextBox_Id.Text == "")
                {
                    MessageBox.Show("Data Not Found");
                }

                else
                {
                    int Id = Convert.ToInt32(TextBox_Id.Text);
                    var checkId = connection.Suppliers.Where(S => S.Id == Id).FirstOrDefault();

                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are You Sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        connection.Suppliers.Remove(checkId);
                        var delete = connection.SaveChanges();


                        MessageBox.Show(delete + " Supplier has been deleted");
                        reset_supplier();
                        TB_M_Supplier.ItemsSource = connection.Suppliers.ToList();
                    }
                }
            }

            catch (Exception)
            {
            }


        }


        private void TB_M_Supplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectsupplier = TB_M_Supplier.SelectedItem;

            if (selectsupplier == null)
            {
                TB_M_Supplier.ItemsSource = connection.Suppliers.ToList();
            }

            else
            {
                string id = (TB_M_Supplier.SelectedCells[0].Column.GetCellContent(selectsupplier) as TextBlock).Text;
                TextBox_Id.Text = id;
                string name = (TB_M_Supplier.SelectedCells[1].Column.GetCellContent(selectsupplier) as TextBlock).Text;
                TextBox_Name.Text = name;
                string address = (TB_M_Supplier.SelectedCells[2].Column.GetCellContent(selectsupplier) as TextBlock).Text;
                TextBox_Address.Text = address;
                string email = (TB_M_Supplier.SelectedCells[3].Column.GetCellContent(selectsupplier) as TextBlock).Text;
                TextBox_Email.Text = email;


            }
        }

        private void Btn_Insert_Item_Click(object sender, RoutedEventArgs e)
        {


            try
            {

                var _Price = Convert.ToInt32(TextBoxItemPrice.Text);
                var _Stock = Convert.ToInt32(TextBoxItemStock.Text);
                var _Supp = connection.Suppliers.Where(S => S.Id == cb_sup).FirstOrDefault();


                if ((TextBoxItemName.Text == "") || (TextBoxItemPrice.Text == "") || (TextBoxItemStock.Text == "") || (Combo_Supplier.Text == ""))
                {
                    if (TextBoxItemName.Text == "")
                    {
                        MessageBox.Show("Name is required", "Warning", MessageBoxButton.OK);
                        TextBoxItemName.Focus();
                    }
                    else if (TextBoxItemPrice.Text == "")
                    {
                        MessageBox.Show("Price is required", "Warning", MessageBoxButton.OK);
                        TextBoxItemPrice.Focus();
                    }
                    else if (TextBoxItemStock.Text == "")
                    {
                        MessageBox.Show("Stock is required", "Warning", MessageBoxButton.OK);
                        TextBoxItemStock.Focus();
                    }
                    else if (Combo_Supplier.Text == "")
                    {
                        MessageBox.Show("Supplier is required", "Warning", MessageBoxButton.OK);
                        Combo_Supplier.Focus();
                    }

                }

                else

                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are You Sure?", "Insert Confirmation", System.Windows.MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)


                    {
                        var input_item = new Item(TextBoxItemName.Text, _Price, _Stock, _Supp);
                        connection.Items.Add(input_item);
                        var insert = connection.SaveChanges();

                        MessageBox.Show(insert + " Item has been inserted");

                        reset_item();
                        TB_M_Item.ItemsSource = connection.Items.ToList();
                    }

                }


            }

            catch (Exception)
            {
            }
        }


        private void reset_item()
        {
            TextBoxItemId.Text = string.Empty;
            TextBoxItemName.Text = string.Empty;
            TextBoxItemPrice.Text = string.Empty;
            TextBoxItemStock.Text = string.Empty;
            Combo_Supplier.Text = string.Empty;
        }


        private void Combo_Supplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_sup = Convert.ToInt32(Combo_Supplier.SelectedValue);
        }

        private void TB_M_Item_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectitem = TB_M_Item.SelectedItem;

            if (selectitem == null)
            {
                TB_M_Item.ItemsSource = connection.Items.ToList();
            }
            else
            {
                string Id = (TB_M_Item.SelectedCells[0].Column.GetCellContent(selectitem) as TextBlock).Text;
                TextBoxItemId.Text = Id;
                string Name = (TB_M_Item.SelectedCells[1].Column.GetCellContent(selectitem) as TextBlock).Text;
                TextBoxItemName.Text = Name;
                string Price = (TB_M_Item.SelectedCells[2].Column.GetCellContent(selectitem) as TextBlock).Text;
                TextBoxItemPrice.Text = Price;
                string Stock = (TB_M_Item.SelectedCells[3].Column.GetCellContent(selectitem) as TextBlock).Text;
                TextBoxItemStock.Text = Stock;
                string Supplier = (TB_M_Item.SelectedCells[4].Column.GetCellContent(selectitem) as TextBlock).Text;
                Combo_Supplier.Text = Supplier;

                //Btn_Delete_Item.IsEnabled = true;
                Btn_Update_Item.IsEnabled = true;
            }

        }



        private void Btn_Update_Item_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (TextBoxItemId.Text == "" || TextBoxItemName.Text == "" || TextBoxItemPrice.Text == "" || TextBoxItemStock.Text == "" || Combo_Supplier.Text == "")
                {
                    MessageBox.Show("Data Not Found");
                }
                else
                {

                    int Id = Convert.ToInt32(TextBoxItemId.Text);
                    var checkId = connection.Items.Where(S => S.Id == Id).FirstOrDefault();

                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are You Sure?", "Update Confirmation", System.Windows.MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        var _Price = Convert.ToInt32(TextBoxItemPrice.Text);
                        var _Stock = Convert.ToInt32(TextBoxItemStock.Text);
                        var _Supp = connection.Suppliers.Where(S => S.Id == cb_sup).FirstOrDefault();

                        checkId.Name = TextBoxItemName.Text;
                        checkId.Price = _Price;
                        checkId.Stock = _Stock;
                        checkId.Supplier = _Supp;

                        var update = connection.SaveChanges();

                        MessageBox.Show("Item has been updated");
                        reset_item();
                        TB_M_Item.ItemsSource = connection.Items.ToList();
                    }
                }
            }

            catch (Exception)
            {
            }
        }

        private void Btn_Delete_Item_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (TextBoxItemId.Text == "")
                {
                    MessageBox.Show("Data Not Found");
                }
                else
                {

                    int Id = Convert.ToInt32(TextBoxItemId.Text);
                    var checkId = connection.Items.Where(S => S.Id == Id).FirstOrDefault();


                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are You Sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        connection.Items.Remove(checkId);

                        var delete = connection.SaveChanges();

                        MessageBox.Show(delete + " Item has been deleted");
                        reset_item();
                        TB_M_Item.ItemsSource = connection.Items.ToList();
                    }
                }
            }

            catch (Exception)
            {
            }

        }


        private void TB_M_Supplier_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }



        private void TextBox_Id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_Name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z]+$");
            e.Handled = regex.IsMatch(e.Text);

        }

        private void TextBox_Address_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void TextBox_Email_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9@-_.]+$");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void TextBoxItemId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void TextBoxItemName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBoxItemPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBoxItemStock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void TextBoxUserName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z]+$");
            e.Handled = regex.IsMatch(e.Text);

        }

        private void TextBoxUserEmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9@-_.]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Combo_User_Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_role = Convert.ToInt32(Combo_User_Role.SelectedValue.ToString());
        }

        private void Btn_Add_User_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((TextBoxUserName.Text == "") || (TextBoxUserEmail.Text == "") || (Combo_User_Role.Text == ""))
                {
                    if (TextBoxUserName.Text == "")
                    {
                        MessageBox.Show("Name is required", "Warning", MessageBoxButton.OK);
                        TextBoxUserName.Focus();
                    }
                    else if (TextBoxUserEmail.Text == "")
                    {
                        MessageBox.Show("Email is required", "Warning", MessageBoxButton.OK);
                        TextBoxUserEmail.Focus();
                    }
                    else if (Combo_User_Role.Text == "")
                    {
                        MessageBox.Show("Role is required", "Warning", MessageBoxButton.OK);
                        Combo_User_Role.Focus();
                    }
                }
                else
                {

                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are You Sure?", "Insert Confirmation", System.Windows.MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)


                    {

                        string Password = Guid.NewGuid().ToString();
                        var checkuseremail = connection.Users.FirstOrDefault(S => S.Email == TextBoxUserEmail.Text);
                        var userrole = connection.Roles.FirstOrDefault(Y => Y.Id == cb_role);
                        if (checkuseremail == null)
                        {
                            var input_user = new User(TextBoxUserName.Text, TextBoxUserEmail.Text, Password, userrole);
                            connection.Users.Add(input_user);
                            var insert = connection.SaveChanges();

                            if (insert >= 1)
                            {
                                MessageBox.Show("User has been inserted");

                            }
                            TB_M_User.ItemsSource = connection.Users.ToList();

                            Outlook._Application openapp = new Outlook.Application();
                            Outlook.MailItem sentmail = (Outlook.MailItem)openapp.CreateItem(Outlook.OlItemType.olMailItem);
                            sentmail.To = TextBoxUserEmail.Text;
                            sentmail.Subject = "Your Password " + DateTime.Now.ToString("dd/MM/yyyy");
                            sentmail.Body = "Dear " + TextBoxUserName.Text + "\nThis Is Your Password : " + Password;
                            sentmail.Importance = Outlook.OlImportance.olImportanceNormal;
                            ((Outlook._MailItem)sentmail).Send();

                            MessageBox.Show("Your email has been sent!", "Message", MessageBoxButton.OK);
                            reset_adduser();


                        }
                        else
                        {
                            MessageBox.Show("Email has been used");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }


        }

        private void reset_adduser()
        {
            TextBoxUserName.Text = string.Empty;
            TextBoxUserEmail.Text = string.Empty;
            Combo_User_Role.Text = string.Empty;
        }

        private void TB_M_User_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBoxRoleName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }


        private void Btn_Add_Role_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBoxRoleName.Text == "")
                {
                    MessageBox.Show("Role Name is required", "Warning", MessageBoxButton.OK);
                    TextBoxRoleName.Focus();
                }
                else
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are You Sure?", "Insert Confirmation", System.Windows.MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)


                    {



                        var checkrole = connection.Roles.FirstOrDefault(S => S.Name == TextBoxRoleName.Text);
                        if (checkrole == null)
                        {
                            var input_role = new Role(TextBoxRoleName.Text);
                            connection.Roles.Add(input_role);
                            var insert = connection.SaveChanges();
                            if (insert >= 1)
                            {
                                MessageBox.Show("Role has been inserted");
                            }
                            reset_addrole();
                            TB_M_Role.ItemsSource = connection.Roles.ToList();
                        }
                        else
                        {
                            MessageBox.Show("Role has been used");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            Combo_User_Role.ItemsSource = connection.Roles.ToList();

        }





        private void reset_addrole()
        {
            TextBoxRoleName.Text = string.Empty;
        }

        private void TB_M_Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_Search_Supplier_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Button_Search_Supplier_Click(object sender, RoutedEventArgs e)
        {
            List<Supplier> FilterModeListSupplier = new List<Supplier>();

            int _parsedValue; // result

            if (string.IsNullOrWhiteSpace(TextBox_Search_Supplier.Text))
            {
                TB_M_Supplier.ItemsSource = connection.Suppliers.ToList();
            }
            else
            {
                foreach (Supplier rowsupplier in connection.Suppliers.ToList())
                {

                    if (rowsupplier.Name.ToLower().Contains(TextBox_Search_Supplier.Text.ToLower()))
                    {
                        FilterModeListSupplier.Add(rowsupplier);
                    }

                    else if (int.TryParse(TextBox_Search_Supplier.Text, out _parsedValue))

                    {
                        if (rowsupplier.Id.Equals(Convert.ToInt32(TextBox_Search_Supplier.Text)))

                        { FilterModeListSupplier.Add(rowsupplier); }
                    }


                    else if (rowsupplier.Address.ToLower().Contains(TextBox_Search_Supplier.Text.ToLower()))
                    {
                        FilterModeListSupplier.Add(rowsupplier);
                    }


                    else if (rowsupplier.Email.ToLower().Contains(TextBox_Search_Supplier.Text.ToLower()))
                    {
                        FilterModeListSupplier.Add(rowsupplier);
                    }


                }

                TB_M_Supplier.ItemsSource = FilterModeListSupplier.ToList();

            }
        }

        private void Button_Search_Item_Click(object sender, RoutedEventArgs e)
        {

            List<Item> FilterModeList = new List<Item>();

            int parsedValue; // result

            if (string.IsNullOrWhiteSpace(TextBox_Search_Item.Text))
            {
                TB_M_Item.ItemsSource = connection.Items.ToList();
            }
            else
            {
                foreach (Item row in connection.Items.ToList())
                {
                    if (row.Name.ToLower().Contains(TextBox_Search_Item.Text.ToLower()))
                    {
                        FilterModeList.Add(row);
                    }

                    else if (int.TryParse(TextBox_Search_Item.Text, out parsedValue))

                    {
                        if (row.Id.Equals(Convert.ToInt32(TextBox_Search_Item.Text)))
                        {
                            FilterModeList.Add(row);
                        }
                        if (row.Price.Equals(Convert.ToInt32(TextBox_Search_Item.Text)))
                        {
                            FilterModeList.Add(row);
                        }
                        if (row.Stock.Equals(Convert.ToInt32(TextBox_Search_Item.Text)))
                        {
                            FilterModeList.Add(row);
                        }

                    }

                }

                TB_M_Item.ItemsSource = FilterModeList.ToList();

            }
        }

        private void TextBox_Search_Item_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }


    }
}
