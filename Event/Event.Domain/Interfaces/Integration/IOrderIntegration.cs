using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Domain.Interfaces.Integration
{
    public interface IOrderIntegration
    {
        Task<bool> GetOrderActiveEvent(int eventId, string token);
    }
}
