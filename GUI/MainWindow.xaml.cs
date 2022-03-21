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
        Robot ?robot;
        string ?chosenArm;
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

        /*Robot*/
        private void moveRobot_Click(object sender, RoutedEventArgs e)
        {
            int x = 0, y = 0, z = 0;
            if (robot != null)
                if (int.TryParse(rxOffset.Text, out x) && int.TryParse(ryOffset.Text, out y) && int.TryParse(rzOffset.Text, out z))
                {
                    if (robot.Move(x, y, z))
                        MessageBox.Show($"Moving robot");
                    else
                    {
                        MessageBox.Show($"Robot busy, current action = {robot.currentAction}");
                    }
                }
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
                SelectedcbItem = cbItem;
                cbItems.Add(cbItem);
                string[] names = new string[num];
                for (int i = 0; i < num; i++)
                {
                    names[i] = $"Arm{i}";
                    cbItems.Add(new ComboBoxItem { Content = names[i] });
                }
                robot = new Robot(num, names);
            }
        }

        private void showRobotCoordsButton_Click(object sender, RoutedEventArgs e)
        {
            if(robot != null)
                MessageBox.Show(robot.toString());
        }
        private void showRobotStatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (robot != null)
                MessageBox.Show(robot.GetStatus());
        }
        /*Robot arms*/

        private void moveSelectedArm_Click(object sender, RoutedEventArgs e)
        {
            int x=0, y=0, z=0;
            if (robot != null && chosenArm != null)
                if (int.TryParse(xOffset.Text, out x) && int.TryParse(yOffset.Text, out y) && int.TryParse(zOffset.Text, out z))
                {
                    if(robot.MoveArm(chosenArm, x, y, z))
                        MessageBox.Show($"Moving {chosenArm}");
                    else
                    {
                        MessageBox.Show($"{chosenArm} busy, current action = {robot.GetArm(chosenArm).currentAction}");
                    }
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

        private void performShortAction_Click(object sender, RoutedEventArgs e)
        {
            if (robot != null && chosenArm != null)
                if (robot.ArmActionFactory(chosenArm,"short"))
                        MessageBox.Show($"Action started {chosenArm}");
                else
                {
                        MessageBox.Show($"{chosenArm} busy, current action = {robot.GetArm(chosenArm).currentAction}");
                }
        }

        private void performMediumAction_Click(object sender, RoutedEventArgs e)
        {
            if (robot != null && chosenArm != null)
                if (robot.ArmActionFactory(chosenArm, "medium"))
                    MessageBox.Show($"Action started {chosenArm}");
                else
                {
                    MessageBox.Show($"{chosenArm} busy, current action = {robot.GetArm(chosenArm).currentAction}");
                }
        }

        private void performLongAction_Click(object sender, RoutedEventArgs e)
        {
            if (robot != null && chosenArm != null)
                if (robot.ArmActionFactory(chosenArm, "long"))
                    MessageBox.Show($"Action started {chosenArm}");
                else
                {
                    MessageBox.Show($"{chosenArm} busy, current action = {robot.GetArm(chosenArm).currentAction}");
                }
        }

        /*Utilities*/
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
    }
}
