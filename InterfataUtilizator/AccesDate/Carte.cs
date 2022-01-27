using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfataUtilizator.AccesDate
{
    class Carte
    {
        //IDCarte,Titlu,ISBN,An,Limba,tip, Cantit, IDEdit
        public int IDCarte { get; set; }
        public string Titlu { get; set; }
        public string ISBN { get; set; }
        public int An { get; set; }
        public string Limba { get; set; }
        public string Tip { get; set; }
        public int Cantit { get; set; }
        public int IDEdit { get; set; }
    }
}
