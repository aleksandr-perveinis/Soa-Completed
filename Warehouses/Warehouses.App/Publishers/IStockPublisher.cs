using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Warehouses.App.Publishers
{
    public interface IStockPublisher
    {
        Task StockExist(Models.IStockExistModel model);

    }
}
