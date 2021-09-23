using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// veri tabanı 
using System.Data.OleDb;
//word
//using word = Microsoft.Office.Interop.Word;

// yansıma kütüphanesi
using System.Reflection;



namespace stajTakipV._1._1
{
    public partial class StajyerGorevleri : Form
    {
        public StajyerGorevleri()
        {
            InitializeComponent();
        }

        //Veri tabanı yolu ve provier 
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.16.0;Data Source=stajTakipData.accdb");

        //
        //Form Load
        private void StajyerGorevleri_Load(object sender, EventArgs e)
        {
            formuGoster();
        }
        //formu göster.
        //
        private void formuGoster()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter kullanicilariListele = new OleDbDataAdapter(
                    "select Tcno,Adı,Soyadı,OgrenciNo,Sınıfı,Bölümü,Programı,FirmaAdı,StajYapacağıBölüm from stajBasvuruFormu Order By Tcno ASC", baglantim);
                DataSet dsHafiza = new DataSet();
                //fill dolduruyor
                kullanicilariListele.Fill(dsHafiza);
                gunaDataGridView1.DataSource = dsHafiza.Tables[0];

                baglantim.Close();

            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }
        }

        //
        //btn kaydet
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            

            try
            {
                baglantim.Open();
                int grvdurum = 0;
                int grvdurum2 = 0;
                int grvdurum3 = 0;
                int grvdurum4 = 0;
                if (CheckBoxhafta1.Checked == true) grvdurum = 1;
                else grvdurum = 0;


                if (CheckBoxhafta2.Checked == true) grvdurum2 = 1;
                else grvdurum2 = 0;


                if (CheckBoxhafta3.Checked == true)  grvdurum3 = 1;
                else grvdurum3 = 0;

                if (CheckBoxhafta4.Checked == true)  grvdurum4 = 1;
                else  grvdurum4 = 0;

                OleDbCommand guncelle = new OleDbCommand("update stajBasvuruFormu set " +
                    "hafta1gorev='" +  txtHafta1.Text + "'," +
                    "hafta2gorev='" + txtHafta2.Text + "'," +
                    "hafta3gorev='" + txtHafta3.Text + "'," +
                    "hafta4gorev='" + txtHafta4.Text + "'," +
                    "hafta1gorevDurum=" + grvdurum + "," +
                    "hafta2gorevDurum=" + grvdurum2 + "," +
                    "hafta3gorevDurum=" + grvdurum3 + "," +
                    "hafta4gorevDurum=" + grvdurum4 + "" +
                    " where Tcno='" + txtAra.Text + "'", baglantim);
                guncelle.ExecuteNonQuery();//güneclle 
                baglantim.Close();
 
                MessageBox.Show("Haftalık Görevler Eklendi.", "STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "hata . STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }

        }

        //
        //ara buton
        private void btnAra_Click(object sender, EventArgs e)
            {


            bool kayitAramaDurumu = false;
            if (btnAra.Text != "")
            {
                baglantim.Open();
                //araclar  tablosundaki tüm alanları seç where tcno alanındaki eşit olanalrı getir 
                OleDbCommand selecsorgu = new OleDbCommand("select * from stajBasvuruFormu where Tcno='" + txtAra.Text + "'", baglantim);
                OleDbDataReader kayitOkuma = selecsorgu.ExecuteReader();
                while (kayitOkuma.Read())
                {
                    kayitAramaDurumu = true;
                    ////   veri tabanından 1 nolu elemanı alıp stringe dönüştürüp yazdırıyoruz
                    //try
                    //{
                    //    pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\aracResimleri\\" + kayitOkuma.GetValue(0).ToString() + ".jpg");
                    //    //bulnunan kaydın 0. plaka alanını aldık.
                    //}
                    //catch
                    //{
                    //    MessageBox.Show("Araç resmi Bulunamadı!!", "VOLKAN RENT A CAR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    //resim yok resmini getireymiyorum çalışmıyor 
                    //    pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\aracResimleri\\resimyok.jpg");
                    //}

                     txtHafta1.Text = kayitOkuma.GetValue(16).ToString();
                     txtHafta2.Text = kayitOkuma.GetValue(17).ToString();
                     txtHafta3.Text = kayitOkuma.GetValue(18).ToString();
                     txtHafta4.Text = kayitOkuma.GetValue(19).ToString();
                    string deger = "";
                    deger = kayitOkuma.GetValue(20).ToString();
                    if (deger == "True")
                        CheckBoxhafta1.Checked = true;
                     else
                        CheckBoxhafta1.Checked = false;

                    deger = kayitOkuma.GetValue(21).ToString();
                    if (deger == "True")
                        CheckBoxhafta2.Checked = true;
                    else
                        CheckBoxhafta2.Checked = false;

                    
                    deger = kayitOkuma.GetValue(22).ToString();
                    if (deger == "True")
                        CheckBoxhafta3.Checked = true;
                    else
                        CheckBoxhafta3.Checked = false;     
                    
                    deger = kayitOkuma.GetValue(23).ToString();
                    if (deger == "True")
                        CheckBoxhafta4.Checked = true;
                    else
                        CheckBoxhafta4.Checked = false;

                    //  break;
                }
                //  eğer yok ise folse ise
                if (kayitAramaDurumu == false)
                {  //Exclamation bilgi msg box
                    MessageBox.Show("Öğrenci Kaydı BULUNAMADI !!", "TAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                baglantim.Close();

            }
            else
            {
                MessageBox.Show("Öğrenci Numarasını doğru Girin!!", "TAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
 
            }
        }

        private void btnDosyaYukle_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {

        }

        //data grid e bastığımızda textbox a aktarımı için
        private void gunaDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Data Grid View çökyordu başlığa tıklayınca çökmemesi için 
            if (e.RowIndex == -1) return;
            txtAra.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
        }


        Font Baslik = new Font("Verdana", 14, FontStyle.Bold);
        Font orta = new Font("Verdana", 11, FontStyle.Bold);
        Font orta2 = new Font("Verdana", 12, FontStyle.Bold);
        Font govde = new Font("Verdana", 11);
        Pen myPen = new Pen(Color.Black);
        SolidBrush sb = new SolidBrush(Color.Black);

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
       
            StringFormat sFormat = new StringFormat();


                sFormat.Alignment = StringAlignment.Near;                                      // yatay // dikey
                e.Graphics.DrawString("NAMIK KEMAL ÜNİVERSİTESİ STAJ ÖDEVLERİ", Baslik, sb, 100, 50);

                e.Graphics.DrawString(CheckBoxhafta1.Text, orta, sb, 50, 150);
                if (CheckBoxhafta1.Checked ==true)
                    e.Graphics.DrawString("Ödev Yapıldı", orta, sb, 450, 150);
                else
                e.Graphics.DrawString("Ödev Yapılmadı!", orta, sb, 450, 150);

                e.Graphics.DrawString(txtHafta1.Text, govde, sb, 50, 200);
            

                e.Graphics.DrawString(CheckBoxhafta2.Text, orta, sb, 50, 350);
                if (CheckBoxhafta2.Checked == true) 
                    e.Graphics.DrawString("Ödev Yapıldı", orta, sb, 450, 350);
                else
                e.Graphics.DrawString("Ödev Yapılmadı!", orta, sb, 450, 350);

                e.Graphics.DrawString(txtHafta2.Text, govde, sb, 50, 400);

                        

                e.Graphics.DrawString(CheckBoxhafta3.Text, orta, sb, 50, 600);
                if (CheckBoxhafta3.Checked == true) 
                    e.Graphics.DrawString("Ödev Yapıldı", orta, sb, 450, 300);
                else
                e.Graphics.DrawString("Ödev Yapılmadı!", orta, sb, 450, 600);

                e.Graphics.DrawString(txtHafta3.Text, govde, sb, 50, 650);

                        

                e.Graphics.DrawString(CheckBoxhafta4.Text, orta, sb, 50, 800);
                if (CheckBoxhafta4.Checked == true) 
                    e.Graphics.DrawString("Ödev Yapıldı", orta, sb, 450, 800);
                else
                e.Graphics.DrawString("Ödev Yapılmadı!", orta, sb, 450, 800);

                e.Graphics.DrawString(txtHafta4.Text, govde, sb, 50, 950);



        }

        private void StajyerGorevleri_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadece rakam girişi
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
