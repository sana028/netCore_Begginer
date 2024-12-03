using Microsoft.AspNetCore.Mvc;
using Net_Beginner_web_app.Models;

namespace Net_Beginner_web_app.Delegate_Events
{
    public class ApiNotification<T>
    {
        public delegate IActionResult ToasterDelegate(string message = "", string type = "success");

        public event ToasterDelegate Toaster;

        public async Task<T> ExecuteApiCall(Func<Task<T>> apiCall,string action)
        {
            try
            {
                dynamic result = await apiCall();
                //T r = await apiCall();

                if (result !=null)
                {
                    Toaster?.Invoke($"{action} Task completed successfully", "success")?.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                // Show error toaster
                Toaster?.Invoke(ex.Message,"error")?.ToString();
                throw;
            }
            Toaster?.EndInvoke(null);
        }
    }
}
