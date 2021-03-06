using System.Collections.Generic;

namespace Core.Entities
{
    public class ResultViewModel
    {
        public ResultViewModel()
        {
            Errors = new List<ErrorModel>();
        }

        public List<ErrorModel> Errors { get; set; }

        public string Data { get; set; }
    }
}