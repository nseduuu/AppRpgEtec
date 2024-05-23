using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Models
{
    internal class Disputa
    {
        public int Id { get; set; }
        public DateTime? DataDisputa { get; set; }
        public int AtacanteId { get; set; }
        public int OponeteId { get; set; }
        public string Narracao { get; set; }
        public int HabilidadeId { get; set; }
        public List<int> ListaIdPersonagem { get; set; }
        public List<string> Resultados { get; set; }
    }
}
