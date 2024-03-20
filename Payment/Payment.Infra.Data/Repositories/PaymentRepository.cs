using Payment.Domain.Interfaces.Repositories;
using Payment.Infra.Data.Context;
using TicketNow.Infra.Data.Repositories;

namespace Payment.Infra.Data.Repositories
{
    public class PaymentRepository : BaseRepository<Payments, int, ApplicationDbContext>, IPaymentsRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context)
        { }

    }
}
