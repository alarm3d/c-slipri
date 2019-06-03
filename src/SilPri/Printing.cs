using RawPrint;

namespace SilPri
{
    public class Printing
    {

        public bool printPDF(string PrinterName, string Filepath, string Filename)
        {

            IPrinter printer = new Printer();
            try
            {
                printer.PrintRawFile(PrinterName, Filepath, Filename);
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
