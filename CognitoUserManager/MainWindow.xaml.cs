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

namespace CognitoUserManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public void SetUserPassword(string userId, string password)
        {
            string passwordSetCommand = @"aws cognito-idp admin-set-user-password --user-pool-id us-east-1_yWGy5Hak0 --username {0} --password ""{1}"" --permanent";
            RunCommand(string.Format(passwordSetCommand, userId, password));
        }

        public void SetAttribute(string userId, string attributeName, string attributeValue)
        {
            string attributeSetCommand = @"aws cognito-idp admin-update-user-attributes --user-pool-id us-east-1_yWGy5Hak0 --username {0} --user-attributes Name={1},Value=""{2}""";
            RunCommand(string.Format(attributeSetCommand, userId, attributeName, attributeValue));
        }

        public void RunCommand(string command)
        {
            try
            {

                var process = new System.Diagnostics.Process();
                process.StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Verb = "runas",
                    WorkingDirectory = @"C:\",
                    RedirectStandardInput = true
                };
                process.Start();

                process.StandardInput.WriteLine(command);
                process.StandardInput.Flush();
                process.StandardInput.Close();

                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception("Process exited with exit code " + process.ExitCode);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error running command: " + ex.Message);

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(password.Text))
                {
                    SetUserPassword(userid.Text, password.Text);
                    password.Text = "";
                }
                if (!string.IsNullOrEmpty(firstName.Text))
                {
                    SetAttribute(userid.Text, "given_name", firstName.Text);
                    firstName.Text = "";
                }
                if (!string.IsNullOrEmpty(lastName.Text))
                {
                    SetAttribute(userid.Text, "family_name", lastName.Text);
                    lastName.Text = "";
                }
                if (!string.IsNullOrEmpty(agency.Text))
                {
                    SetAttribute(userid.Text, "custom:Agency", agency.Text);
                    agency.Text = "";
                }

                userid.Text = "";
                MessageBox.Show("Success!");
            }
            catch(Exception ex)
            {
                MessageBox.Show("There was an error: " + ex.Message);
            }

        }
    }
}
