using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Bilbliotecas.sincronizador
{
    public abstract class Sincronizador
    {

        private FileSystemWatcher watcher;
        public string path;



        public Sincronizador(string path)
        {
            this.path = path;
            this.iniciaEscuta();
        }

        /// <summary>
        /// Inicia escuta nas pastas
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void iniciaEscuta()
        {
            if (this.watcher == null)
            {
                // Create a new FileSystemWatcher and set its properties.
                this.watcher = new FileSystemWatcher();
                this.watcher.Path = @path;
                /* Watch for changes in LastAccess and LastWrite times, and
                   the renaming of files or directories. */
                this.watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                // Only watch xml files.
                this.watcher.Filter = "*.xml";
                this.watcher.IncludeSubdirectories = true;
                // Add event handlers.
                this.watcher.Changed += new FileSystemEventHandler(OnChanged);
                this.watcher.Created += new FileSystemEventHandler(OnChanged);
                this.watcher.Deleted += new FileSystemEventHandler(OnChanged);

                // Begin watching.
                this.watcher.EnableRaisingEvents = true;
            }
            else
            {
                this.watcher.EnableRaisingEvents = true;
            }
        }

        public void pausarEscuta()
        {
            this.watcher.EnableRaisingEvents = false;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            WatcherChangeTypes wct = e.ChangeType;
            switch (wct.ToString())
            {
                case "Created":
                    novoArquivo(e.FullPath);
                    break;
                case "Changed":
                    arquivoAlterado(e.FullPath);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// metodo que será executado caso a escuta capture a alteracao arquivo na pasta
        /// </summary>
        public abstract void arquivoAlterado(string path_file);


        /// <summary>
        /// metodo que será executado caso a escuta capture um novo arquivo na pasta
        /// </summary>
        public abstract void novoArquivo(string path_file);
    }
}
