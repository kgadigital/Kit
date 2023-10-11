using KitAplication.Models.Enums;
using System.Security.Cryptography.X509Certificates;

namespace KitAplication.Interface
{
    public interface IFileHandler
    {
        void LoggToTextFile(string inputInformation,string input);
        
        void LoggQAToTextFileMonthly(string question, string answer, TextLoggLevel logLevel, string Tagname = "");
    }
}
