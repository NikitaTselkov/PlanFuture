using Prism.Mvvm;

namespace PlanFuture.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Plan Future";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
