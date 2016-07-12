using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CEPsBrasil.Factory;
using CEPsBrasil.Model;

namespace CEPsBrasil.Application
{
    public class EnderecoApplication
    {
        public Endereco Pesquisar(string cep)
        {
            try
            {
                //PRIMEIRO TRATAMOS O PARAMETRO
                cep = TratarParametro(cep);

                //BUSCAMOS AS INFROMAÇÕES NO SITE DOS CORREIOS
                var correiosHTML = RequestCorreios(cep);

                //TRATAMOS O HTML E RETORNAMOS O OBJETO
                return MapperHTML(correiosHTML);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string TratarParametro(string cep)
        {
            //SE ESTIVER EM BRANCO JA SAIMOS
            if(string.IsNullOrEmpty(cep))
                throw new Exception("Não é possível fazer uma consulta de um CEP em branco.");

            //SE ESTIVER COM MASCARA, REMOVEMOS
            cep = cep.Replace("-", string.Empty);

            //VERIFICAMOS SE O CEP TEM O TAMANHO CORRETO
            if(cep.Length != 8)
                throw new Exception("O CEP não está em um formato válido.");

            return cep;
        }

        private string RequestCorreios(string cep)
        {
            //FAZEMOS A REQUISIÇÃO
            var request = WebRequest.Create("http://m.correios.com.br/movel/buscaCepConfirma.do");
            request.Method = "POST";
            
            var postData = "cepEntrada=" + cep + "&tipoCep=&cepTemp=&metodo=buscarCep";
            var byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            
            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();


            //RETORNAMOS O HTML
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();

            if(responseStream == null)
                throw new Exception("Não foi possível recuperar as informações do site dos Correios.");

            return new StreamReader(stream: responseStream, encoding: Encoding.GetEncoding("ISO-8859-1")).ReadToEnd();
        }

        private Endereco MapperHTML(string html)
        {
            const string reg = "<span class=\"respostadestaque\">(.*?)</span><br/>";
            var result = Regex.Matches(html, reg, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            string resultCidade;

            var endereco = new EnderecoFactory().NovoEndereco();

            switch (result.Count)
            {

                case 0:
                    throw new Exception("O CEP informado não foi encontrado.");
                case 2:
                    endereco.CEP = result[1].Groups[1].Value;
                    resultCidade = result[0].Groups[1].Value.Replace("\n", string.Empty).Replace("\t", string.Empty).Trim();
                    endereco.Cidade.Nome = resultCidade.Split('/')[0].Trim();
                    endereco.Estado.UF = resultCidade.Split('/')[1].Trim();
                    endereco.CEPCidade = true;
                    break;
                case 4:
                case 5:
                    endereco.CEP = result[3].Groups[1].Value;
                    var logradouro = result[0].Groups[1].Value.Replace("\n", string.Empty).Trim();
                    endereco.Logradouro = logradouro.IndexOf('-') > 0 ? logradouro.Substring(0, logradouro.IndexOf('-') - 1) : logradouro;
                    endereco.Bairro = result[1].Groups[1].Value.Trim();
                    resultCidade = result[2].Groups[1].Value.Replace("\n", string.Empty).Replace("\t", string.Empty).Trim();
                    endereco.Cidade.Nome = resultCidade.Split('/')[0].Trim();
                    endereco.Estado.UF = resultCidade.Split('/')[1].Trim();
                    endereco.CEPCidade = false;
                    
                    if (endereco.Estado.UF == "SP" && endereco.Cidade.Nome == "São Paulo")
                        endereco.Zona = VerificaZonaSp(endereco.CEP);
                    else
                        endereco.Zona = null;

                    break;
                default:
                    throw new Exception("O CEP informado não foi encontrado.");
            }

            return endereco;
        }

        public Zona? VerificaZonaSp(string numeroCEP)
        {
            try
            {
                var numero = Convert.ToInt64(numeroCEP.Replace("-", string.Empty).Substring(0, 5));

                //CENTRO
                if (numero >= 01000 && numero <= 01099)
                    return Zona.Centro;
                else if (numero >= 01100 && numero <= 01199)
                    return Zona.Centro;
                else if (numero >= 01200 && numero <= 01299)
                    return Zona.Centro;
                else if (numero >= 01300 && numero <= 01399)
                    return Zona.Centro;
                else if (numero >= 01400 && numero <= 01499)
                    return Zona.Centro;
                else if (numero >= 01500 && numero <= 01599)
                    return Zona.Centro;
                //NORTE
                else if (numero >= 02000 && numero <= 02099)
                    return Zona.Norte;
                else if (numero >= 02100 && numero <= 02199)
                    return Zona.Norte;
                else if (numero >= 02200 && numero <= 02299)
                    return Zona.Norte;
                else if (numero >= 02300 && numero <= 02399)
                    return Zona.Norte;
                else if (numero >= 02400 && numero <= 02499)
                    return Zona.Norte;
                else if (numero >= 02500 && numero <= 02599)
                    return Zona.Norte;
                else if (numero >= 02600 && numero <= 02699)
                    return Zona.Norte;
                else if (numero >= 02700 && numero <= 02799)
                    return Zona.Norte;
                else if (numero >= 02800 && numero <= 02899)
                    return Zona.Norte;
                else if (numero >= 02900 && numero <= 02999)
                    return Zona.Norte;
                //ZONA LESTE
                else if (numero >= 03000 && numero <= 03099)
                    return Zona.Leste;
                else if (numero >= 03100 && numero <= 03199)
                    return Zona.Leste;
                else if (numero >= 03200 && numero <= 03299)
                    return Zona.Leste;
                else if (numero >= 03300 && numero <= 03399)
                    return Zona.Leste;
                else if (numero >= 03400 && numero <= 03499)
                    return Zona.Leste;
                else if (numero >= 03500 && numero <= 03599)
                    return Zona.Leste;
                else if (numero >= 03600 && numero <= 03699)
                    return Zona.Leste;
                else if (numero >= 03700 && numero <= 03799)
                    return Zona.Leste;
                else if (numero >= 03800 && numero <= 03899)
                    return Zona.Leste;
                else if (numero >= 03900 && numero <= 03999)
                    return Zona.Leste;
                else if (numero >= 08000 && numero <= 08099)
                    return Zona.Leste;
                else if (numero >= 08100 && numero <= 08199)
                    return Zona.Leste;
                else if (numero >= 08200 && numero <= 08299)
                    return Zona.Leste;
                else if (numero >= 08300 && numero <= 08399)
                    return Zona.Leste;
                else if (numero >= 08400 && numero <= 08499)
                    return Zona.Leste;
                //ZONA SUL
                else if (numero >= 04000 && numero <= 04099)
                    return Zona.Sul;
                else if (numero >= 04100 && numero <= 04199)
                    return Zona.Sul;
                else if (numero >= 04200 && numero <= 04299)
                    return Zona.Sul;
                else if (numero >= 04300 && numero <= 04399)
                    return Zona.Sul;
                else if (numero >= 04400 && numero <= 04499)
                    return Zona.Sul;
                else if (numero >= 04500 && numero <= 04599)
                    return Zona.Sul;
                else if (numero >= 04600 && numero <= 04699)
                    return Zona.Sul;
                else if (numero >= 04700 && numero <= 04799)
                    return Zona.Sul;
                else if (numero >= 04800 && numero <= 04899)
                    return Zona.Sul;
                else if (numero >= 04900 && numero <= 04999)
                    return Zona.Sul;
                //ZONA OESTE
                else if (numero >= 05000 && numero <= 05099)
                    return Zona.Oeste;
                else if (numero >= 05100 && numero <= 05199)
                    return Zona.Oeste;
                else if (numero >= 05200 && numero <= 05299)
                    return Zona.Oeste;
                else if (numero >= 05300 && numero <= 05399)
                    return Zona.Oeste;
                else if (numero >= 05400 && numero <= 05499)
                    return Zona.Oeste;
                else if (numero >= 05500 && numero <= 05599)
                    return Zona.Oeste;
                else if (numero >= 05600 && numero <= 05699)
                    return Zona.Oeste;
                else if (numero >= 05700 && numero <= 05799)
                    return Zona.Oeste;
                else if (numero >= 05800 && numero <= 05899)
                    return Zona.Oeste;
                else
                    return null;

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
