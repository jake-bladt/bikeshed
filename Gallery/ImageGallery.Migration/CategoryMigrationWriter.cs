using System;
using System.IO;

namespace Gallery.Migration
{
    public class CategoryMigrationWriter : IDisposable
    {

        protected String _filePath;
        protected StreamWriter _writer;

        public CategoryMigrationWriter(String path)
        {
            _filePath = path;
            _writer = new StreamWriter(_filePath, false);
        }

        public void Dispose()
        {
            try
            {
                _writer.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void WriteExportLine(String line)
        {
            _writer.WriteLine(line);
        }

    }
}
