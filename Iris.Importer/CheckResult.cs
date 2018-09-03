using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Iris.Importer
{
    public class CheckResult
    {
        public Employee Employee { get; set; }
        public List<ValidationMessage> Warnings { get; set; }
        public List<ValidationMessage> Errors { get; set; }

        public CheckResult()
        {
            this.Employee = new Employee();
            this.Employee.Duties = new List<AssignedDuty>() { new AssignedDuty() };
            this.Employee.ContactInfo = new ContactInfo();
            this.Warnings = new List<ValidationMessage>();
            this.Errors = new List<ValidationMessage>();
        }

        public void AddMessage(ValidationMessage message)
        {
            if (message.Type == ValidationType.Error)
                this.Errors.Add(message);
            else if (message.Type == ValidationType.Warning)
                this.Warnings.Add(message);
        }

        public void WriteLine(RichTextBox box, int lineIndex = 0)
        {
            int start = box.TextLength;
            string text = string.Format("[{0}] {1} -> ", lineIndex == 0 ? box.Lines.Count() : lineIndex, this.Employee.Title);
            box.AppendText(text);
            box.Select(start, text.Length);
            box.SelectionFont = new Font(box.Font, FontStyle.Bold);
            if (this.Errors.Any())
                box.SelectionColor = Color.Red;
            else if (this.Warnings.Any())
                box.SelectionColor = Color.Yellow;
            else
                box.SelectionColor = Color.Green;
            box.SelectionLength = 0; // clear
            box.SelectionFont = new Font(box.Font, FontStyle.Regular);

            start = box.TextLength;
            var errors = string.Join(", ", this.Errors.Select(x => x.Message));           
            box.AppendText(errors);
            box.Select(start, errors.Length);
            box.SelectionColor = Color.Red;
            box.SelectionLength = 0; // clear

            if(errors.Length > 0)
                box.AppendText(" ");
            start = box.TextLength;
            var warnings = string.Join(",", this.Warnings.Select(x => x.Message));
            box.AppendText(warnings);
            box.Select(start, warnings.Length);
            box.SelectionColor = Color.Yellow;
            box.SelectionLength = 0; // clear

            box.AppendText(Environment.NewLine);
        }
    }
}
