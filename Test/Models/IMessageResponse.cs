using System;

namespace Test.Models
{
    public interface IMessageResponse
    {
        public Guid CorrelationId { get; set; }
        public string ResponseText { get; set; }
    }


}
