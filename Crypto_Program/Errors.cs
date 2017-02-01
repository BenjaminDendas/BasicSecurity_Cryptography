using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Crypto_Program
{
    public static class Errors
    {
        // Error methode om errors weg te schrijven naar aparte files.
        public static void Error(Exception ex, string user, string logfile)
        {
            FileStream errorLog = null;
            StreamWriter errorLogWriter = null;
            try
            {
                errorLog = new FileStream("Logs" + "\\" + logfile + ".log", FileMode.Append, FileAccess.Write);
                errorLogWriter = new StreamWriter(errorLog);
                DateTime date = DateTime.Now;
                string datum = (date.Day + "/" + date.Month + "/" + date.Year + "   " + date.Hour + ":" + date.Minute + ":" + date.Second).ToString();
                errorLogWriter.WriteLine("-------" + datum + "--------");
                errorLogWriter.WriteLine("------- Start Error Message -------");
                errorLogWriter.WriteLine(ex.Message);
                errorLogWriter.WriteLine();
                errorLogWriter.WriteLine("------- End Error Message -------");
                errorLogWriter.WriteLine();
                errorLogWriter.WriteLine("-------- Start Stacktrace --------");
                errorLogWriter.WriteLine(ex.StackTrace);
                errorLogWriter.WriteLine();
                errorLogWriter.WriteLine("-------- End Stacktrace -----------");
                errorLogWriter.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Er ging iets mis bij het throwen van de Exception", "Fatal Error", MessageBoxButton.OK);
            }
            finally
            {
                if (errorLog != null)
                {
                    errorLog.Close();
                }

                if (errorLogWriter != null)
                {
                    errorLogWriter.Close();
                }

            }
        }

        public static void Error(Exception ex, string logfile)
        {
            FileStream errorLog = null;
            StreamWriter errorLogWriter = null;
            try
            {
                errorLog = new FileStream("Logs" + "\\" + logfile + ".log", FileMode.Append, FileAccess.Write);
                errorLogWriter = new StreamWriter(errorLog);
                DateTime date = DateTime.Now;
                string datum = (date.Day + "/" + date.Month + "/" + date.Year + "   " + date.Hour + ":" + date.Minute + ":" + date.Second).ToString();
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                errorLogWriter.WriteLine("-------" + datum + "--------");
                errorLogWriter.WriteLine("------- Start Error Message -------");
                errorLogWriter.WriteLine(ex.Message);
                errorLogWriter.WriteLine();
                errorLogWriter.WriteLine("------- End Error Message -------");
                errorLogWriter.WriteLine();
                errorLogWriter.WriteLine("-------- Start Stacktrace --------");
                errorLogWriter.WriteLine(ex.StackTrace);
                errorLogWriter.WriteLine();
                errorLogWriter.WriteLine("-------- End Stacktrace -----------");
                errorLogWriter.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Er ging iets mis bij het throwen van de Exception", "Fatal Error", MessageBoxButton.OK);
            }
            finally
            {
                if (errorLog != null)
                {
                    errorLog.Close();
                }

                if (errorLogWriter != null)
                {
                    errorLogWriter.Close();
                }
            }
        }

        public class NotEnoughSizeInPhotoException : ApplicationException
        {
            public NotEnoughSizeInPhotoException() : base() { }

            public NotEnoughSizeInPhotoException(string message) : base(message) { }

            public NotEnoughSizeInPhotoException(string message, Exception innerException) : base(message, innerException) { }
        }

        public class EncryptionException : ApplicationException
        {
            public EncryptionException() : base() { }

            public EncryptionException(string message) : base(message) { }

            public EncryptionException(string message, Exception innerException) : base(message, innerException) { }
        }

        public class NotAllFilledException : ApplicationException
        {
            public NotAllFilledException() : base() { }

            public NotAllFilledException(string message) : base(message) { }

            public NotAllFilledException(string message, Exception innerException) : base(message, innerException) { }
        }


    }
}
