using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CEPsBrasil.Model
{
    public class Endereco
    {
        public string CEP { get; set; }

        public string Logradouro { get; set; }

        public string Bairro { get; set; }

        public Zona? Zona { get; set; }

        public bool CEPCidade { get; set; }

        public Cidade Cidade { get; set; }

        public Estado Estado { get; set; }
    }
}
