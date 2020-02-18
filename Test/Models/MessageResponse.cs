using System;

namespace Test.Models
{
    public class MessageResponse:IMessageResponse
    {
       
        public Guid CorrelationId { get; set; }
        public string ResponseText { get; set; }
    }


}
