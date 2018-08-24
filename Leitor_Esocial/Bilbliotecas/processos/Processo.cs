using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Bilbliotecas.processos
{
    public abstract class Processo
    {
        public string Nome_processo { get; private set; }
        public int Intervalo_minutos { get; private set; }
        public Thread Thread { get; private set; }
        public NotifyIcon Icone_notificao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnpj">Cnpj do usuário para identificação do Processo</param>
        /// <param name="user">User com todas as informações provindas do WebService</param>
        /// <param name="intervalo">Intervalo em minutos</param>
        public Processo(string nome_processo, int intervalo, NotifyIcon icone_notificacao)
        {
            this.Nome_processo = nome_processo;
            this.Icone_notificao = icone_notificacao;
            this.Intervalo_minutos = intervalo * 1000 * 60;
            Thread = new Thread(run);
        }

        // Metodos thread
        public void Start() => this.Thread.Start();
        public void Join() => this.Thread.Join();
        public bool IsAlive => this.Thread.IsAlive;
        // Override
        public abstract void run();
    }
}
