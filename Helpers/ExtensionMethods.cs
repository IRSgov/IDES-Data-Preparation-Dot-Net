using System;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public static class ExtensionMethods
    {
        public static string ShowDialogWithFilter(this OpenFileDialog dialog, string filter)
        {
            dialog.Filter = filter;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }

            return string.Empty;
        }

        public static void DisplayException(this Exception ex, string messageBoxTitle)
        {
            string errorMessage = ex.GetBaseException().Message;

            if (errorMessage.Contains("password"))
            {
                // certificate password is incorrect
                MessageBox.Show("Specified certificate password is incorrect!", messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // other error
            MessageBox.Show(errorMessage, messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }



    }
}
