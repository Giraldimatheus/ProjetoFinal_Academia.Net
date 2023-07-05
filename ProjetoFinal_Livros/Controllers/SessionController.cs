using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal_Livros.Controllers
{
    public class SessionController : Controller
    {
        [HttpGet]
        public IEnumerable<string> GetSessionInfo()
        {
            List<string> sessionInfo = new List<string>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionVariables.SessionKeyUsername)))
            {
                HttpContext.Session.SetString(SessionVariables.SessionKeyUsername, "Current User");
                HttpContext.Session.SetString(SessionVariables.SessionKeySessionId, Guid.NewGuid().ToString());
            }
            var usarname = HttpContext.Session.GetString(SessionVariables.SessionKeyUsername);
            var sessionId = HttpContext.Session.GetString(SessionVariables.SessionKeySessionId);

            sessionInfo.Add(usarname);
            sessionInfo.Add(sessionId);

            return sessionInfo;
        }
    }
}
