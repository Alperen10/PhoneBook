using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneBook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        static AppData db;
        protected static AppData App
        {
            get
            {
                if (db==null)
                {
                    db = new AppData();

                }
                return db;

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string fileName = string.Format("{0}//data.dat",Application.StartupPath);
            if (File.Exists(fileName))
            {
                App.TelefonDefteri.ReadXml(fileName);
                telefonDefteriBindingSource.DataSource = App.TelefonDefteri;
                panel1.Enabled = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                telefonDefteriBindingSource.EndEdit();
                App.TelefonDefteri.AcceptChanges();
                App.TelefonDefteri.WriteXml(string.Format("{0}//data.dat", Application.StartupPath));
                panel1.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                App.TelefonDefteri.RejectChanges();
            }


        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            tbxNumber.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            telefonDefteriBindingSource.ResetBindings(false);
            panel1.Enabled = false;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                panel1.Enabled = true;
                App.TelefonDefteri.AddTelefonDefteriRow(App.TelefonDefteri.NewTelefonDefteriRow());
                telefonDefteriBindingSource.MoveLast();
                tbxNumber.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                App.TelefonDefteri.RejectChanges();
            }
           
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Bu kaydı silmek istediğinize emin misiniz?","Message",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)

                {
                    telefonDefteriBindingSource.RemoveCurrent();

                }
            }

        }

        private void tbxSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {


                if (!string.IsNullOrEmpty(tbxSearch.Text))
                {
                    var query = from o in App.TelefonDefteri
                                where o.Telefon_Numarası == tbxSearch.Text || o._Ad_Soyad.Contains(tbxSearch.Text)
                                || o.Email == tbxSearch.Text
                                select o;
                    dataGridView1.DataSource = query.ToList();
                }
                else
                {
                    dataGridView1.DataSource = telefonDefteriBindingSource;
                }
            }
    


        }
    }
}
