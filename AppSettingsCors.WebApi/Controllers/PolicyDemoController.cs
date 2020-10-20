using System.Web.Http;

namespace AppSettingsCors.WebApi.Controllers
{
    public class PolicyDemoController : ApiController
    {
        [HttpGet]
        [CorsPolicyA, CorsPolicyB]
        public string AB()
        {
            return "Hello A & B";
        }

        [HttpGet]
        [AppSettingsCors("AllowedOriginsCors_A", "AllowedHeadersCors_A", "AllowedMethodsCors_A")]
        public string A()
        {
            return "Hello A";
        }

        [HttpGet]
        [CorsPolicyB]
        public string B()
        {
            return "Hello B";
        }

        [HttpGet]
        [ConfigCorsPolicyAttribute("Policy_C")]
        public string C()
        {
            return "Hello C";
        }
    }
}
