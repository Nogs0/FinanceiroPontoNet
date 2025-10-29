using FinanceiroPontoNet.Application.Bancos;
using FinanceiroPontoNet.Application.Bancos.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FinanceiroPontoNet.Web.Controllers.Bancos
{
    [Route("bancos")]
    public class BancoController : ControllerBase
    {
        private readonly IBancoService _service;
        private readonly ILogger<BancoController> _logger;

        public BancoController(IBancoService service, ILogger<BancoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo banco.
        /// </summary>
        /// <param name="dto">Objeto contendo os dados necessários para a criação do banco.</param>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBancoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating a new banco.");
            var banco = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { codigo = banco.Codigo }, banco);
        }

        /// <summary>
        /// Busca um banco específico pelo seu código.
        /// </summary>
        /// <param name="codigo">O código do banco a ser buscado.</param>
        [HttpGet("{codigo}")]
        public async Task<IActionResult> Get(string codigo)
        {
            _logger.LogInformation($"Fetching banco with code: {codigo}", codigo);
            var banco = await _service.GetByCodigoAsync(codigo);
            return Ok(banco);
        }

        /// <summary>
        /// Retorna uma lista paginada com todos os bancos cadastrados.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching bancos.");
            var bancos = await _service.GetAllAsync();
            return Ok(bancos);
        }

        /// <summary>
        /// Atualiza as informações de um banco existente.
        /// </summary>
        /// <param name="id">O ID do banco a ser atualizado.</param>
        /// <param name="dto">Objeto contendo os dados atualizados do banco.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BancoDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("The route ID does not match the request body ID.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Updating banco with ID: {dto.Id}", id);
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// Exclui um banco.
        /// </summary>
        /// <param name="id">O ID do banco a ser excluído.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting banco with ID: {id}", id);
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
