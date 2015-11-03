namespace FileConverter.Watcher
{
    public abstract class WatcherBase
    {
        public delegate void FileChangedDelegate(string fileName);

        public event FileChangedDelegate FileChanged;

        protected void RaiseFileChangedEvent(string fileName)
        {
            if(FileChanged != null) { FileChanged(fileName); }
        }
    }
}
