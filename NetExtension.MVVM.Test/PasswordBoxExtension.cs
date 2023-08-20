using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NetExtension.MVVM.Test
{
    public  class PasswordBoxExtension: DependencyObject
    {



        public static string GetPassword(DependencyObject element) 
        {
           
            return (string)element.GetValue(PasswordProperty);
        }
        public static void SetPassword(DependencyObject element,string pwd) {
         element.SetValue(PasswordProperty, pwd);

        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxExtension),new PropertyMetadata("", OnPasswordChangedCallback));

        private static void OnPasswordChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox box)
            {

                box.Password = e.NewValue.ToString();

            }
        }





    }
}
