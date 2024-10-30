using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class GetID : Form
    {
        public int IDValue { get; private set; } // Это свойство будет хранить введенный ID
       

        public GetID()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int id;
            
                if ((int.TryParse(ID.Text, out id)))
                {
                        IDValue = id;
                        this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Ошибка. Введите корректный ID");
                }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void GetID_Load(object sender, EventArgs e)
        {

        }

        private void ID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
