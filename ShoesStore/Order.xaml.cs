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

namespace ShoesStore
{
    /// <summary>
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        public Order()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            using (var context = new ShoesStoreEntities())
            {
                orderScroll.Children.Clear();
                foreach (var item in context.Orderr)
                {
                    OrderCard order = new OrderCard();
                    
                    order.orderId.Text += item.id.ToString();
                    order.orderStatus.Text += " " + item.Status.Name;
                    var adress = context.PickUpPoint.FirstOrDefault(x => x.Id == item.adressId);
                    order.pupAdress.Text += adress.City.CityName + " " + adress.Street;
                    order.createdAt.Text += " " + item.orderDate.ToString();
                    order.arriveDate.Text = " " + item.arriveDate.ToString();

                    orderScroll.Children.Add(order);
                }
            }
        }
    }
}
