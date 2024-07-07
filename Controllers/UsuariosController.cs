using APILogin.Context;
using APILogin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TaskAPI.Context;
using TaskAPI.Models;

namespace APILogin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UsuariosController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> Get()
        {
            var usuarios = _context.Usuarios.ToList();
            if (usuarios is null)
            {
                return NotFound("Usuários não encontrados...");
            }
            return usuarios;
        }

        [Authorize]
        [HttpGet("{id:int}", Name = "ObterUsuario")]
        public ActionResult<Usuario> Get(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(p => p.UsuarioId == id);
            if (usuario is null)
            {
                return NotFound("Usuário não encontrado...");
            }
            return usuario;
        }

        [HttpPost("register")]
        public ActionResult Register(Usuario usuario)
        {
            if (usuario is null)
            {
                return BadRequest();
            }
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterUsuario", new { id = usuario.UsuarioId }, usuario);
        }


        [HttpPost("login")]
        public ActionResult<Usuario> Post(Usuario usuario)
        {
            if (usuario is null)
            {
                return BadRequest();
            }
            var user = _context.Usuarios.FirstOrDefault(p => p.NomeUsuario == usuario.NomeUsuario);
            if (user is null)
            {
                return NotFound("Usuário não encontrado...");
            }
            if (user.Senha != usuario.Senha)
            {
                return Unauthorized();
            }
            return Ok(GeraToken(user));

        }

        [Authorize]
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return BadRequest();
            }
            _context.Entry(usuario).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(usuario);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(p => p.UsuarioId == id);

            if (usuario is null)
            {
                return NotFound("Usuário não encontrado...");
            }
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return Ok(usuario);
        }
        private UsuarioToken GeraToken(Usuario userInfo)
        {
            //define declarações do usuário
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.NomeUsuario),
                 new Claim("meuPet", "pipoca"),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            //gera uma chave com base em um algoritmo simetrico
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            //gera a assinatura digital do token usando o algoritmo Hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Tempo de expiracão do token.
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            // classe que representa um token JWT e gera o token
            JwtSecurityToken token = new JwtSecurityToken(
              issuer: _configuration["TokenConfiguration:Issuer"],
              audience: _configuration["TokenConfiguration:Audience"],
              claims: claims,
              expires: expiration,
              signingCredentials: credenciais);

            //retorna os dados com o token e informacoes
            return new UsuarioToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };
        }
    }
}