using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Logic logic = new Logic();

        public Logic Logic {  get; set; }
        string name;
        string group;
        string speciality;
        public Form1()
        {
            InitializeComponent();
            Logic = new Logic();
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e) // Текстовое поле с именем
        {
            speciality = Scpeciality.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e) // Текстовое поле с группой
        {
        group = Group.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) // Текстовое поле с специальностью
        {
            name = TextBox.Text;
        }

        private void button1_Click(object sender, EventArgs e) // Кнопка добавить
        {
            string output = "";
            try
            {
                output = Logic.AddStudent(name, group, speciality);
            }
            finally
            {
                Interface.Text = output;
            }
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Interface_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                TextBox.Text = listView1.SelectedItems[0].SubItems[1].Text;
                Group.Text = listView1.SelectedItems[0].SubItems[2].Text;
                Scpeciality.Text = listView1.SelectedItems[0].SubItems[3].Text;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetID form3 = new GetID();
            if(form3.ShowDialog()==DialogResult.OK)
            {
                int id = form3.IDValue;
                Interface.Text = Logic.DeleteObject(id);
                DataOutput();
            }   
        }
        /// <summary>
        /// Добавляет объекты класса в ListView
        /// </summary>

        private void button1_Click_1(object sender, EventArgs e)
        {
            Interface.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetID form3 = new GetID();
            if (form3.ShowDialog() == DialogResult.OK) 
            {
                int id = form3.IDValue;
                Interface.Text = "Измените данные студента.";
                Interface.Text = Logic.UpdateObject(Scpeciality.Text, Group.Text, TextBox.Text, id);
                DataOutput();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(Logic.Gistogramma());
            form2.Show();
        }
        /// <summary>
        /// Функция по загрузке данных в ListView
        /// </summary>
        private void Upload_DB_Click(object sender, EventArgs e)
        {
            DataOutput();
        }
        private void FindByID_Click(object sender, EventArgs e)
        {
            GetID form3 = new GetID();
            if(form3.ShowDialog() == DialogResult.OK)
            {
                int id = form3.IDValue;
                Interface.Text = TextOfId(id);
            }
        }
        /// <summary>
        /// Функция по получения записей Student из базы данных
        /// </summary>
        public void DataOutput() 
        {
            listView1.Items.Clear();
            try
            {
                IEnumerable<IEnumerable<object>> students = Logic.GetSutednts();

                foreach (var student in students)
                {
                    ListViewItem listViewItem = null;

                    int fieldIndex = 0;
                    foreach (var field in student)
                    {
                        if (fieldIndex == 0)
                        {
                            listViewItem = new ListViewItem(field.ToString());
                        }
                        else if (listViewItem != null)
                        {
                            listViewItem.SubItems.Add(field.ToString());
                        }
                        fieldIndex++;
                    }

                    if (listViewItem != null)
                    {
                        listView1.Items.Add(listViewItem);
                    }
                }

                Scpeciality.Clear();
                Group.Clear();
                TextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "OK", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Функция по получения записи об одном студенте исходя из введеного ID
        /// </summary>
        /// <param name="id">ID студента</param>
        /// <returns></returns>
        public string TextOfId(int id)
        {
            string text = "";
            foreach(var objects in logic.GetByID(id))
            {
                text += objects + " ";
            }
            return text;

        }
    }
}
