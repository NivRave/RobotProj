using ClassLibrary1.Robot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Robot robot;
        string chosenArm;
        public ObservableCollection<ComboBoxItem> cbItems { get; set; }
        public ComboBoxItem SelectedcbItem { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            cbItems = new ObservableCollection<ComboBoxItem>();
            var cbItem = new ComboBoxItem { Content = "<--Select Arm-->" };
            cbItems.Add(cbItem);
            SelectedcbItem = cbItem;
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            int num;
            if (!int.TryParse(numberOfArmsText.Text, out num))
            {
                //Throw exception, remove return and msg
                MessageBox.Show("Number of arms required!");
                return;
            }
            else
            {
                cbItems.Clear();
                var cbItem = new ComboBoxItem { Content = "<--Select Arm-->" };
                cbItems.Add(cbItem);
                SelectedcbItem = cbItem;
                string[] names = new string[num];
                for (int i = 0; i < num; i++)
                {
                    names[i] = $"Arm{i}";
                    cbItems.Add(new ComboBoxItem { Content = names[i] });
                }
                robot = new Robot(num, names);
            }
        }

        private void showButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(robot.toString());
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (armSelectionBox.SelectedValue != null && armSelectionBox.SelectedIndex != 0)
            {
                chosenArm = ((ComboBoxItem)armSelectionBox.SelectedItem).Content.ToString();
                MessageBox.Show($"Working on {chosenArm}");
            }
            else
            {
                chosenArm = null;
            }
        }

        private void moveSelectedArm_Click(object sender, RoutedEventArgs e)
        {
            int x=0, y=0, z=0;
            if (chosenArm != null)
                if (int.TryParse(xOffset.Text, out x) && int.TryParse(yOffset.Text, out y) && int.TryParse(zOffset.Text, out z))
                {
                    robot.MoveArm(chosenArm, x, y, z);
                    MessageBox.Show($"Working on {chosenArm}");
                }
        }

        private void numericTextbox(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^0-9]+");
            e.Handled = rgx.IsMatch(e.Text);
        }
        private void offsetTextbox(object sender, TextCompositionEventArgs e)
        {
            Regex rgx;
            string txt = e.Text;
            int len = txt.Length;
            if (txt[0] == '-')
            {
                if (len > 1)
                {
                    rgx = new Regex("-?[^0-9]+");
                    e.Handled = rgx.IsMatch(e.Text);
                }
                else
                {
                    e.Handled = false;
                }
            }
            else
            {
                rgx = new Regex("[^0-9]+");
                e.Handled = rgx.IsMatch(e.Text);
            }
        }
    }
}
