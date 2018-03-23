using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Mvvm;

namespace Stratus.ViewModels
{
    class WindowViewModel : ViewModelBase
    {

        public ICommand FullScreenCommand { get; }
        public ICommand PipCommand { get; }

       
    }
}
