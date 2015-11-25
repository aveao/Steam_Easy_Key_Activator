using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;

namespace SEKA_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //titles from http://translation.steampowered.com/Us_And_Them.php?token_key_ID=54280, you need to be a steam translator to access the link
        List<string> activationtitles = new List<string> { "Product Activation", "Aktivace produktu", "Produktaktivering", "Productactivering", "Tuotteen aktivointi", "Activation de produit", "Produktaktivierung", "Termékaktiválás", "Attivazione del prodotto", "製品を有効化", "제품 등록", "Veiviser for aktivering av produkt", "Aktywacja produktu", "Ativação de produto", "Activare Produs", "Активация продукта", "Activación de producto", "Produktaktiveringsguide", "产品激活", "產品啟動", "การเปิดใช้ผลิตภัณฑ์", "Ativação de produto", "Активиране на продукт", "Ενεργοποίηση προϊόντος", "Ürün Etkinleştirme", "Активація продукту", "Kích hoạt sản phẩm" };

        public MainWindow()
        {
            InitializeComponent();

            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("Steam"); //Checks if steam is running
            if (pname.Length == 0)
            {
                button.IsEnabled = false;
                button.Content = "Please start steam first.";
            }
            else
            {
                button.IsEnabled = true;
                button.Content = "Start Activating";
            }
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        { //http://stackoverflow.com/questions/115868/how-do-i-get-the-title-of-the-current-active-window-using-c
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {

            foreach (string code in new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text.Split(new[] { '\r', '\n' }))
            {
                Process.Start("steam://open/activateproduct"); //opens the activate product window
                await Task.Delay(100);
                System.Windows.Forms.SendKeys.SendWait("{ENTER}"); //Clicks the next button
                await Task.Delay(100);
                System.Windows.Forms.SendKeys.SendWait("{ENTER}"); //Clicks the I agree button
                await Task.Delay(100);
                System.Windows.Forms.SendKeys.SendWait(code); //Types down the key
                await Task.Delay(100);
                System.Windows.Forms.SendKeys.SendWait("{ENTER}"); //Clicks the activate button
                await Task.Delay(500);
                var currentwindowtitle = GetActiveWindowTitle();
                while (currentwindowtitle.StartsWith("Steam ")) //It shows steam - working, I'd do "Steam -" but some languages use different characters, so this is better than spaghetti code
                {
                    currentwindowtitle = GetActiveWindowTitle();
                    await Task.Delay(100);
                }
                while (activationtitles.Contains(currentwindowtitle))
                {
                    System.Windows.Forms.SendKeys.SendWait("{ESC}"); //Exits the activate product window
                    await Task.Delay(100);
                    currentwindowtitle = GetActiveWindowTitle();
                }
            }
        }

        private void label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/ardaozkal/Steam_Easy_Key_Activator");
        }
    }
}
