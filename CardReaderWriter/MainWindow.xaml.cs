
using NFC_CardReader;
using NFC_CardReader.ACR122U;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;


namespace CardReaderWriter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WinSmartCardContext Context;
        WinSmartCard WSC;
        ACR122U_SmartCard ACR122U_SmartCard;
        DispatcherTimer timer = new DispatcherTimer();
        
        string prevUID;
        private string SERIAL_NUMBER_Regex = @"^(?=\d{0,8}$)";
        private string SERIAL_NUMBER_Regex_full = @"^(?=\d{8}$)";
        private const int port = 8888;
        private const string server = "127.0.0.1";
        private byte[] serverData = new byte[1023];
        byte[] trailer1Key = new byte[6];
        byte[] trailer2Key = new byte[6];
        byte[] trailer3Key = new byte[6];
        byte[] trailer4Key = new byte[6];
        byte[] trailer5Key = new byte[6];
        byte[] trailer6Key = new byte[6];
        byte[] trailer7Key = new byte[6];
        byte[] trailer8Key = new byte[6];
        byte[] trailer9Key = new byte[6];
        byte[] trailer10Key = new byte[6];
        byte[] trailer11Key = new byte[6];
        byte[] trailer12Key = new byte[6];
        byte[] trailer13Key = new byte[6];
        byte[] trailer14Key = new byte[6];
        byte[] trailer15Key = new byte[6];
        byte[] trailer16Key = new byte[6];

        public MainWindow()
        {
            Directory.CreateDirectory(@AppDomain.CurrentDomain.BaseDirectory + "/logs");
            InitializeComponent();
            InitReader();
            SERIAL_NUMBERTextBox.Text = Convert.ToString(Properties.Settings.Default.SERIAL_NUMBER);
            
        }

        private void SERIAL_NUMBERTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = (sender as TextBox).Text;
            if (!Regex.IsMatch(input, SERIAL_NUMBER_Regex))
            {
                if (SERIAL_NUMBERTextBox.Text.Length != 0)
                    SERIAL_NUMBERTextBox.Text = SERIAL_NUMBERTextBox.Text.Remove(SERIAL_NUMBERTextBox.Text.Length - 1);
                SERIAL_NUMBERTextBox.Select(SERIAL_NUMBERTextBox.Text.Length, 0);
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TryWriteCardAutoMode();
        }
        private bool AthenticateAndWriteAllBlocks()
        {

            byte[] trailer1String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer2String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer3String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer4String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer5String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer6String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer7String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer8String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer9String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer10String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer11String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer12String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer13String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer14String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer15String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            byte[] trailer16String = new byte[16] { 255, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 };
            for (int i = 0; i < 16; i++)
            {
                trailer1String[i] = serverData[i];
                if (i < 6)
                    trailer1Key[i] = serverData[i];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer2String[i] = serverData[i + 16];
                if (i < 6)
                    trailer2Key[i] = serverData[i + 16];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer3String[i] = serverData[i + 32];
                if (i < 6)
                    trailer3Key[i] = serverData[i + 32];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer4String[i] = serverData[i + 48];
                if (i < 6)
                    trailer4Key[i] = serverData[i + 48];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer5String[i] = serverData[i + 64];
                if (i < 6)
                    trailer5Key[i] = serverData[i + 64];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer6String[i] = serverData[i + 80];
                if (i < 6)
                    trailer6Key[i] = serverData[i + 80];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer7String[i] = serverData[i + 96];
                if (i < 6)
                    trailer7Key[i] = serverData[i + 96];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer8String[i] = serverData[i + 112];
                if (i < 6)
                    trailer8Key[i] = serverData[i + 112];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer9String[i] = serverData[i + 128];
                if (i < 6)
                    trailer9Key[i] = serverData[i + 128];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer10String[i] = serverData[i + 144];
                if (i < 6)
                    trailer10Key[i] = serverData[i + 144];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer11String[i] = serverData[i + 160];
                if (i < 6)
                    trailer11Key[i] = serverData[i + 160];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer12String[i] = serverData[i + 176];
                if (i < 6)
                    trailer12Key[i] = serverData[i + 176];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer13String[i] = serverData[i + 192];
                if (i < 6)
                    trailer13Key[i] = serverData[i + 192];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer14String[i] = serverData[i + 208];
                if (i < 6)
                    trailer14Key[i] = serverData[i + 208];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer15String[i] = serverData[i + 224];
                if (i < 6)
                    trailer15Key[i] = serverData[i + 224];

            }
            for (int i = 0; i < 16; i++)
            {
                trailer16String[i] = serverData[i + 240];
                if(i < 6)
                    trailer16Key[i] = serverData[i + 240];

            }

            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, new byte[6] { 255, 255, 255, 255, 255, 255 }); //try standard key
            if(string.Compare(Convert.ToString(ACR122U_SmartCard.Athentication(3, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1)), "Success") == 0)
            {
                Trace.WriteLine(ACR122U_SmartCard.Athentication(3, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer1String, 3);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(7, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer2String, 7);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(11, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer3String, 11);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(15, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer4String, 15);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(19, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer5String, 19);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(23, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer6String, 23);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(27, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer7String, 27);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(31, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer8String, 31);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(35, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer9String, 35);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(39, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer10String, 39);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(43, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer11String, 43);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(47, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer12String, 47);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(51, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer13String, 51);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(55, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer14String, 55);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(59, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer15String, 59);
                Trace.WriteLine(ACR122U_SmartCard.Athentication(63, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
                ACR122U_SmartCard.WriteBlock(trailer16String, 63);
            }
            

            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer1Key);
            Trace.WriteLine("Athentication Key to sector 0: " + ACR122U_SmartCard.Athentication(0, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 1: " + ACR122U_SmartCard.Athentication(1, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 2: " + ACR122U_SmartCard.Athentication(2, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            int j = 257;  //начинаем запись с 257 байта ( первые 256 - трейлеры)
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 1);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 2);

            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer2Key);
            Trace.WriteLine("Athentication Key to sector 4: " + ACR122U_SmartCard.Athentication(4, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 5: " + ACR122U_SmartCard.Athentication(5, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 6: " + ACR122U_SmartCard.Athentication(6, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 4);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 5);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 6);

            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer3Key);
            Trace.WriteLine("Athentication Key to sector 8: " + ACR122U_SmartCard.Athentication(8, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 9: " + ACR122U_SmartCard.Athentication(9, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 10: " + ACR122U_SmartCard.Athentication(10, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 8);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 9);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 10);

            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer4Key);
            Trace.WriteLine("Athentication Key to sector 12: " + ACR122U_SmartCard.Athentication(12, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 13: " + ACR122U_SmartCard.Athentication(13, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 14: " + ACR122U_SmartCard.Athentication(14, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 12);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 13);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 14);

            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer5Key);
            Trace.WriteLine("Athentication Key to sector 16: " + ACR122U_SmartCard.Athentication(16, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 17: " + ACR122U_SmartCard.Athentication(17, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 18: " + ACR122U_SmartCard.Athentication(18, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 16);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 17);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 18);

            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer6Key);
            Trace.WriteLine("Athentication Key to sector 20: " + ACR122U_SmartCard.Athentication(20, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 21: " + ACR122U_SmartCard.Athentication(21, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 22: " + ACR122U_SmartCard.Athentication(22, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 20);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 21);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 22);

            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer7Key);
            Trace.WriteLine("Athentication Key to sector 24: " + ACR122U_SmartCard.Athentication(24, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 25: " + ACR122U_SmartCard.Athentication(25, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 26: " + ACR122U_SmartCard.Athentication(26, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 24);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 25);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 26);

            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer8Key);
            Trace.WriteLine("Athentication Key to sector 28: " + ACR122U_SmartCard.Athentication(28, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 29: " + ACR122U_SmartCard.Athentication(29, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 30: " + ACR122U_SmartCard.Athentication(30, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 28);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 29);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 30);
            
            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer9Key);
            Trace.WriteLine("Athentication Key to sector 32: " + ACR122U_SmartCard.Athentication(32, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 33: " + ACR122U_SmartCard.Athentication(33, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 34: " + ACR122U_SmartCard.Athentication(34, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 32);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 33);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 34);
            
            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer10Key);
            Trace.WriteLine("Athentication Key to sector 36: " + ACR122U_SmartCard.Athentication(36, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 37: " + ACR122U_SmartCard.Athentication(37, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 38: " + ACR122U_SmartCard.Athentication(38, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 36);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 37);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 38);
            
            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer11Key);
            Trace.WriteLine("Athentication Key to sector 40: " + ACR122U_SmartCard.Athentication(40, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 41: " + ACR122U_SmartCard.Athentication(41, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 42: " + ACR122U_SmartCard.Athentication(42, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 40);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 41);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 42);
            
            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer12Key);
            Trace.WriteLine("Athentication Key to sector 44: " + ACR122U_SmartCard.Athentication(44, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 45: " + ACR122U_SmartCard.Athentication(45, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 46: " + ACR122U_SmartCard.Athentication(46, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 44);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 45);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 46);
            
            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer13Key);
            Trace.WriteLine("Athentication Key to sector 48: " + ACR122U_SmartCard.Athentication(48, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 49: " + ACR122U_SmartCard.Athentication(49, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 50: " + ACR122U_SmartCard.Athentication(50, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 48);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 49);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 50);
            
            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer14Key);
            Trace.WriteLine("Athentication Key to sector 52: " + ACR122U_SmartCard.Athentication(5, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 53: " + ACR122U_SmartCard.Athentication(53, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 54: " + ACR122U_SmartCard.Athentication(54, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 52);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 53);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 54);
            
            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer15Key);
            Trace.WriteLine("Athentication Key to sector 56: " + ACR122U_SmartCard.Athentication(56, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 57: " + ACR122U_SmartCard.Athentication(57, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 58: " + ACR122U_SmartCard.Athentication(58, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 56);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 57);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 58);
            
            ACR122U_SmartCard.LoadAthenticationKeys(ACR122U_KeyMemories.Key1, trailer16Key);
            Trace.WriteLine("Athentication Key to sector 60: " + ACR122U_SmartCard.Athentication(60, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 61: " + ACR122U_SmartCard.Athentication(61, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            Trace.WriteLine("Athentication Key to sector 62: " + ACR122U_SmartCard.Athentication(62, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 60);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 61);
            ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 62);

            //повторная проверочная запись
            Trace.WriteLine("Athentication Key to sector 1: " + ACR122U_SmartCard.Athentication(1, ACR122U_Keys.KeyA, ACR122U_KeyMemories.Key1));
            j = 257;  //начинаем запись с 257 байта ( первые 256 - трейлеры)
            return string.Compare(Convert.ToString(ACR122U_SmartCard.WriteBlock(new byte[16] { serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++], serverData[j++] }, 1)), "Success") == 0 ? true : false; 
            
        }
        private bool IsAthenticateTrailers()
        {        
            return true;
        }
        


        private void Button_WriteSERIAL_NUMBER_Click(object sender, RoutedEventArgs e)
        {
            string input = SERIAL_NUMBERTextBox.Text;
            if (!Regex.IsMatch(input, SERIAL_NUMBER_Regex_full))
            {
                MessageBox.Show("Write ERROR: SERIAL должен состоять из 8 цифр!");
                ToSessionLog("Ошибка записи: SERIAL должен состоять из 8 цифр");
            }
            else
            {
                try
                {

                    if (TryInitCard())
                    {
                        
                            if (SendToServer(ACR122U_SmartCard.GetcardUID(), Convert.ToInt32(SERIAL_NUMBERTextBox.Text)))
                            {
                            if (IsAthenticateTrailers())
                            {
                                
                                
                                if (AthenticateAndWriteAllBlocks())
                                {
                                    
                                    Properties.Settings.Default.SERIAL_NUMBER = Convert.ToInt32(SERIAL_NUMBERTextBox.Text);
                                    Properties.Settings.Default.Save();

                                    ToSessionLog("Успешная ручная запись на карту: " + ACR122U_SmartCard.GetcardUID());
                                    LogInfoTextBlock.Text += LogInfo(ACR122U_SmartCard.GetcardUID().Replace("-", string.Empty), ReadSERIAL_NUMBER(), ReadTYPE());
                                    WroteCardLog(LogInfo(ACR122U_SmartCard.GetcardUID().Replace("-", string.Empty), ReadSERIAL_NUMBER(), ReadTYPE()));
                                }
                                else
                                {
                                    MessageBox.Show("Write ERROR: Ошибка записи!");
                                    ToSessionLog("Ошибка ручной записи: Ошибка записи! ");
                                }


                            }
                            else
                            {
                                MessageBox.Show("Write ERROR: Нету доступа до сервера или карты!");
                                ToSessionLog("Ошибка ручной записи: Нету доступа до сервера или карты!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Write ERROR: Нету доступа к сектору памяти!");
                            ToSessionLog("Ошибка ручной записи: Нету доступа к сектору памяти!");
                        }

                        


                    }
                    
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    ToSessionLog("Ошибка ручной записи: " + ex.Message);
                    MessageBox.Show("Write ERROR: поднесите карту!");
                }

            }


        }
        private string LogInfo(string UID, int TYPE, int SERIAL_NUMBER)
        {
            return SERIAL_NUMBER + "," + UID + "," + TYPE + "\n";
        }

        private void Button_ReadInfo_Click(object sender, RoutedEventArgs e)
        {
            if (TryInitCard())
                MessageBox.Show(BitConverter.ToString(serverData));

        }

        private bool TryInitCard()
        {
            try
            {
                WSC = Context.CardConnect(SmartCardShareTypes.SCARD_SHARE_SHARED);
                ACR122U_SmartCard = new ACR122U_SmartCard(WSC);
                UIDTextBlock.Foreground = Brushes.Green;
                UIDTextBlock.Text = "UDI: " + ACR122U_SmartCard.GetcardUID();              
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Write ERROR: поднесите карту!");
                ToSessionLog("Ошибка ручной записи: " + e.Message);
                Trace.WriteLine("TryInitCard " + e.Message);
                UIDTextBlock.Foreground = Brushes.Red;
                UIDTextBlock.Text = "UDI: " + prevUID;
                prevUID = "No card";
                return false;
            }

        }
        private bool InitReader()
        {
            try
            {
                List<string> Names = WinSmartCardContext.ListReadersAsStringsStatic();               
                Names = Names.Where(x => x.Contains("ACS ACR122") && !x.Contains("ACS ACR122U PICC Interface")).ToList();              
                int Selection = 0;
                ReaderNameTextBlock.Text = Names[Selection];
                Context = new WinSmartCardContext(OperationScopes.SCARD_SCOPE_SYSTEM, Names[Selection]);
                WSC = Context.CardConnect(SmartCardShareTypes.SCARD_SHARE_SHARED);
                ReaderStatusTextBlock.Foreground = Brushes.Green;
                ReaderStatusTextBlock.Text = "Reader подключен";
                ToSessionLog("Проверка ридера: Reader подключен");
                return true;
            }

            catch (NFC_CardReader.WinSCardException e)
            {
                if (Convert.ToString(Context.LastResultCode) == "SCARD_E_UNKNOWN_READER")
                {
                    ReaderStatusTextBlock.Foreground = Brushes.Red;
                    ReaderStatusTextBlock.Text = "Reader НЕ подключен";
                    ToSessionLog("Проверка ридера: Reader НЕ подключен");
                    MessageBox.Show(e.Message);
                    return false;
                }
                else
                {
                    ReaderStatusTextBlock.Foreground = Brushes.Green;
                    ReaderStatusTextBlock.Text = "Reader подключен";
                    ToSessionLog("Проверка ридера: Reader подключен");
                    return true;
                }
            }
            catch (Exception exe)
            {
                MessageBox.Show(exe.Message);
                return false;
            }
        }
        private int ReadTYPE()
        {
            try
            {
                
                String TYPE = SERIAL_NUMBERTextBox.Text.Substring(0, 2);
                return Convert.ToInt32(TYPE);
            } catch(Exception ex)
            {
                MessageBox.Show("Возможно нету доступа до сектора:" + ex.Message);
                ToSessionLog("Возможно нету доступа до сектора:" + ex.Message);
                return 0;
            }
            
        }

        //NOT USE STILL
        private bool WriteSERIAL_NUMBER(int SERIAL_NUMBER)
        {
            return string.Compare(Convert.ToString(ACR122U_SmartCard.WriteValueToBlock(SERIAL_NUMBER, 1)), "Success") == 0 ? true : false;
        }

        private int ReadSERIAL_NUMBER()
        {
            
            return Convert.ToInt32(SERIAL_NUMBERTextBox.Text);
        }

        private bool SendToServer(string UID, int SERIAL_NUMBER)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(server, port);
                StringBuilder response = new StringBuilder();
                NetworkStream stream = client.GetStream();

                string message = String.Format(Convert.ToString(SERIAL_NUMBER));
                UID = UID.Replace("-", string.Empty);
                message += String.Format("." + UID);
                //массив байтов для отправки
                byte[] Tdata = Encoding.UTF8.GetBytes(message);
                stream.Write(Tdata, 0, Tdata.Length);
                //прием ответа с сервера
                do
                {
                    int bytes = stream.Read(serverData, 0, serverData.Length);
                    response.Append(Encoding.UTF8.GetString(serverData, 0, bytes));
                }
                while (stream.DataAvailable); // пока данные есть в поток
                
                // Закрываем потоки
                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Trace.WriteLine("SocketException: {0}", e.Message);
                return false;
            }
            catch (Exception e)
            {
                Trace.WriteLine("Exception: {0}", e.Message);
                return false;
            }
            return true;
        }
        private void AutoWriteCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (TryWriteCardAutoMode())
            {
                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
                timer.Start();
            }
            else
                AutoWriteCheckBox.IsChecked = false;
        }

        private void AutoWriteCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Uncheked!");
            timer.Stop();
        }

        private bool TryWriteCardAutoMode()
        {          
            try
            {
                WSC = Context.CardConnect(SmartCardShareTypes.SCARD_SHARE_SHARED);
                ACR122U_SmartCard = new ACR122U_SmartCard(WSC);
                UIDTextBlock.Foreground = Brushes.Green;
                UIDTextBlock.Text = "UDI: " + ACR122U_SmartCard.GetcardUID();
                if (prevUID != ACR122U_SmartCard.GetcardUID())
                {
                    string input = SERIAL_NUMBERTextBox.Text;
                    if (!Regex.IsMatch(input, SERIAL_NUMBER_Regex_full))
                    {
                        CardAlertTextBlock.Text = "";
                        MessageBox.Show("Write ERROR: SERIAL должен состоять из 8 цифр!");
                        ToSessionLog("Ошибка записи в авто режиме: Write ERROR: SERIAL должен состоять из 8 цифр!");
                        return false;
                    }
                    else
                    {
                        try
                        {
                            if (SendToServer(ACR122U_SmartCard.GetcardUID(), Convert.ToInt32(SERIAL_NUMBERTextBox.Text)))
                            {
                                if(IsAthenticateTrailers())
                                {
                                    
                                   
                                    if (AthenticateAndWriteAllBlocks())
                                    {
                                        LogInfoTextBlock.Text += LogInfo(ACR122U_SmartCard.GetcardUID().Replace("-", string.Empty), ReadSERIAL_NUMBER(), ReadTYPE());
                                        WroteCardLog(LogInfo(ACR122U_SmartCard.GetcardUID().Replace("-", string.Empty), ReadSERIAL_NUMBER(), ReadTYPE()));
                                    }
                                    else
                                    {
                                        CardAlertTextBlock.Text = "";
                                        MessageBox.Show("Write ERROR: ошибка записи");
                                        ToSessionLog("Ошибка записи в авто режиме: Write ERROR: ошибка записи");
                                        return false;
                                    }
                                } else
                                {
                                    MessageBox.Show("Write ERROR: Возможно, нету доступа к секторам");
                                }
                                

                            }
                            else
                            {
                                CardAlertTextBlock.Text = "";
                                MessageBox.Show("Write ERROR: Сервер не отвечает!");
                                ToSessionLog("Ошибка записи в авто режиме: Write ERROR: Сервер не отвечает!");
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.Message);
                            CardAlertTextBlock.Text = "";
                            MessageBox.Show("Write ERROR: поднесите карту!");
                            ToSessionLog("Ошибка записи в авто режиме: " + ex.Message);
                            return false;
                        }

                    }
                    prevUID = ACR122U_SmartCard.GetcardUID();
                    CardAlertTextBlock.Text = "";
                    Properties.Settings.Default.SERIAL_NUMBER = Convert.ToInt32(SERIAL_NUMBERTextBox.Text) + 1; ;
                    Properties.Settings.Default.Save();
                    SERIAL_NUMBERTextBox.Text = Convert.ToString(Properties.Settings.Default.SERIAL_NUMBER);
                    ToSessionLog("Успешная запись в авто режиме на карту: " + ACR122U_SmartCard.GetcardUID());
                    return true;
                }
                //ToSessionLog("Неизвестная ошибка записи карты");
                return false;
                
            }
            catch (Exception e)
            {
                CardAlertTextBlock.Foreground = Brushes.Red;
                CardAlertTextBlock.Text = "Поднесите карту";
                //ToSessionLog("Ошибка записи в авто режиме: " + e.Message);
                Trace.WriteLine("TryInitCard " + e.Message);
                UIDTextBlock.Foreground = Brushes.Red;
                UIDTextBlock.Text = "UDI: " + prevUID;
                prevUID = "No card";
                return false;
            }
        }

        private void CheckReader_Click(object sender, RoutedEventArgs e)
        {
            InitReader();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(@AppDomain.CurrentDomain.BaseDirectory + "/logs"))
                Directory.CreateDirectory(@AppDomain.CurrentDomain.BaseDirectory + "/logs");
            Process.Start(@AppDomain.CurrentDomain.BaseDirectory + "/logs");          
        }

        private void ToSessionLog(string log)
        {
            FileStream SessionLogFile = new FileStream(@AppDomain.CurrentDomain.BaseDirectory + "/logs/SessionsLog.txt", FileMode.Append);
            StreamWriter writer = new StreamWriter(SessionLogFile);
            string LogStr = "";
            LogStr = Convert.ToString(DateTime.Now);
            LogStr += "  " + log;
            writer.WriteLine(LogStr);
            writer.Close();           
        }
        private void WroteCardLog(string log)
        {
            FileStream CardLogFile = new FileStream(@AppDomain.CurrentDomain.BaseDirectory + "/logs/WroteCardLog.txt", FileMode.Append);
            StreamWriter writer = new StreamWriter(CardLogFile);
            //string LogStr = "";
            //LogStr = Convert.ToString(DateTime.Now);
            //LogStr += "  " + log;
            writer.WriteLine(log);
            writer.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(@AppDomain.CurrentDomain.BaseDirectory + "/logs/WroteCardLog.txt", String.Empty);
        }
    }
}
