using System.IO;

namespace FileConverter.Watcher
{
    public class DirectoryWatcher : WatcherBase
    {
        private FileSystemWatcher watcher;

        public DirectoryWatcher(string dir)
        {
            watcher = new FileSystemWatcher(dir);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = false;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            base.RaiseFileChangedEvent(e.FullPath);
        }
    }
}
