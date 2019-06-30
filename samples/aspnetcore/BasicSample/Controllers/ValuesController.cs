namespace Microsoft.Examples.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Web.Http;

    [ApiController]
    [ApiVersion( "1.0" )]
    [Route( "api/[controller]" )]
    public class ValuesController : ControllerBase
    {
        // GET api/values?api-version=1.0
        [HttpGet]
        public string Get( ApiVersion apiVersion ) => $"Controller = {GetType().Name}\nVersion = {apiVersion}";
    }
}