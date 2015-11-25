using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamEasyKeyActivator
{
    public partial class Form1 : Form
    {
        //titles from http://translation.steampowered.com/Us_And_Them.php?token_key_ID=54280, you need to be a steam translator to access the link
        List<string> activationtitles = new List<string> { "Product Activation", "Aktivace produktu", "Produktaktivering", "Productactivering", "Tuotteen aktivointi", "Activation de produit", "Produktaktivierung", "Termékaktiválás", "Attivazione del prodotto", "製品を有効化", "제품 등록", "Veiviser for aktivering av produkt", "Aktywacja produktu", "Ativação de produto", "Activare Produs", "Активация продукта", "Activación de producto", "Produktaktiveringsguide", "产品激活", "產品啟動", "การเปิดใช้ผลิตภัณฑ์", "Ativação de produto", "Активиране на продукт", "Ενεργοποίηση προϊόντος", "Ürün Etkinleştirme", "Активація продукту", "Kích hoạt sản phẩm" };

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("Steam"); //Checks if steam is running
            if (pname.Length == 0)
            {
                button1.Enabled = false;
                button1.Text = "Please start steam first.";
            }
            else
            {
                button1.Enabled = true;
                button1.Text = "Start Activating";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (string code in richTextBox1.Text.Split(new[] { '\r', '\n' }))
            {
                Process.Start("steam://open/activateproduct"); //opens the activate product window
                Thread.Sleep(100);
                SendKeys.SendWait("{ENTER}"); //Clicks the next button
                Thread.Sleep(100);
                SendKeys.SendWait("{ENTER}"); //Clicks the I agree button
                Thread.Sleep(100);
                SendKeys.SendWait(code); //Types down the key
                Thread.Sleep(100);
                SendKeys.SendWait("{ENTER}"); //Clicks the activate button
                Thread.Sleep(500);
                var currentwindowtitle = GetActiveWindowTitle();
                while (currentwindowtitle.StartsWith("Steam ")) //It shows steam - working, I'd do "Steam -" but some languages use different characters, so this is better than spaghetti code
                {
                    currentwindowtitle = GetActiveWindowTitle();
                    Thread.Sleep(100);
                }
                while (activationtitles.Contains(currentwindowtitle))
                {
                    SendKeys.SendWait("{ESC}"); //Exits the activate product window
                    Thread.Sleep(100);
                    currentwindowtitle = GetActiveWindowTitle();
                }
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

        private void label2_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ardaozkal/Steam_Easy_Key_Activator");
        }
    }
}
