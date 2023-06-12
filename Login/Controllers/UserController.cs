using Login.Data;
using Login.Models;
using Login.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Http.ModelBinding;

namespace Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UsuarioContext _db;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, UsuarioContext db)
        {

            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUser()
        {
            _logger.LogInformation("Obtener el usuario");
            return Ok(await _db.Usuarios.ToListAsync());
        }


        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<UsuarioDto>> GetUser(int id)
        {
            if (id == 0)
            {
                _logger.LogError($"Error al traer el usuario con ID {id}");
                return BadRequest();
            }
            var product = await _db.Usuarios.FirstOrDefaultAsync(s => s.UserId == id);

            if (product == null)
            {
                _logger.LogError($"Error al traer el producto con id {id}");
                return NotFound();
            }

            return Ok(User);
        }

        /*Metodo que se utiliza para agregar un registro nuevo*/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<UsuarioDto>> AddUser([FromBody] CrearUsuarioDto usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //si esto se cumple el producto no se aggrega pero se muestra un mensaje de producto ya existente
            if (await _db.Usuarios.FirstOrDefaultAsync(s => s.UserName.ToLower() == usuarioDto.UserName.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "¡El usuario con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (usuarioDto == null)
            {
                return BadRequest(usuarioDto);
            }

            Usuario modelo = new()
            {
                UserName = usuarioDto.UserName
            };

            await _db.Usuarios.AddAsync(modelo);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetUser", new { id = modelo.UserId }, modelo);

        }
        /*Metodo encargado de eliminar un registro*/
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var user = await _db.Usuarios.FirstOrDefaultAsync(s => s.UserId == id);

            if (id == null)
            {
                return NotFound();
            }

            _db.Usuarios.Remove(user);
            await _db.SaveChangesAsync(true);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ActualizarUsuarioDto UpdateDTO)
        {
            if (UpdateDTO == null || id != UpdateDTO.UserId)
            {
                return BadRequest();
            }

            Usuario modelo = new()
            {
                UserId = UpdateDTO.UserId,
                UserName = UpdateDTO.UserName
            };

            _db.Usuarios.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialStudent(int id, JsonPatchDocument<ActualizarUsuarioDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var product = await _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == id);

            ActualizarUsuarioDto productDto = new()
            {
                UserId = product.UserId,
                UserName = product.UserName
            };
            if (product == null) return BadRequest();

            patchDto.ApplyTo(usuarioDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Usuario modelo = new()
            {
                UserId = product.UserId,
                UserName = product.UserName
            };

            _db.Usuarios.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
