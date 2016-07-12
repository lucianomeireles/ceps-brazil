using CEPsBrasil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;
using CEPsBrasil.Application;
using CEPsBrasil.API.Models;
using WebApi.OutputCache.V2;

namespace CEPsBrasil.API.Controllers
{
    [System.Web.Http.RoutePrefix("enderecos")]
    public class EnderecosController : ApiController
    {
        [System.Web.Http.HttpGet]
        //[OutputCache(Duration = 86400, VaryByParam = "cep", Location = OutputCacheLocation.ServerAndClient)]
        [CacheOutput(ServerTimeSpan = 120)]
        [System.Web.Http.Route("pesquisar/{cep}")]
        public EnderecoResponse Pesquisar(string cep)
        {
            var response = new EnderecoResponse();

            try
            {
                response.Endereco = new EnderecoApplication().Pesquisar(cep);
                response.Sucesso = true;
            }
            catch (Exception e)
            {
                response.Sucesso = false;
                response.Mensagem = e.Message;
            }

            return response;
        }
    }
}
