using System;
using System.Windows.Forms;

namespace Iris.Importer
{
    public partial class HeaderSelector : UserControl
    {
        public HeaderData Data
        {
            get
            {
                return new HeaderData()
                {
                    Column = checkBox1.Text,
                    IsChecked = checkBox1.Checked,
                    Property = this.comboBox1.Text,
                    PropertyIndex = this.comboBox1.SelectedIndex
                };
            }
            set
            {
                this.checkBox1.Text = value.Column;
                this.checkBox1.Checked = value.IsChecked;
                this.comboBox1.SelectedIndex = -1;
                this.comboBox1.Text = string.Empty;
            }
        }

        public HeaderSelector()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.checkBox1.Checked = this.comboBox1.SelectedIndex != -1;
        }

        public void SetIndex(int index)
        {
            this.comboBox1.SelectedIndex = index;
        }
    }

    public class HeaderData
    {
        public string Column { get; set; }
        public bool IsChecked { get; set; }
        public string Property { get; set; }
        public int PropertyIndex { get; set; }
    }
}
