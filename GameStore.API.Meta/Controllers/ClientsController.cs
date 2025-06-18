using GameStore.Meta.Business.Services;
using GameStore.Meta.Models.Rest.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Meta.Controllers
{
    public class ClientsController : BaseController
    {
        readonly ClientService ClientService;

        public ClientsController(ClientService clientService)
        {
            ClientService = clientService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var result = await ClientService.GetDetailAsync(id);

            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateClientModel model)
        {
            var result = await ClientService.CreateClientAsync(model);

            return Ok(result);
        }
    }
}
