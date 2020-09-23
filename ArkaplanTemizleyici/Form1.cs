using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArkaplanTemizleyici
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string adres = string.Empty;
        string[] dosya = new string[1];
        public static string DosyaAdresi = string.Empty;
        string[] _surukleBirakDosyasi = new string[1];
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void label1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                //Drag drop
                _surukleBirakDosyasi = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                foreach (string surukleBirak in _surukleBirakDosyasi)
                    DosyaAdresi = surukleBirak;
                //Drag drop

                //Api 
                using (var client = new HttpClient())
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Headers.Add("X-Api-Key", "1nZm2g5D3tgvJ5jEYcdRt8iq");
                    formData.Add(new ByteArrayContent(File.ReadAllBytes(DosyaAdresi)), "image_file", DosyaAdresi);
                    formData.Add(new StringContent("auto"), "size");
                    var response = client.PostAsync("https://api.remove.bg/v1.0/removebg", formData).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        FileStream fileStream = new FileStream("no-bg.png", FileMode.Create, FileAccess.Write, FileShare.None);
                        response.Content.CopyToAsync(fileStream).ContinueWith((copyTask) => { fileStream.Close(); });
                        MessageBox.Show("Arkaplanı temizlenmiş dosyayı programın bulunduğu klasörde bulabilirsiniz.","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        Process.Start(Application.StartupPath);
                    }
                    else
                    {
                        MessageBox.Show("Hata: " + response.Content.ReadAsStringAsync().Result);
                    }
                }
                //Api 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
