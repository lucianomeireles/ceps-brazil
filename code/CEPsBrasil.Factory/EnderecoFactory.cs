using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CEPsBrasil.Model;

namespace CEPsBrasil.Factory
{
    public class EnderecoFactory
    {
        public Endereco NovoEndereco()
        {
            return new Endereco
            {
                Cidade = new Cidade(),
                Estado = new Estado()
            };
        }
    }
}
