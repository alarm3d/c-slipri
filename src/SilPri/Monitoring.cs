using System.IO;

namespace SilPri
{
    public class Monitoring
    {
        private FileSystemWatcher watcher;
        public Monitoring()
        {
            watcher = new FileSystemWatcher();
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite;
            watcher.Filter = "*.pdf";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
        }

        public void Start(string path)
        {
            watcher.Path = path;
            watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;

        }
        
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string printer = "Microsoft Print to PDF";
            string fullPath = e.FullPath;
            string name = e.Name;
            var print = new Printing();
            
            if (print.printPDF(printer, fullPath, name))
            { 
                File.Delete(e.FullPath);
            }
        }
    }
}
