using InfectadosAPI.Configurations;
using InfectadosAPI.Data.Collections;
using InfectadosAPI.Filters;
using InfectadosAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace InfectadosAPI.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    [AllowAnonymous]
    public class UsuarioController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Usuario> _usuariosCollection;

        private readonly ILogger<UsuarioController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public UsuarioController(Data.MongoDB mongoDB, ILogger<UsuarioController> logger, IAuthenticationService authenticationService)
        {
            _mongoDB = mongoDB;
            _usuariosCollection = _mongoDB.DB.GetCollection<Usuario>(typeof(Usuario).Name.ToLower());

            _logger = logger;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Este serviço permite autenticar um usuário cadastrado.
        /// </summary>
        /// <param name="dto">Model do Usuário (Entrada)</param>
        /// <returns>Retorna status OK, usuário e token em caso de sucesso</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso na autenticação", Type = typeof(UsuarioDtoOutput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ErrorModel))]
        [SwaggerResponse(statusCode: 500, description: "Condição inesperada encontrada", Type = typeof(ErrorModel))]
        [HttpPost("autenticar")]
        [ValidacaoModelState]
        public IActionResult AutenticarUsuario(UsuarioDtoInput dto)
        {
            try
            {
                var existeUsuario = _usuariosCollection.Find(Builders<Usuario>.Filter.Where(_ => _.Login == dto.Login && _.Senha == dto.Senha)).CountDocuments();

                if (existeUsuario == 0){
                    return new BadRequestObjectResult(new ErrorModel(new string[] { "Usuário e/ou Senha inválidos." }));
                }

                var usuarioModelOutput = new UsuarioDtoOutput()
                {
                    Login = dto.Login,
                };

                var token = _authenticationService.GerarToken(usuarioModelOutput);

                return Ok(new UsuarioDtoOutput
                {
                    Login = dto.Login,
                    Token = token,
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new ObjectResult(new ErrorModel(new string[] { "Erro ao autenticar usuário: " + e.Message })) { StatusCode = 500 };
            }
        }

        /// <summary>
        /// Este serviço permite inserir um usuário.
        /// </summary>
        /// <param name="dto">Model do Usuário (Entrada)</param>
        /// <returns>Retorna status OK e mensagem de sucesso</returns>
        [SwaggerResponse(statusCode: 201, description: "Usuário adicionado", Type = typeof(SuccessModel))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ErrorModel))]
        [SwaggerResponse(statusCode: 500, description: "Condição inesperada encontrada", Type = typeof(ErrorModel))]
        [HttpPost("inserir")]
        [ValidacaoModelState]
        public IActionResult InserirUsuario(UsuarioDtoInput dto)
        {
            try
            {
                var existeUsuario = _usuariosCollection.Find(Builders<Usuario>.Filter.Where(_ => _.Login == dto.Login)).CountDocuments();

                if(existeUsuario > 0){
                    return new BadRequestObjectResult(new ErrorModel(new string[] { "Usuário já cadastrado." }));
                }

                var usuario = new Usuario(dto.Login, dto.Senha);
                _usuariosCollection.InsertOne(usuario);

                return Created("", new SuccessModel("Usuário inserido com sucesso."));
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new ObjectResult(new ErrorModel(new string[] { "Erro ao cadastrar usuário: " + e.Message })) { StatusCode = 500 };
            }
        }
    }
}
                