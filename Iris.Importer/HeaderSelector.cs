using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }

    public class HeaderData
    {
        public string Column { get; set; }
        public bool IsChecked { get; set; }
        public string Property { get; set; }
        public int PropertyIndex { get; set; }
    }
}
