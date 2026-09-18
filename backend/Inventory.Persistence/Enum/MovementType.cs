using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Persistence.Enum
{
        public enum MovementType
        {
            // Usamos valores numéricos para que sea más fácil de manejar en la DB
            Entry = 0,      // Entradas (compras, devoluciones de clientes)
            Exit = 1,       // Salidas (ventas, mermas)
            Adjustment = 2  // Ajustes manuales (inventarios físicos)
        }
    }
