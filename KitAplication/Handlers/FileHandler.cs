using KitAplication.Interface;
using KitAplication.Models.Enums;
using System.Drawing.Text;

namespace KitAplication.Handler
{
    public class FileHandler : IFileHandler
    {
        protected IConfiguration _configuration;
        private readonly ILogger<FileHandler> _logger;

        public FileHandler(IConfiguration configuration, ILogger<FileHandler> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        /// <summary>
        ///Logs the input information and input to a text file  with the current date and time stamp.
        /// </summary>
        /// <param name="inputInformation">Information about the input parameter to be loggd</param>
        /// <param name="input">Input to be logged</param>
        /// <remarks>The log file path is retrieved from the configuration settings.
        /// The log information is formatted with the current date and time stamp and saved to the log file.
        /// If the log file already exists, new information is appended to it.</remarks>
        public void LoggToTextFile(string inputInformation,string input)
        {
            string docPhat = _configuration["LogPath"];
            if (docPhat == null)
            {
                _logger.LogError("{0}, FilePath in configuration not found. Logg to textfile dont work", DateTime.Now);
            }
            else
            {
                string formatInputString = String.Format("Datum:{0}.[INF] {1}: {2}", DateTime.Now, inputInformation, input);
                using (StreamWriter outputFile = new StreamWriter(Path.GetFullPath(docPhat), append: true))
                {
                    outputFile.WriteLine(formatInputString);
                }
            }

        }
        public void LoggQAToTextFileMonthly(string question, string answer, TextLoggLevel logLevel, string Tagname = "")
        {
            string id = Guid.NewGuid().ToString();
            string logPath = _configuration["LogPath"];
            string logFileName = _configuration["LogPathName"];
            string currentMonth = DateTime.Now.ToString("yyyy-MM");
            //string currentMonth = "2023-04";
            string filename = $"{currentMonth}__{logFileName}";
            string path = Path.Combine(logPath, filename);
            // Ensure the directory exists, create it if it doesn't
            Directory.CreateDirectory(logPath);

            if (logPath == null || logFileName == null || path == null)
            {
                _logger.LogError("{0}, LoghPath or LogPathName in configuration not found. Logg to textfile dont work", DateTime.Now);
            }
            else
            {
                string formatQuestionString = String.Format("[{0}] Datum:{1}.{2}:{3}: {4}", id, DateTime.Now, Tagname, logLevel, question);
                string formatAnswerString = String.Format("[{0}] Datum:{1}.{2}:{3}: {4}", id, DateTime.Now, Tagname, logLevel, answer);
                using (StreamWriter outputFile = new StreamWriter(Path.GetFullPath(path), append: true))
                {
                    outputFile.WriteLine(formatQuestionString);
                    outputFile.WriteLine(formatAnswerString);
                }
            }
        }
    }
}
