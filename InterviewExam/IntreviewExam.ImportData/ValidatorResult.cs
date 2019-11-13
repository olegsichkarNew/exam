using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntreviewExam.ImportData
{
    public class ValidatorResult
    {
        private readonly StringBuilder messageBuilder = new StringBuilder();
        public bool Valid { get; set; }

        public ValidationResult ValidationType { get; set; }
        public string Message
        {
            get { return messageBuilder.ToString(); }
            set { messageBuilder.Append(value); }
        }
        public void AddNewErrorMessage(string message)
        {
            AddMessageContent(message, ValidationResult.Error);
        }
        public void AddNewErrorMessage(string message, ValidatedFields field)
        {
            AddMessageContent(message, ValidationResult.Error);
        }
        private void AddMessageContent(string message, ValidationResult result)
        {
            messageBuilder.Append(message);

            Valid = false;
            if (ValidationType == ValidationResult.None)
                ValidationType = result;
        }
    }

}
