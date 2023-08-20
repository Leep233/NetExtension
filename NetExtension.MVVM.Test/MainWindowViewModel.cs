using NetExtension.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetExtension.MVVM.Test
{
    public class MainWindowViewModel:BindableBase
    {
        private string name=string.Empty;

        public string Name
        {
            get { return name; }
            set { if (SetProperty(ref name, value, nameof(Name))) 
                {
                    SetNameCommand.RaiseCanExecuteChanged();
                } }
        }

        private string password = string.Empty;

        public string Password
        {
            get { return password; }
            set
            {
                if (SetProperty(ref password, value, nameof(Password)))
                {
                }
            }
        }

        public DelegateCommand<string> SetNameCommand { get; set; }

        public MainWindowViewModel()
        {
            SetNameCommand = new DelegateCommand<string>(CanExecuteSetNameCommand, ExecuteSetNameCommand);
     
        }

    

        private void ExecuteSetNameCommand(string password)
        {
            Name = "张三";
            

            Console.WriteLine(this.Password);
        }

        private bool CanExecuteSetNameCommand(string password)
        {
            return true;// string.IsNullOrEmpty(Name);
        }
    }
}
