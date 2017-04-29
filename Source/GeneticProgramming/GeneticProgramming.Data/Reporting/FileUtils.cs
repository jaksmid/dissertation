using System.IO;

namespace GeneticProgramming.Data.Reporting
{
    public class FileUtils
    {
        public static void WriteAsFile(string toWrite, string fileName, string fileFormat="json")
        {
            string output = fileFormat == null ? fileName : fileName + "." + fileFormat;
            var writer = new StreamWriter(output);
            writer.Write(toWrite);
            writer.Flush();
            writer.Close();
        }
    }
}
