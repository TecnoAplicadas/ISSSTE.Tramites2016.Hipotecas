#region

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ISSSTE.Tramites2016.Common.Security.Helpers;
using ISSSTE.Tramites2016.Common.Security.Identity;
using ISSSTE.Tramites2016.Common.Security.Owin;
using ISSSTE.Tramites2016.Common.Security.Web;
using ISSSTE.Tramites2016.Common.Util;
using ISSSTE.Tramites2016.Common.Web;
using ISSSTE.Tramites2016.Hipotecas.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Controllers
{
    /// <summary>
    ///     Controlador que maneja operaciones relacionadas a la sesión
    /// </summary>
    [Authorize]
    [RoutePrefix("Account")]
    public class AccountController : BaseController
    {
        #region Constructors

        public AccountController(ILogger logger)
            : base(logger)
        {
        }

        #endregion

        #region Actions

        /// <summary>
        ///     Despliega la página de login
        /// </summary>
        /// <param name="returnUrl">Url a la que redirigir una vez que se complete el logueo</param>
        /// <returns>Vista</returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return HandleOperationExecution(() =>
            {
                ViewBag.ReturnUrl = returnUrl;

                return View();
            });
        }

        /// <summary>
        ///     Procesa la respuesta del proveedor de identidad externo almacenando la información del usuario. En caso de se
        ///     llamado por el sitio propio, inicia el proceso de logueo con el proveedor de identidad externo
        /// </summary>
        /// <param name="returnUrl">Url a la que redirigir una vez que se complete el logueo</param>
        /// <param name="error">Error generado en el proceso de logueo</param>
        /// <returns>Redirección</returns>
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.AllowAnonymous]
        [NoAsyncTimeout]
        public async Task<ActionResult> ExternalLogin(string returnUrl, string error)
        {
            return await HandleOperationExecutionAsync<ActionResult>(async () =>
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;

                if (error != null)
                {
                    return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
                }

                var loginInfo = await authenticationManager.GetExternalLoginInfoAsync();

                if (loginInfo == null && !User.Identity.IsAuthenticated)
                {
                    return new IsssteChallengeResult(IsssteTramitesConstants.DefaultAuthenticationType,
                        Url.Action("ExternalLogin", "Account", new {ReturnUrl = returnUrl}));
                }

                if (loginInfo == null)
                    return Redirect(Url.Action("LoginError", "Account"));

                try
                {
                    var userManager =
                        HttpContext.GetOwinContext().GetUserManager<IsssteUserManager<IsssteIdentityUser>>();
                    var roleManager = HttpContext.GetOwinContext().GetUserManager<IsssteRoleManager<IdentityRole>>();
                    var signInManager = HttpContext.GetOwinContext().Get<IsssteSignInManager<IsssteIdentityUser>>();

                    var loginResult =
                        await
                            IsssteAuthorizationHelper.LoginAsync(loginInfo, userManager, roleManager, signInManager,
                                authenticationManager, Startup.ClientId);

                    //Token y Cookie
                    if (loginResult.Succeeded)
                    {
                        return Redirect(Url.Action("LoginComplete", "Account", new {ReturnUrl = returnUrl}));
                    }
                    var ex = new ArgumentException(loginResult.Errors.Single());

                    LogException(ex);

                    return Redirect(Url.Action("LoginError", "Account"));
                }
                catch (Exception ex)
                {
                    LogException(ex);

                    return Redirect(Url.Action("LoginError", "Account"));
                }
            });
        }

        /// <summary>
        ///     Despliega la´página que completa el logueo del lado del cliente
        /// </summary>
        /// <param name="returnUrl">Url a la que redirigir una vez que se complete el logueo</param>
        /// <returns>Vista</returns>
        [HttpGet]
        public ViewResult LoginComplete(string returnUrl)
        {
            return HandleOperationExecution(() =>
            {
                var owinContext = HttpContext.GetOwinContext();
                var userNameClaim =
                    owinContext.Authentication.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);

                var model = new LoginCompleteViewModel
                {
                    ClientId = Startup.ClientId,
                    UserName = userNameClaim == null ? "" : userNameClaim.Value,
                    ReturnUrl = returnUrl + "/#/search"
                };

                return View(model);
            });
        }

        /// <summary>
        ///     Despliega la pagina de error en el logueo
        /// </summary>
        /// <returns>Vista</returns>
        [AllowAnonymous]
        [HttpGet]
        public ViewResult LoginError()
        {
            return HandleOperationExecution(() => { return View(); });
        }

        /// <summary>
        ///     Cierra sesión tanto en la cookie como en OAuth 2.0 y despliega la página de cierre de sesión
        /// </summary>
        /// <param name="returnUrl">Página a la que redirigir una vez cumplido el cierre de sesión</param>
        /// <param name="soft">Indica si tambien se debe de cerrar la sesión del proveedor de indetidad o no</param>
        /// <returns>Vista</returns>
        [AllowAnonymous]
        [HttpGet]
        public ViewResult Logout(string returnUrl, bool soft = false)

        {
            return HandleOperationExecution(() =>
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;

                authenticationManager.SignOut();

                ViewBag.Soft = soft;
                ViewBag.ReturnUrl = returnUrl;

                return View();
            });
        }

        #endregion
    }
}