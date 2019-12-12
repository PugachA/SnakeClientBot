using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeClient.Models
{
    public class SnakeAPIResponse<T>
    {
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
