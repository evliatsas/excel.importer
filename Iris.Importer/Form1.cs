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

using OfficeOpenXml;

namespace Iris.Importer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dlg = openFileDialog1.ShowDialog();
            if(dlg == DialogResult.OK)
            {
                var fi = new FileInfo(openFileDialog1.FileName);
                this.textBox1.Text = openFileDialog1.FileName;
                this.flowLayoutPanel1.Controls.Clear();
                this.richTextBox1.Clear();

                using (var p = new ExcelPackage(fi))
                {
                    var workSheet = p.Workbook.Worksheets.First();
                    // check if ignoring first row
                    int offset = checkBox1.Checked ? 1 : 0;
                    var start = workSheet.Dimension.Start;
                    var end = workSheet.Dimension.End;

                    var startRow = start.Row + offset;

                    for (int col = start.Column; col <= end.Column; col++)
                    {
                        string cellValue;
                        if (offset == 1)
                            cellValue = workSheet.Cells[start.Row, col].Text;
                        else
                            cellValue = string.Format("Στήλη {0}", col + 1);

                        var header = new HeaderSelector();
                        header.Data = new HeaderData() { Column = cellValue };
                        this.flowLayoutPanel1.Controls.Add(header);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fi = new FileInfo(openFileDialog1.FileName);
            var validator = new EmployeeValidator();
            using (var p = new ExcelPackage(fi))
            {
                var workSheet = p.Workbook.Worksheets.First();
                // check if ignoring first row
                int offset = checkBox1.Checked ? 1 : 0;
                var start = workSheet.Dimension.Start;
                var end = workSheet.Dimension.End;

                var startRow = start.Row + offset;

                for (int row = startRow; row <= end.Row; row++)
                {                   
                    var result = new CheckResult();

                    for (int col = start.Column; col <= end.Column; col++)
                    {
                        string cellValue = workSheet.Cells[row, col].Text;

                        var header = this.flowLayoutPanel1.Controls[col - 1] as HeaderSelector;
                        if (header.Data.IsChecked)
                        {
                            switch (header.Data.PropertyIndex)
                            {
                                case 0:
                                    result.AddMessage(validator.CheckLastName(cellValue));
                                    result.Employee.LastName = cellValue;
                                    break;
                                case 1:
                                    result.AddMessage(validator.CheckFirstName(cellValue));
                                    result.Employee.FirstName = cellValue;
                                    break;
                                case 2:
                                    result.AddMessage(validator.CheckFatherFirstName(cellValue));
                                    result.Employee.FatherFirstName = cellValue;
                                    break;
                                case 3:
                                    result.AddMessage(validator.CheckGender(cellValue));
                                    result.Employee.Gender = cellValue;
                                    break;
                                case 4:
                                    result.AddMessage(validator.CheckAddress(cellValue));
                                    result.Employee.ContactInfo.Address = cellValue;
                                    break;
                            }
                        }
                    }

                    result.WriteLine(this.richTextBox1);
                }
            }
        }               
    }
}
