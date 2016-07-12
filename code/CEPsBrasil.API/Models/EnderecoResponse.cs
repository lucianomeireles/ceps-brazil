using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CEPsBrasil.Model;

namespace CEPsBrasil.API.Models
{
    public class EnderecoResponse
    {
        public bool Sucesso { get; set; }

        public string Mensagem { get; set; }

        public Endereco Endereco { get; set; }
    }
}