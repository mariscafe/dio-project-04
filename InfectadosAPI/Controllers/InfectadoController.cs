using InfectadosAPI.Data.Collections;
using InfectadosAPI.Filters;
using InfectadosAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;

namespace InfectadosAPI.Controllers
{
    [Route("api/infectados")]
    [ApiController]
    [Authorize]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        private readonly ILogger<UsuarioController> _logger;

        public InfectadoController(Data.MongoDB mongoDB, ILogger<UsuarioController> logger)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());

            _logger = logger;
        }

        /// <summary>
        /// Este serviço permite inserir um infectado.
        /// </summary>
        /// <param name="dto">Model do Infectado</param>
        /// <returns>Retorna status OK e dados do infectado</returns>
        [SwaggerResponse(statusCode: 201, description: "Infectado adicionado", Type = typeof(InfectadoDtoOutput))]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado", Type = typeof(EmptyResult))]
        [SwaggerResponse(statusCode: 500, description: "Condição inesperada encontrada", Type = typeof(ErrorModel))]
        [HttpPost("inserir")]
        [ValidacaoModelState]
        public ActionResult InserirInfectado([FromBody] InfectadoDtoInput dto)
        {
            try
            {
                var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

                _infectadosCollection.InsertOne(infectado);
 
                return Created("", infectado);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new ObjectResult(new ErrorModel(new string[] { "Erro ao cadastrar infectado: " + e.Message })) { StatusCode = 500 };
            }
        }

        /// <summary>
        /// Este serviço permite atualizar as informações de um infectado registrado
        /// </summary>
        /// <param name="id">Identificador do Infectado</param>
        /// <param name="dto">Model do Infectado</param>
        /// <returns>Retorna status OK e dados do infectado</returns>
        [SwaggerResponse(statusCode: 200, description: "Infectado atualizado", Type = typeof(InfectadoDtoOutput))]
        [SwaggerResponse(statusCode: 400, description: "ID não reconhecido", Type = typeof(ErrorModel))]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado", Type = typeof(EmptyResult))]
        [SwaggerResponse(statusCode: 500, description: "Condição inesperada encontrada", Type = typeof(ErrorModel))]
        [HttpPut("atualizar/{id}")]
        [ValidacaoModelState]
        public ActionResult AtualizarInfectado(String id, [FromBody] InfectadoDtoInput dto)
        {
            try
            {
                var infectado = new Infectado(id, dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

                long atualizado = _infectadosCollection.ReplaceOne(Builders<Infectado>.Filter.Where(_ => _.Id == id), infectado).ModifiedCount;

                if (atualizado == 1)
                {
                    return Ok(infectado);
                }
                else
                {
                    return new BadRequestObjectResult(new ErrorModel(new string[] { "Infectado não encontrado." }));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new ObjectResult(new ErrorModel(new string[] { "Erro ao atualizar infectado: " + e.Message })) { StatusCode = 500 };
            }
        }

        /// <summary>
        /// Este serviço permite excluir um infectado registrado
        /// </summary>
        /// <param name="id">Identificador do Infectado</param>
        /// <returns>Mensagem de sucesso</returns>
        [SwaggerResponse(statusCode: 200, description: "Infectado excluído", Type = typeof(SuccessModel))]
        [SwaggerResponse(statusCode: 400, description: "ID não reconhecido", Type = typeof(ErrorModel))]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado", Type = typeof(EmptyResult))]
        [SwaggerResponse(statusCode: 500, description: "Condição inesperada encontrada", Type = typeof(ErrorModel))]
        [HttpDelete("excluir/{id}")]
        public ActionResult ExcluirInfectado(String id)
        {
            try
            {
                long excluido = _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(_ => _.Id == id)).DeletedCount;

                if (excluido == 1)
                {
                    return Ok(new SuccessModel("Infectado excluido com sucesso."));
                }
                else
                {
                    return new BadRequestObjectResult(new ErrorModel(new string[] { "Infectado não encontrado." }));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new ObjectResult(new ErrorModel(new string[] { "Erro ao excluir infectado: " + e.Message })) { StatusCode = 500 };
            }
        }

        /// <summary>
        /// Este serviço permite recuperar as informações de todos os infectados registrados
        /// </summary>
        /// <returns>Retorna status OK e lista de infectados</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao obter infectados", Type = typeof(InfectadoDtoOutput))]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado", Type = typeof(EmptyResult))]
        [SwaggerResponse(statusCode: 500, description: "Condição inesperada encontrada", Type = typeof(ErrorModel))]
        [HttpGet("listar")]
        public ActionResult ObterInfectados()
        {
            try
            {
                var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();

                return Ok(infectados);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new ObjectResult(new ErrorModel(new string[] { "Erro ao obter infectados: " + e.Message })) { StatusCode = 500 };
            }
        }       
    }
}
