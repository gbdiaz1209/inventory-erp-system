namespace Inventory.WebAPI.Configurations

{
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty; //La firma digital. Debe ser larga (mínimo 256 bits / 32 caracteres) para que los algoritmos de seguridad modernos no tengan vulnerabilidades.
        public string Issuer { get; set; } = string.Empty;//(Emisor): Quién genera el token (en este caso, tu API de .NET).
        public string Audience { get; set; } = string.Empty;//(Audiencia): Para quién está destinado el token (tu app de Angular). El backend verificará esto para asegurarse de que el token no venga de otra aplicación extraña.
        public int DurationInMinutes { get; set; }//El tiempo de vida del token. Por seguridad empresarial, se recomienda que expiren rápido (ej. 1 hora).
    }
}
public class JwtSettings
{

}
