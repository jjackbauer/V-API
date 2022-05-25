using System.Collections.Generic;

namespace VolvoApi.Model.DTO
{
    public abstract class Response
    {
        public List<string> Errors { get; }
        public Response()
        {
            Errors = new List<string>();
        }
        public void AddError(string message)
        {
            Errors.Add(message);
        }
    }
}
