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

namespace ShoesStore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EnterAsGuest_Click(object sender, RoutedEventArgs e)
        {
            Catalog cat = new Catalog(1, -1); // запускаем каталог для гостя, вместо роли передаем -1
            cat.Show();
            this.Close();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            string log = login.Text;
            string pass = password.Password;

            using (var context = new ShoesStoreEntities())
            {
                var user = context.User.FirstOrDefault(x => x.Login == log);

                if (user == null)
                {
                    MessageBox.Show("Такой пользователь не существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (user.Password != pass)
                {
                    MessageBox.Show("Неправильный пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int userId = Convert.ToInt32(user.Id);
                Catalog cat = new Catalog(0, userId); // входит авторизованный пользователь, isGuest = 0, передаем ID пользователя
                cat.Show();
                this.Close();
            }
        }
    }
}
