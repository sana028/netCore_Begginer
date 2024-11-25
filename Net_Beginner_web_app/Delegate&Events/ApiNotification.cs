using Microsoft.AspNetCore.Mvc;

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
                

                var result = await apiCall();
                
                // Show success toaster
                Toaster?.Invoke($"{action} Task completed successfully", "success")?.ToString();
                return result;
            }
            catch (Exception ex)
            {
                // Show error toaster
                Toaster?.Invoke("An error occurred",ex.Message)?.ToString();
               
                throw;
            }
            Toaster?.EndInvoke(null);
        }
    }
}
