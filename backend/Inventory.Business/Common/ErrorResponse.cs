using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? Details { get; set; } // Útil en desarrollo, ocultar en producción
}