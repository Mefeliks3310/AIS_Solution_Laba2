using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Logic Logic { get; set; }
        public Form2(Dictionary<string,int> pairs)
        {
            Logic = new Logic();
            InitializeComponent();
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.Title = "Специальности";
            chart1.ChartAreas[0].AxisY.Title = "Количество студентов";
            Random random = new Random();
            int id = 1;
            foreach (var pair in pairs)
            {
                Series series = new Series(pair.Key);
                series.ChartType = SeriesChartType.Column;
                series.Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                series.Points.AddXY(id, pair.Value);
                chart1.Series.Add(series);
                id++;
            }
        }

        
       
        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
