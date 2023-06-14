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


        public DelegateCommand SetNameCommand { get; set; }

        public MainWindowViewModel()
        {
            SetNameCommand = new DelegateCommand(CanExecuteSetNameCommand, ExecuteSetNameCommand);
     
        }

    

        private void ExecuteSetNameCommand()
        {
            Name = "张三";
        }

        private bool CanExecuteSetNameCommand()
        {
            return string.IsNullOrEmpty(Name);
        }
    }
}
