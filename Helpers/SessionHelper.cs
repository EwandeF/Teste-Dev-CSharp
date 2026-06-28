namespace TesteDevCSharp.Helpers
{
    public static class SessionHelper
    {
        public static int? GetUsuarioId(IHttpContextAccessor httpContextAccessor)
        {
            var id = httpContextAccessor.HttpContext?.Session.GetString("UsuarioId");
            if (id == null) return null;
            return int.TryParse(id, out var resultado) ? resultado : null;
        }

        public static void SetUsuario(IHttpContextAccessor httpContextAccessor, int id, string nome)
        {
            httpContextAccessor.HttpContext?.Session.SetString("UsuarioId", id.ToString());
            httpContextAccessor.HttpContext?.Session.SetString("UsuarioNome", nome);
        }

        public static void Limpar(IHttpContextAccessor httpContextAccessor)
        {
            httpContextAccessor.HttpContext?.Session.Clear();
        }
    }
}