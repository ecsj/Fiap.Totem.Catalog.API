using System.Text;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Catalog.UnitTests.Base
{
    public abstract class BaseUnitTest
    {
        public IConfiguration Configuration { get; set; }

        protected BaseUnitTest()
        {
            var appSettings = @"{""ConnectionStrings"":{
            ""MongoDB"" : ""Value1"",
            ""Key2"" : ""Value2"",
            ""Key3"" : ""Value3""
            }}";

            var builder = new ConfigurationBuilder();

            builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));

            Configuration = builder.Build();
        }
    }
}
