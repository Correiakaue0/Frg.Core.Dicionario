using Domain.Services;

namespace Frg.Core.Dicionario.Configuration
{
    public static class Configuration
    {
        public static void Seed(IConfiguration configuration)
        {
            ConfigSystem.FilePathPortuguese = configuration["FilePaths:FilePathPortuguese"];
            ConfigSystem.FilePathEnglish = configuration["FilePaths:FilePathEnglish"];
            ConfigSystem.FilePathSpanish = configuration["FilePaths:FilePathSpanish"];
        }
    }
}
