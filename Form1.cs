using Microsoft.Office.Interop.PowerPoint;
using Applications = Microsoft.Office.Interop.PowerPoint.Application;
namespace Powerpoint_to_pdf
{
    public partial class Form1 : Form
    {
        public Form1()  
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string folderPath;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                button1.Hide();
                folderPath = folderBrowserDialog1.SelectedPath;
                // Call method to convert .doc files to .pdf files and delete original .doc files
                ConvertPptsToPdfs(folderPath);

                MessageBox.Show("Conversion complete!");
                System.Windows.Forms.Application.Exit();
            }
        }
        void ConvertPptsToPdfs(string folderPath)
        {

            // Get all .doc files in folder and its subfolders
            string[] pptFiles = Directory.GetFiles(folderPath, "*.ppt*", SearchOption.AllDirectories);

            // Create Microsoft Word application object
            Applications powerpoint = new Applications();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = pptFiles.Length;
            progressBar1.Value = 0;
            foreach (string pptFile in pptFiles)
            {
                // Open the .doc file
                progressBar1.Value++;
                Presentation presentation = powerpoint.Presentations.Open(pptFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
                string name = pptFile.Substring(folderPath.Length+1);
                label2.Text+=$"Converting {name} to PDF...\n";
                // Get the PDF filename
                string pdfFile = Path.ChangeExtension(pptFile, ".pdf");

                // Save the document as a PDF file
                presentation.ExportAsFixedFormat(pdfFile, PpFixedFormatType.ppFixedFormatTypePDF);

                // Close the document
                presentation.Close();

                // Delete the original .doc file
                File.Delete(pptFile);
            }

            // Quit Microsoft Word application
            powerpoint.Quit();
            Console.WriteLine(); // Move to next line after progress bar
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
    }
}
