using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace stajTakipV._1._1
{
    public partial class BasvuruFormu : Form
    {
        //OleDbConnection = con;
        //OleDbCommand =cmd;
        //OleDbDataReader= dr;

        public BasvuruFormu()
        {
            InitializeComponent();
        }

        //Veri tabanı yolu ve provier 
      OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.16.0;Data Source=stajTakipData.accdb");

        //
        //Load
        private void Form1_Load(object sender, EventArgs e)
        {

            formuGoster();
            combobox1islev();
            comboboxVarsayilan();
        }
        private void comboboxVarsayilan()
        {
            comboBoxBolum.Text = "BÖLÜMÜNÜZÜ SEÇİN";
            comboBoxProgram.Text = "PROGRAMIMINIZI SEÇİN";
        }
            //
            //combo box 1
            private void combobox1islev()
        {
            baglantim.Open();
            DataTable dt = new DataTable();
            OleDbDataAdapter sorgu = new OleDbDataAdapter("SELECT * FROM tbmyoBolumler ORDER BY id ASC", baglantim);
            sorgu.Fill(dt);
            comboBoxBolum.ValueMember = "id";
            comboBoxBolum.DisplayMember = "bolum";
            comboBoxBolum.DataSource = dt;
        }

        //
        //seçili olana göre yazıyoruz
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxBolum.SelectedIndex !=-1)
            {
                DataTable dt = new DataTable();
                OleDbDataAdapter sorgu2 = new OleDbDataAdapter("SELECT * FROM tbmyoProgramlar where bolumid=" + comboBoxBolum.SelectedValue, baglantim);
                sorgu2.Fill(dt);
                comboBoxProgram.ValueMember = "id";
                comboBoxProgram.DisplayMember = "programlar";
                comboBoxProgram.DataSource = dt;

            }

        }

        //formu göster.
        //
        private void formuGoster()
        {     
            try
            {
                baglantim.Open();
                OleDbDataAdapter kullanicilariListele = new OleDbDataAdapter(
                    "select Tcno,Adı,Soyadı,BabaAdı,DoğumTarihi,OgrenciNo,Sınıfı,Telefon,Bölümü,Programı,FirmaAdı,FirmaAdresi,StajSorumlusu,FirmaTel,StajYapacağıBölüm from stajBasvuruFormu Order By Tcno ASC", baglantim);
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
        //kaydet buton
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            baglantim.Close();
            bool kayitKontrol = false;
            //ekleyeceğimiz kişi daha önce varmı yokmu bakıyoruz
            baglantim.Open();
            OleDbCommand selectSorgu = new OleDbCommand("select * from stajBasvuruFormu where Tcno='" + txtTc.Text + "'", baglantim);
            OleDbDataReader kayitOkuma = selectSorgu.ExecuteReader();
            while (kayitOkuma.Read())
            {
                kayitKontrol = true;
                break;
            }
            baglantim.Close();
            //girilen de veri tabanın da yok ise kayıt yapıyoz
            if (kayitKontrol == false)
            {
                if (txtTc.Text.Length < 11 || txtTc.Text == "")
                    gunaLabeltc.ForeColor = Color.Red;
                else
                    gunaLabeltc.ForeColor = Color.Black;


                if (txtTc.Text.Length==11 && txtTc.Text != "")
                {

                
                    try
                    {
                            baglantim.Open();
                            OleDbCommand ekle = new OleDbCommand("insert  into stajBasvuruFormu (Tcno,Adı,Soyadı,BabaAdı,DoğumTarihi,OgrenciNo,Sınıfı,Telefon,Bölümü,Programı,FirmaAdı,FirmaAdresi,StajSorumlusu,FirmaTel,StajYapacağıBölüm) values" + 
                                "( '" + txtTc.Text + "'," +
                                "'" + txtad.Text + "'," +
                                "'" + txtsoyad.Text + "', " +
                                "'" + txtbabaAdı.Text + "'," +
                               "'" + dateTimeDogum.Text + "'," +
                                "'" + txtOgrencino.Text + "'," +
                                "'" + txtSınıf.Text + "'," +
                                "'" + txtCepTel.Text + "'," +
                                "'" + comboBoxBolum.Text + "'," +
                             "'" + comboBoxProgram.Text + "'," +
                             "'" + txtFirmaAd.Text + "'," +
                             "'" + txtAdres.Text + "'," +
                             "'" + txtsorumlu.Text + "'," +
                             "'" + txtisyeritel.Text + "'," +
                             "'" + txtStajyb.Text + "'" +
                             ")", baglantim);
                            ekle.ExecuteNonQuery();
                            baglantim.Close();
                            MessageBox.Show("Kayıt başarılı bir şekilde oluşturuldu.", "STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            fomruTemizle();
                            formuGoster();
                     }
                        catch (Exception hatamsj)
                        {

                            MessageBox.Show(hatamsj.Message, "Kayıt oluşturulamadı STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            baglantim.Close();

                        }
                }
                else
                {
                    MessageBox.Show("Kırmızı alanları doğru girin", "VOLKAN YILDIZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
                MessageBox.Show("Girilen Tc No kayıtlıdır.", "STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }


        //
        //btn yazdır
        private void btnYazdir_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
        //
        //yazdırma
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
            e.Graphics.DrawString("NAMIK KEMAL ÜNİVERSİTESİ STAJ BAŞVURU FORMU\n", Baslik, sb, 100, 50);
            e.Graphics.DrawString("Teknik Bilimler M.Y.O. öğrencileri mezun olabilmek için yüm derslerinden başarılı olmanın", govde, sb, 60, 100);
            e.Graphics.DrawString("yanında 30 iş günü staj yapmak zorundadır.Öğrencilerimiz, işletmelerde iş yeri stajına  ", govde, sb, 60, 120);
            e.Graphics.DrawString("devam ettikleri sürece 5510 sayılı Sosyal sigortalar kanunu 4. maddesi birinci fıkrasının", govde, sb, 60, 140);
            e.Graphics.DrawString("(a) bendine göre iş kazası ve hastalığı sigortası M.Y.O Müdürlüğünmüzce  yapılacaktır.", govde, sb, 60,160);
            //.., .., uzunluk, düzlük 
            e.Graphics.DrawLine(myPen, 60, 180, 800, 180);

            e.Graphics.DrawString("T.C Kimlik No:", orta, sb, 60, 200);  e.Graphics.DrawString(txtTc.Text , govde, sb, 200, 200);
            e.Graphics.DrawString("Adı                :", orta, sb, 60, 220);  e.Graphics.DrawString(txtad.Text , govde, sb, 200, 220);
            e.Graphics.DrawString("Soyadı          :", orta, sb, 60, 240);  e.Graphics.DrawString(txtsoyad.Text , govde, sb, 200, 240);
            e.Graphics.DrawString("Baba Adı       :", orta, sb, 60, 260);  e.Graphics.DrawString(txtbabaAdı.Text , govde, sb, 200, 260);
            e.Graphics.DrawString("Doğum Tarihi :", orta, sb, 60, 280);  e.Graphics.DrawString(dateTimeDogum.Text , govde, sb, 200, 280);
                    //üst kısmın yan tarafı -> 
                e.Graphics.DrawString("Öğrenci No:", orta, sb, 320, 200);  e.Graphics.DrawString(txtOgrencino.Text , govde, sb, 440, 200);
                e.Graphics.DrawString("Sınıfı         :", orta, sb, 320, 220);  e.Graphics.DrawString(txtSınıf.Text , govde, sb, 440, 220);
                e.Graphics.DrawString("Telefon      :", orta, sb, 320, 240);  e.Graphics.DrawString(txtCepTel.Text , govde, sb, 440, 240);
                e.Graphics.DrawString("Bölümü     :", orta, sb, 320, 260); e.Graphics.DrawString(comboBoxBolum.Text, govde, sb, 440, 260);
                e.Graphics.DrawString("Programı  :", orta, sb, 320, 280); e.Graphics.DrawString(comboBoxProgram.Text, govde, sb, 440, 280);

            //.., .., uzunluk, düzlük 
            e.Graphics.DrawLine(myPen, 60, 345, 800, 345);
            e.Graphics.DrawString("Öğrencinin Staj Yapacağı Kurum veya Kuruluş ", orta2, sb, 150, 350); 
            //.., .., uzunluk, düzlük 
            e.Graphics.DrawLine(myPen, 60, 375, 800, 375);

            e.Graphics.DrawString("Firma Adı          :", orta, sb, 60, 380); e.Graphics.DrawString(txtFirmaAd.Text, govde, sb, 200, 380);
            e.Graphics.DrawString("Firma Adresi       :", orta, sb, 60, 410); e.Graphics.DrawString(txtAdres.Text, govde, sb, 200, 410);
            e.Graphics.DrawString("Staj Sorumlusu     :", orta, sb, 60, 440); e.Graphics.DrawString(txtsorumlu.Text, govde, sb, 200, 440);
            e.Graphics.DrawString("Firma Telefon      :", orta, sb, 60, 470); e.Graphics.DrawString(txtisyeritel.Text, govde, sb, 200, 470);
            e.Graphics.DrawString("Staj Yapacağı Bölüm:", orta, sb, 60, 500); e.Graphics.DrawString(txtStajyb.Text, govde, sb, 280, 500);



            //.., .., uzunluk, düzlük 
            e.Graphics.DrawLine(myPen, 45, 550, 800, 550);
            e.Graphics.DrawString("     Yukarıda kimlik bilgileri yazılı öğrenci aşağıda belirtilen tarihler arasında iş yerimizde ", govde, sb, 60, 560);
            e.Graphics.DrawString("30 iş günü staj yapacaktır.", govde, sb, 60, 580);
            e.Graphics.DrawString("Öğrencinin staj süresi boyunca sigorta primleri Yüksekokulunuzca karşılanacaktır. Bu ;", govde, sb, 60, 600);
            e.Graphics.DrawString("nedenle;", govde, sb, 60, 620);
            e.Graphics.DrawString("1- Öğrencinin staja başlama tarihinin en az 1 (bir) hafta önce Yüksekokulunuz Öğrenci", govde, sb, 60, 640);
            e.Graphics.DrawString("İşleri bürosuna bildireceğimi,", govde, sb, 60, 660);
            e.Graphics.DrawString("2- Öğrencinin staja başlama ve bitiş tarihlerinin değişmesi halinde en az 1 (bir) hafta önce ", govde, sb, 60, 680);
            e.Graphics.DrawString("Yüksekokulunuz Öğrenci İşleri bürosuna bildireceiğimi", govde, sb, 60, 700);
            e.Graphics.DrawString("Öğrenci İşleri Bürosuna bildireceğimi,(0282 250 3400' dan Dahili 28-29-32) Taahhüt ederim.", govde, sb, 60, 720);
            e.Graphics.DrawString("Staja Başlama Tarihi:", orta, sb, 60, 740); e.Graphics.DrawString("İşyeri Kaşe", orta, sb, 560, 740);
            e.Graphics.DrawString("Staj Btiş Tarihi:", orta, sb, 60, 760);     e.Graphics.DrawString("Yekili İmza", orta, sb, 560, 760);
            //.., .., uzunluk, düzlük 
            e.Graphics.DrawLine(myPen, 45, 780, 800, 780);


            
            //.., .., uzunluk, düzlük 
            e.Graphics.DrawLine(myPen, 45, 550, 800, 550);
            e.Graphics.DrawString("     Yukarıdabelirtilen firmada ../../.. - ../../.. Tarihleri arasında 30 iş günü  ", govde, sb, 60, 800);
            e.Graphics.DrawString("stajımı yapacağım.", govde, sb, 60, 820);
            e.Graphics.DrawString("1- Staj başvuru formumu , staj kabul yazımı ve müstehaklık belgemi en geç ../../.. kadar  ", govde, sb, 60, 840);
            e.Graphics.DrawString(" öğrenci işleri bürosuna bildireceğimi,", govde, sb, 60, 860);
            e.Graphics.DrawString("2- Staja başlama ve bitiş tarihlerinin değişmesi halinde en az 1 (bir) hafta önce  ", govde, sb, 60, 880);
            e.Graphics.DrawString("Yüksekokulunuz Öğrenci İşleri bürosuna bildireceiğimi", govde, sb, 60, 900);
            e.Graphics.DrawString("Aksi halde 5510 sayılı kanunun doğacak cezai yülkümlülükleri kabul ettiğimi Taahhüt ederim.", govde, sb, 60, 920);
            e.Graphics.DrawString("Öğrenci İşleri Bürosuna teslim tarihi ../../..", govde, sb, 60, 940);
            e.Graphics.DrawString("Danışmanın:", orta, sb, 60, 960); e.Graphics.DrawString(txtad.Text + " "+txtsoyad.Text, orta, sb, 560, 960);
            e.Graphics.DrawString("(Unvanı-adı-soyadı-imzası)", orta, sb, 60, 980);     e.Graphics.DrawString("(imzası)", orta, sb, 560, 980);
            //.., .., uzunluk, düzlük 
            e.Graphics.DrawLine(myPen, 45, 780, 800, 780);



        }

        //
        //öğrencilerin datası
        private void gunaDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Data Grid View çökyordu başlığa tıklayınca çökmemesi için 
            if (e.RowIndex == -1) return;
            txtTc.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtad.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtsoyad.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtbabaAdı.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            dateTimeDogum.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtOgrencino.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtSınıf.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            txtCepTel.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            comboBoxBolum.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            comboBoxProgram.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();

            txtFirmaAd.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
            txtAdres.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
            txtsorumlu.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString();
            txtisyeritel.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString();
            txtStajyb.Text = gunaDataGridView1.Rows[e.RowIndex].Cells[14].Value.ToString();



        }

        //
        //formu temizle
        private void fomruTemizle()
        {
            txtTc.Clear();
            txtad.Clear();
            txtsoyad.Clear();
            txtbabaAdı.Clear();
            txtOgrencino.Clear();
            txtCepTel.Clear();
            txtSınıf.Clear();
            comboboxVarsayilan();
            txtFirmaAd.Clear();
            txtAdres.Clear();
            txtsorumlu.Clear();
            txtisyeritel.Clear();
            txtStajyb.Clear();

        }

        //
        //formu güncelle (YENİLE)
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            formuGoster();
            fomruTemizle();
        }
        //
        //kayıt sil metot
        private void kayitSil()
        {
            if (txtTc.Text != "")
            {
                baglantim.Close();// bağlantıyı kapatıp tekrar açınca çalıştı
                bool kayitAramaDurumu = false;
                baglantim.Open();
                OleDbCommand aramaSorgu = new OleDbCommand("select * from stajBasvuruFormu where Tcno='" + txtTc.Text + "'", baglantim);
                OleDbDataReader kayitokuma = aramaSorgu.ExecuteReader();

                while (kayitokuma.Read())
                {
                    kayitAramaDurumu = true;
                    OleDbCommand deleteSorgu = new OleDbCommand("delete from stajBasvuruFormu where Tcno='" + txtTc.Text + "'", baglantim);
                    deleteSorgu.ExecuteNonQuery();
                    MessageBox.Show("Öğrenci Kaydı Silindi.", "STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //baglantim.Close();
                    //aracGoster();
                    //aracEkleTemizle();
                    break;

                }

                if (kayitAramaDurumu == false)
                    MessageBox.Show("Silinecek Araç Kaydı Bulunamadı!!", "STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Error);

                baglantim.Close();
                formuGoster();
                fomruTemizle();
            }//if

            else
                MessageBox.Show("Tc no 11 karakterden oluşmalı!!", "STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        //
        //kayıt sil buton
        private void gunaButton2_Click(object sender, EventArgs e)
        {


            if (txtTc.Text.Length == 11)
            {
                DialogResult sil = new DialogResult();
                sil = MessageBox.Show("Öğrenci kaydını silmek istiyormusunuz ?", "Uyarı", MessageBoxButtons.YesNo);
                if (sil == DialogResult.Yes)
                {
                    kayitSil();
                }
                if (sil == DialogResult.No)
                {
                    MessageBox.Show("Öğrenci Kaydı silinmedi.");
                }
            }
            else
            {
                MessageBox.Show("Tc no 11 karakterden oluşmalı!!", "STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

           
        }
        //
        //kayit  güncelle metot
        private void kayitGuncelle()
        {
            try
            {
                baglantim.Open();
                OleDbCommand guncelle = new OleDbCommand("update stajBasvuruFormu set " +
                    "Adı='" + txtad.Text + "'," +
                    "Soyadı='" + txtsoyad.Text + "'," +
                    "BabaAdı ='" + txtbabaAdı.Text + "'," +
                    "DoğumTarihi='" + dateTimeDogum.Text + "'," +
                    "OgrenciNo='" + txtOgrencino.Text + "'," +
                    "Sınıfı='" + txtSınıf.Text + "'," +
                    "Telefon='" + txtCepTel.Text + "'," +
                    "Bölümü='" + comboBoxBolum.Text + "'," +
                    "Programı='" + comboBoxProgram.Text + "'" +
                    "where Tcno='" + txtTc.Text + "'", baglantim);
                guncelle.ExecuteNonQuery();//güneclle 
                baglantim.Close();
                formuGoster();
                fomruTemizle();
                MessageBox.Show("Kayıt başarılı bir şekilde değiştirildi.", "STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Kayıt oluşturulamadı HATA!. VOLKAN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }



        }
        //
        //KAYİT GÜNCELLE BUTON
        private void btnKayitGuncelle_Click(object sender, EventArgs e)
        {
            kayitGuncelle();
        }

        private void gunaPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //
        //tc txt
        private void gunaTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadece rakam girişi
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }
        private void txtad_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadace harf girişi
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private void txtsoyad_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadace harf girişi
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private void txtbabaAdı_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadace harf girişi
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private void txtOgrencino_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadece rakam girişi
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtSınıf_KeyPress(object sender, KeyPressEventArgs e)
        {
            //özel karakter engelleme
            if (e.KeyChar == '£' || e.KeyChar == '½' ||
              e.KeyChar == '€' || e.KeyChar == '₺' ||
              e.KeyChar == '¨' || e.KeyChar == 'æ' ||
              e.KeyChar == 'ß' || e.KeyChar == '´')
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar >= 33 && (int)e.KeyChar <= 47)
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar >= 58 && (int)e.KeyChar <= 64)
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar >= 91 && (int)e.KeyChar <= 96)
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar >= 123 && (int)e.KeyChar <= 127)
            {
                e.Handled = true;
            }
        }

        private void gunaLabel10_Click(object sender, EventArgs e)
        {

        }

        private void txtisyeritel_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadece rakam girişi
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtsorumlu_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadace harf girişi
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private void txtStajyb_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadace harf girişi
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private void txtFirmaAd_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadace harf girişi
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        //
        // öğrenci veya tc ile arama yağpıldğında 
        private void gunaButton1_Click(object sender, EventArgs e)
        {

            bool kayitAramaDurumu = false;
            if (txtTc.Text.Length == 11)
            {
               
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from stajBasvuruFormu where Tcno='" + txtTc.Text + "'", baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();

                while (kayitokuma.Read())
                {
                    kayitAramaDurumu = true;

                    txtTc.Text = kayitokuma.GetValue(0).ToString();
                    txtad.Text = kayitokuma.GetValue(1).ToString();
                    txtsoyad.Text = kayitokuma.GetValue(2).ToString();
                    txtbabaAdı.Text = kayitokuma.GetValue(3).ToString();
                    dateTimeDogum.Text = kayitokuma.GetValue(4).ToString();
                    txtOgrencino.Text = kayitokuma.GetValue(5).ToString();
                    txtSınıf.Text = kayitokuma.GetValue(6).ToString();
                    txtCepTel.Text = kayitokuma.GetValue(7).ToString();
                    comboBoxBolum.Text = kayitokuma.GetValue(8).ToString();
                    comboBoxProgram.Text = kayitokuma.GetValue(9).ToString();
                    txtFirmaAd.Text = kayitokuma.GetValue(10).ToString();
                    txtAdres.Text = kayitokuma.GetValue(11).ToString();
                    txtsorumlu.Text = kayitokuma.GetValue(12).ToString();
                    txtisyeritel.Text = kayitokuma.GetValue(13).ToString();
                    txtStajyb.Text = kayitokuma.GetValue(14).ToString();
                
                    break;

                }

                if (kayitAramaDurumu == false)
                    MessageBox.Show("Kullanıcı Kaydı BULUNAMADI !!.", "STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Information);

                baglantim.Close();
            }
            //if
            else
                MessageBox.Show("Aramak isteiğiniz kaydın 11 karakterden oluşan Tc sini girin.!!", "STAJ TAKİP PROGRAMI", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void txtTc_TabStopChanged(object sender, EventArgs e)
        {
             
        }


        //text boxk içine yazma

        //private void textBox1_Enter(object sender, EventArgs e)
        //{
        //    if (textBox1.Text == "Kullanıcı Adı")
        //    {
        //        textBox1.Clear();
        //        textBox1.ForeColor = Color.Black;

        //    }
        //}

        //private void textBox1_Leave(object sender, EventArgs e)
        //{
        //    if (textBox1.Text != "Kullanıcı Adı")
        //    {
        //        textBox1.Text = "Kullanıcı Adı";
        //        textBox1.ForeColor = Color.Gray; textBox1.Clear();
        //    }
        //}
    }
}

