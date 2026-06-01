using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Xml.Linq;

namespace ShoesStore
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        public Catalog(int isGuest, int userId)
        {
            InitializeComponent();

            if (isGuest == 1)
            {
                Filter.Visibility = Visibility.Collapsed;
                adminAct.Visibility = Visibility.Collapsed;
                userFullName.Visibility = Visibility.Collapsed;
            }
            if (userId != -1)
            {
                using (var context = new ShoesStoreEntities())
                {
                    var user = context.User.FirstOrDefault(x => x.Id == userId);
                    
                    if (Convert.ToInt32(user.RoleId) == 1)
                    {
                        Filter.Visibility = Visibility.Collapsed;
                        adminAct.Visibility = Visibility.Collapsed;
                    }

                    if (Convert.ToInt32(user.RoleId) == 2)
                    {
                        newProdBtn.Visibility = Visibility.Collapsed;
                    }

                    userFullName.Text = user.LastName + " " + user.FirstName + " " + user.MiddleName;
                }
            }
            
            Init();
        }

        public void Init()
        {
            using (var context = new ShoesStoreEntities())
            {
                filterManufacturer.Items.Clear();
                sortSupply.Items.Clear();
                searchTb.Text = string.Empty;

                filterManufacturer.Items.Add("Без фильтра");
                foreach (var item in context.Manufacturer)
                {
                    filterManufacturer.Items.Add(item.Name);
                }
                filterManufacturer.SelectedIndex = 0;

                sortSupply.Items.Add("Без сортировки");
                sortSupply.Items.Add("Возрастанию");
                sortSupply.Items.Add("Убыванию");
                sortSupply.SelectedIndex = 0;

                productCards.Children.Clear();

                var items = context.Product;

                foreach (var item in items)
                {
                    try
                    {
                        string cat = context.Category.FirstOrDefault(x => x.Id == item.CategoryId).Name.ToString();
                        string name = context.ProductType.FirstOrDefault(x => x.Id == item.TypeId).TypeName.ToString();

                        string desc = item.Description;
                        string manufacturer = context.Manufacturer.FirstOrDefault(x => x.Id == item.ManufacturerId).Name.ToString();
                        string sup = context.Supplier.FirstOrDefault(x => x.Id == item.SupplierId).Name.ToString();
                        float price = Convert.ToSingle(item.Price);
                        string unit = item.Unit;
                        int count = Convert.ToInt32(item.StorageCount);
                        int disc = Convert.ToInt32(item.Discount);

                        string uriPath = "pack://application:,,,/Resources/picture.png";
                        if (item.PhotoPath != null)
                        {
                            uriPath = "pack://application:,,,/Resources/" + item.PhotoPath;
                        }

                        Debug.WriteLine(uriPath);
                        ProductCard card = new ProductCard();
                        if (disc >= 15)
                        {
                            var convert = new BrushConverter();
                            card.cardBack.Background = (Brush)convert.ConvertFromString("#2E8B57");
                        }
                        if (count == 0)
                        {
                            var convert = new BrushConverter();
                            card.cardBack.Background = (Brush)convert.ConvertFromString("#ADD8E6");
                        }
                        BitmapImage img = new BitmapImage(new Uri(uriPath));
                        card.prodImage.Source = img;
                        card.prodCatName.Text = cat + " | " + name;
                        card.prodDesc.Text += " " + desc;
                        card.prodManuf.Text += manufacturer;
                        card.prodSupp.Text += sup;
                        card.prodPrice.Text += price.ToString();
                        card.prodOldPrice.Text = " " + (price + price / 100 * disc).ToString();
                        card.prodUnit.Text += unit;
                        card.prodCount.Text += count.ToString();
                        card.prodDiscount.Text = disc.ToString() + "%";

                        card.DataContext = item.Id;

                        

                        productCards.Children.Add(card);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Произошла ошибка загрузки данных: {e}");
                    }
                    
                }

            }
        }

        private void leave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow auth = new MainWindow();
            auth.Show();
            this.Close();
        }

        private void prod_KeyDown(object sender, KeyEventArgs e)
        {
            var item = e.Source as FrameworkElement;
        }

        private void orderBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isOpen = Application.Current.Windows.OfType<Order>().Any();

            if ( isOpen )
            {
                return;
            }

            Order order = new Order();
            order.Show();
            
        }

        private void newProdBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isOpen = Application.Current.Windows.OfType<ProductEdit>().Any();
            
            
            if (isOpen)
            {
                return;
            }

            /*ProductEdit editor = new ProductEdit();
            editor.Show();*/
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Работает");
            Init();
        }

        private void prodCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            Debug.WriteLine(element.DataContext.ToString());
        }
    }
}
