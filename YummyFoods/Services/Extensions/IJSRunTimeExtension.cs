using Microsoft.JSInterop;

namespace YummyFoods.Services.Extensions
{
    public static class IJSRunTimeExtension
    {
        public static async Task ToasterSuccess(this IJSRuntime js, string message)
        {
            await js.InvokeVoidAsync("ShowToaster", "success", message);
        }
        public static async Task ToasterError(this IJSRuntime js, string message)
        {
            await js.InvokeVoidAsync("ShowToaster", "error", message);
        }
    }
}
