using System.Threading.Tasks;
using Processing.Models;

namespace Processing.App.Publishers
{
    public interface IOrderPublisher
    {
        Task OrderApproved(IOrderApprovedModel model);

    }
}
