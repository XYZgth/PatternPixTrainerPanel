using System.Windows.Controls;

namespace PatternPixTrainerPanel.View
{
    public partial class MainView : UserControl
    {
        public int SelectedRepositoryMode => (int)StorageSlider.Value;
        
        public MainView()
        {
            InitializeComponent();
        }
        public int GetSelectedRepositoryMode()
        {
            return (int)StorageSlider.Value;
        }
    }
}