using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Iris.Importer
{
    public partial class Form1 : Form
    {
        private List<CheckResult> _checkResults;
        public Form1()
        {
            InitializeComponent();

            _checkResults = new List<CheckResult>();
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
            _checkResults.Clear();
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
                                case 5:
                                    result.AddMessage(validator.CheckCity(cellValue));
                                    result.Employee.ContactInfo.City = cellValue;
                                    break;
                                case 6:
                                    result.AddMessage(validator.CheckTK(cellValue));
                                    result.Employee.ContactInfo.TK = cellValue;
                                    break;
                                case 7:
                                    result.AddMessage(validator.CheckPhone(cellValue));
                                    result.Employee.ContactInfo.PhoneNo = cellValue;
                                    break;
                                case 8:
                                    result.AddMessage(validator.CheckMobilePhone(cellValue));
                                    result.Employee.ContactInfo.MobilePhoneNo = cellValue;
                                    break;
                                case 9:
                                    result.AddMessage(validator.CheckEmail(cellValue));
                                    result.Employee.ContactInfo.EMail = cellValue;
                                    break;
                                case 10:
                                    EmployeeCategory category;
                                    result.AddMessage(validator.CheckCategory(cellValue, out category));
                                    result.Employee.Category = category;
                                    break;
                                case 11:
                                    Lookup<int> rank;
                                    result.AddMessage(validator.CheckRank(cellValue, out rank));
                                    result.Employee.Rank = rank;
                                    break;
                                case 12:
                                    Lookup<int> speciality;
                                    result.AddMessage(validator.CheckSpeciality(cellValue, out speciality));
                                    result.Employee.Speciality = speciality;
                                    break;
                                case 13:
                                    Lookup<int> oType;
                                    result.AddMessage(validator.CheckOccupationType(cellValue, out oType));
                                    result.Employee.OccupationType = oType;
                                    break;
                                case 14:
                                    Lookup<int> position;
                                    result.AddMessage(validator.CheckPosition(cellValue, out position));
                                    result.Employee.Position = position;
                                    break;
                                case 15:
                                    Lookup<int> unit;
                                    result.AddMessage(validator.CheckPosition(cellValue, out unit));
                                    result.Employee.Unit = unit;
                                    break;
                                case 16:
                                    Lookup<int> duty;
                                    result.AddMessage(validator.CheckDuty(cellValue, out duty));
                                    result.Employee.Duties.First().Duty = duty;
                                    break;
                                case 17:
                                    Lookup<int> dutyPosition;
                                    result.AddMessage(validator.CheckPosition(cellValue, out dutyPosition));
                                    result.Employee.Duties.First().Position = dutyPosition;
                                    break;
                                case 18:
                                    var pwd = string.IsNullOrEmpty(cellValue) ? "12345678" : cellValue;
                                    result.Employee.Tag = pwd;
                                    break;
                            }
                        }
                    }

                    result.WriteLine(this.richTextBox1, row);
                    _checkResults.Add(result);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (var i=0;i<this.flowLayoutPanel1.Controls.Count;i++)
            {
                var header = this.flowLayoutPanel1.Controls[i] as HeaderSelector;
                header.SetIndex(i);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(_checkResults.Count == 0)
            {
                MessageBox.Show("Δεν έχει πραγματοποιηθεί έλεγχος των δεδομένων.", "Διαχείριση", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_checkResults.Any(a=>a.Errors.Count > 0))
            {
                MessageBox.Show("O έλεγχος των δεδομένων έχει επιστρέψει σφάλματα. Παρακαλώ διορθώστε τα πρώτα.", "Διαχείριση", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var importer = new EmployeeImporter();
            var employees = _checkResults.Select(x => x.Employee);
            importer.Import(employees);
        }
    }
}
