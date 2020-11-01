using Misan_Diana_lab2;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Misan_Diana_Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DoughnutMachine myDoughnutMachine;
        private int mRaisedGlazed;
        private int mRaisedSugar;
        private int mFilledLemon;
        private int mFilledChocolate;
        private int mFilledVanilla;

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            myDoughnutMachine = new DoughnutMachine();
            myDoughnutMachine.DoughnutComplete += new DoughnutMachine.DoughnutCompleteDelegate(DoughnutCompleteHandler);
            cmbType.ItemsSource = PriceList;
            cmbType.DisplayMemberPath = "Key";
            cmbType.SelectedValuePath = "Value";
        }
        public MainWindow()
        {
            InitializeComponent();
            //creare obiect binding pentru comanda
            CommandBinding cmd1 = new CommandBinding();
            //asociere comanda
            cmd1.Command = ApplicationCommands.Print;
            //asociem un handler
            cmd1.Executed += new ExecutedRoutedEventHandler(CtrlP_CommandHandler);
            //adaugam la colectia CommandBindings
            this.CommandBindings.Add(cmd1);
            //input gesture: I + Alt
            ApplicationCommands.Print.InputGestures.Add(
            new KeyGesture(Key.I, ModifierKeys.Alt));


            //Doughnuts>Stop
            //comanda custom
            CommandBinding cmd2 = new CommandBinding();
            cmd2.Command = Misan_Diana_lab2.CustomCommands.StopCommand.Launch;
            cmd2.Executed += new ExecutedRoutedEventHandler(CtrlS_CommandHandler);//asociem handler
            this.CommandBindings.Add(cmd2);

        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void CtrlP_CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("You have in stock:" + mRaisedGlazed + " Glazed," + mRaisedSugar + " Sugar," + mFilledLemon + " Lemon," + mFilledChocolate + " Chocolate," + mFilledVanilla + " Vanilla");
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void glazedToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            glazedToolStripMenuItem.IsChecked = true;
            sugarToolStripMenuItem.IsChecked = false;
            myDoughnutMachine.MakeDoughnuts(DoughnutType.Glazed);
        }
        private void sugarToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            glazedToolStripMenuItem.IsChecked = false;
            sugarToolStripMenuItem.IsChecked = true;
            myDoughnutMachine.MakeDoughnuts(DoughnutType.Sugar);
        }
        private void DoughnutCompleteHandler()
        {
            switch (myDoughnutMachine.Flavor)
            {
                case DoughnutType.Glazed:
                    mRaisedGlazed++;
                    txtGlazedRaised.Text = mRaisedGlazed.ToString();
                    break;
                case DoughnutType.Sugar:
                    mRaisedSugar++;
                    txtSugarRaised.Text = mRaisedSugar.ToString();
                    break;
                case DoughnutType.Lemon:
                    mFilledLemon++;
                    txtLemonFilled.Text = mFilledLemon.ToString(); break;
                case DoughnutType.Chocolate:
                    mFilledChocolate++;
                    txtChocolateFilled.Text = mFilledChocolate.ToString(); break;
                case DoughnutType.Vanilla:
                    mFilledVanilla++;
                    txtVanillaFilled.Text = mFilledVanilla.ToString(); break;
                    //...
            }
            myDoughnutMachine.DoughnutComplete += new
            DoughnutMachine.DoughnutCompleteDelegate(DoughnutCompleteHandler);

        }
        private void stopToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myDoughnutMachine.Enabled = false;
        }
        private void exitToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateQuantity(selectedDoughnut) > 0)
            {
                lstSale.Items.Add(txtQuantity.Text + " " + selectedDoughnut.ToString() + ":" + txtPrice.Text + " " + double.Parse(txtQuantity.Text) * double.Parse(txtPrice.Text));
            }
            else
            {
                MessageBox.Show("Cantitatea introdusa nu este disponibila in stoc!");
            }
        }
        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            bool v = lstSale.Items.Remove(lstSale.SelectedItem);
        }
        private void btnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            txtTotal.Text = (double.Parse((string)txtTotal.Text) + (double.Parse(txtQuantity.Text) * double.Parse((string)txtPrice.Text))).ToString();
            foreach (string s in lstSale.Items)
            {
                switch (s.Substring(s.IndexOf(" ") + 1, s.IndexOf(":") - s.IndexOf(" ") - 1))
                {
                    case "Glazed":
                        mRaisedGlazed = mRaisedGlazed - Int32.Parse(s.Substring(0, s.IndexOf(" ")));
                        txtGlazedRaised.Text = mRaisedGlazed.ToString();
                        break;
                    case "Sugar":
                        mRaisedSugar = mRaisedSugar - Int32.Parse(s.Substring(0, s.IndexOf(" ")));
                        txtSugarRaised.Text = mRaisedSugar.ToString();
                        break;
                    case "Chocolate":
                        mFilledChocolate = mFilledChocolate - Int32.Parse(s.Substring(0, s.IndexOf(" ")));
                        txtChocolateFilled.Text = mFilledChocolate.ToString();
                        break;
                    case "Lemon":
                        mFilledLemon = mFilledLemon - Int32.Parse(s.Substring(0, s.IndexOf(" ")));
                        txtLemonFilled.Text = mFilledLemon.ToString();
                        break;
                    case "Vanilla":
                        mFilledVanilla = mFilledVanilla - Int32.Parse(s.Substring(0, s.IndexOf(" ")));
                        txtVanillaFilled.Text = mFilledVanilla.ToString();
                        break;
                }
            }
        }
        private void txtQuantity_KeyPress(object sender, KeyEventArgs e)
        {
            if (!(e.Key >= Key.D0 && e.Key <= Key.D9))
            {
                MessageBox.Show("Numai cifre se pot introduce!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void FilledItems_Click(object sender, RoutedEventArgs e)
        {
            string DoughnutFlavour;
            MenuItem SelectedItem = (MenuItem)e.OriginalSource;
            DoughnutFlavour = SelectedItem.Header.ToString();
            Enum.TryParse(DoughnutFlavour, out DoughnutType myFlavour);
            myDoughnutMachine.MakeDoughnuts(myFlavour);
        }
        private void FilledItemsShow_Click(object sender, RoutedEventArgs e)
        {
            string mesaj;
            MenuItem SelectedItem = (MenuItem)e.OriginalSource;
            mesaj = SelectedItem.Header.ToString() + " doughnuts are being cooked!";
            this.Title = mesaj;
        }
        KeyValuePair<DoughnutType, double>[] PriceList = {
new KeyValuePair<DoughnutType, double>(DoughnutType.Sugar, 2.5),
new KeyValuePair<DoughnutType, double>(DoughnutType.Glazed,3),
new KeyValuePair<DoughnutType, double>(DoughnutType.Chocolate,4.5),
new KeyValuePair<DoughnutType, double>(DoughnutType.Vanilla,4),
new KeyValuePair<DoughnutType, double>(DoughnutType.Lemon,3.5)
};
        DoughnutType selectedDoughnut;
        private object lstSale;

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtPrice.Text = cmbType.SelectedValue.ToString();
            KeyValuePair<DoughnutType, double> selectedEntry = NewMethod();
            selectedDoughnut = selectedEntry.Key;
        }

        private static KeyValuePair<DoughnutType, double> NewMethod()
        {
            return (KeyValuePair<DoughnutType, double>)cmbType.SelectedItem;
        }

        private int ValidateQuantity(DoughnutType selectedDoughnut)
        {
            int q = int.Parse(txtQuantity.Text);
            int r = 1;
            switch (selectedDoughnut)
            {
                case DoughnutType.Glazed:
                    if (q > mRaisedGlazed)
                        r = 0;
                    break;
                case DoughnutType.Sugar:
                    if (q > mRaisedSugar)
                        r = 0;
                    break;
                case DoughnutType.Chocolate:
                    if (q > mFilledChocolate)
                        r = 0;
                    break;
                case DoughnutType.Lemon:
                    if (q > mFilledLemon)
                        r = 0;
                    break;
                case DoughnutType.Vanilla:
                    if (q > mFilledVanilla)
                        r = 0;
                    break;
            }
            return r;
        }
        private void CtrlS_CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            //handler pentru comanda Ctrl+S -> se va executa stopToolStripMenuItem_Click
            MessageBox.Show("Ctrl+S was pressed! The doughnut machine will stop!");
            this.stopToolStripMenuItem_Click(sender, e);

        }
    }

    internal class txtTotal
    {
        public static object Text { get; internal set; }
    }

    internal class txtPrice
    {
        public static object Text { get; internal set; }
    }

    internal class txtQuantity
    {
        public static string Text { get; internal set; }
    }

    internal class txtVanillaFilled
    {
        public static string Text { get; internal set; }
    }

    internal class txtChocolateFilled
    {
        public static string Text { get; internal set; }
    }

    internal class txtLemonFilled
    {
        public static string Text { get; internal set; }
    }

    internal class txtSugarRaised
    {
        public static string Text { get; internal set; }
    }

    internal class txtGlazedRaised
    {
        public static string Text { get; internal set; }
    }

    internal class sugarToolStripMenuItem
    {
        public static bool IsChecked { get; internal set; }
    }

    internal class glazedToolStripMenuItem
    {
        public static bool IsChecked { get; internal set; }
    }

    internal class cmbType
    {
        internal static object SelectedValue;

        public static KeyValuePair<DoughnutType, double>[] ItemsSource { get; internal set; }
        public static string DisplayMemberPath { get; internal set; }
        public static string SelectedValuePath { get; internal set; }
        public static KeyValuePair<DoughnutType, double> SelectedItem { get; internal set; }
    }
}
