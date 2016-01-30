using NthsKeys.DataModel;
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
using System.Windows.Shapes;
using NthsKeys.Uncompresser;

namespace PasswordEntrance
{
    /// <summary>
    /// Interaction logic for ArchiveDetailWindow.xaml
    /// </summary>
    public partial class ArchiveDetailWindow : Window
    {
        public ArchiveDetailWindow(archive ar)
        {
            archive = ar;
            InitializeComponent();
        }

        private archive archive;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(passwordBox.Text) && archive.Password != passwordBox.Text)
            {
                savePassword(passwordBox.Text);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayPanel.DataContext = archive;
            passwordBox.Text = archive.Password ?? string.Empty;
        }

        private void Extract_Click(object sender, RoutedEventArgs e)
        {
            Uncompresser uc = new Uncompresser(passwordBox.Text, archive.Path, Properties.Settings.Default.pathPrefix);
            try
            {
                uc.Uncompress();
                savePassword(passwordBox.Text);
                MessageBox.Show("解压完成。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("解压出错。" + ex.Message);
            }
        }

        private void savePassword(string pw)
        {
            archive.Password = passwordBox.Text;
            using (Model m = new Model())
            {
                m.archives.FirstOrDefault(i => i.Id == archive.Id).Password = archive.Password;
                m.SaveChanges();
            }
        }
    }
}
