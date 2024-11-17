using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp12
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Page
    {

        taskmanagementsystemEntities2 db =  new taskmanagementsystemEntities2();
        public login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(emailtextbox.Text))
            {
                MessageBox.Show("Please Enter Username");
            }
            else if (string.IsNullOrWhiteSpace(passwordtextbox.Text))
            {
                MessageBox.Show("Please Enter Password");
            }
            else
            {
                var adminUser = db.UserTables.Where(x => x.UserEmail == emailtextbox.Text && x.UserPassword == passwordtextbox.Text && x.Role == "Admin").FirstOrDefault();

                if (adminUser != null)
                {
                    MessageBox.Show("Admin !! \nWelcome back");
                    admin log = new admin();
                    this.NavigationService.Navigate(log);
                }
                else
                {
                    
                    var log = db.UserTables.Where(x => x.UserEmail == emailtextbox.Text && x.UserPassword == passwordtextbox.Text).Select(x => new {x.UserName,x.UserEmail}).FirstOrDefault();

                    if (log != null)
                    {
                        MessageBox.Show($"Welcome {log.UserName}");
                        employee logs = new employee();
                        this.NavigationService.Navigate(logs);
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password", "Error");
                    }

                }
            }
        }
    }
}
