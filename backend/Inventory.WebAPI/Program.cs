using FluentValidation;
using Inventory.Business.DomainServices;
using Inventory.Business.Interfaces;
using Inventory.Business.Interfaces.Repositories;
using Inventory.Persistence.Contexts;
using Inventory.Persistence.Repositories;
using Inventory.Persistence.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURACIÓN DE SERVICIOS (CONTENEDOR DI)
// ==========================================

// Configuración de la Base de Datos
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de Dependencias
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IInventoryMovementService, InventoryMovementService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Servicios de validación
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 🛠️ FUSIÓN UNIFICADA DE SWAGGER: COMENTARIOS XML + SEGURIDAD JWT (Solo se registra una vez)
builder.Services.AddSwaggerGen(options =>
{
    // 1. Configuración básica e info
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Inventory System API",
        Version = "v1",
        Description = "API para la gestión de inventario y movimientos de productos."
    });

    // 2. Leer los comentarios XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // 3. Definir el esquema de seguridad (JWT Bearer)
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa el token JWT en este formato: Bearer {tu_token_aquí}"
    });

    // 4. Aplicar el requisito de seguridad global en la interfaz de Swagger (Corregido)
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configuración de CORS (Actualizada para permitir tu puerto de Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:49261")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configuración de sección JWT fuertemente tipada
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<Inventory.WebAPI.Configurations.JwtSettings>(jwtSection);

// Configurar el esquema de autenticación JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtKey = builder.Configuration["Jwt:Key"] ?? string.Empty;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// ==========================================
// 2. CONSTRUCCIÓN DE LA APLICACIÓN Y MIDDLEWARES
// ==========================================
var app = builder.Build();

// Entorno de Desarrollo para Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// El orden de ejecución de la autopista HTTP importa estrictamente:
app.UseRouting();

app.UseCors("AllowAngular"); // Aplica la política específica que definiste arriba

app.UseAuthentication(); // 1. ¿Quién eres?
app.UseAuthorization();  // 2. ¿A qué tienes permiso?

app.MapControllers(); // Registra las rutas de los controladores

app.Run();