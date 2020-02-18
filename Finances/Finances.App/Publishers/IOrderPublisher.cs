using System.Threading.Tasks;

namespace Finances.App.Publishers
{
    public interface IOrderPublisher
    {
        Task OrderPaid(Models.IOrderPaidModel model);

    }
}
