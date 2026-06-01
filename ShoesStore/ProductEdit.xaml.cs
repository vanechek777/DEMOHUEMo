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
    /// Логика взаимодействия для ProductEdit.xaml
    /// </summary>
    public partial class ProductEdit : Window
    {
        public ProductEdit(bool isEdit, int productId)
        {
            InitializeComponent();
            Init(isEdit, productId);
            
        }

        public void Init(bool isEdit, int productId)
        {
            if (isEdit)
            {
                return;
            }

            using (var context = new ShoesStoreEntities())
            {

            }
        }
    }
}
