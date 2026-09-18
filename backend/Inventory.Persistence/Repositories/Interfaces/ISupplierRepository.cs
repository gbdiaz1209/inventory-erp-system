using Inventory.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace Inventory.Persistence.Repositories.Interfaces
    {
        public interface ISupplierRepository 
    {
     
        Task<Supplier> CreateAsync(Supplier entity);
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier> GetByIdAsync(int id);
        Task UpdateAsync(Supplier entity);



    }
    }