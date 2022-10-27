using Microsoft.AspNetCore.Http;

namespace EsemkaOnePlus.Request
{
    public class PostPhotoRequest
    {
        public FormFile fromFile { get; set; }
    }
}
