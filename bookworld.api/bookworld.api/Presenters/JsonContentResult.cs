using Microsoft.AspNetCore.Mvc;

namespace bookworld.api.Presenters
{
    public class JsonContentResult : ContentResult
    {
        public JsonContentResult()
        {
            ContentType = "application/json";
        }
    }
}
