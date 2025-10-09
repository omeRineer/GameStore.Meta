using C = Core.Utilities.ResultTool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Meta.Controllers
{
    [Route("metaapi/[controller]")]
    public class BaseController : ControllerBase
    {
        protected IActionResult Response(C.IResult result)
            => result.Success ? Ok(result) : BadRequest(result);
    }
}
