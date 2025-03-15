using System.Net.Http;
using System.Windows;

namespace TaskManagerGUI.Middleware
{
    public class ExceptionHandlerService
    {
        public void HandleException(Exception ex)
        {
            string errorMessage = ex.Message;
            
            //// Handle different types of exceptions
            //switch (ex)
            //{
            //    case HttpRequestException httpEx:
            //        //errorMessage = "Network error: Unable to connect to the server.";
            //        break;
            //    case NullReferenceException nullRefEx:
            //        errorMessage = "Oops! Something went wrong. Please try again.";
            //        break;
            //    case UnauthorizedAccessException unauthEx:
            //        //errorMessage = "Access denied. Please check your credentials.";
            //        break;
            //    case InvalidOperationException invalidOpEx:
            //        errorMessage = "Invalid operation. Please contact support.";
            //        break;
            //    default:
            //        errorMessage = "An unexpected error occurred.";
            //        break;
            //}

            // Show the error message in a dialog
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            

            // LogException(ex);
        }

        private void LogException(Exception ex)
        {
            // Simple console log for now
            Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
        }
    }
}
