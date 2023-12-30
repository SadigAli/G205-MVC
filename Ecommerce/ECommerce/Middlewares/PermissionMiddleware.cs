namespace ECommerce.Middlewares
{
    public class PermissionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.StartsWithSegments("/admin") && context.Request.Path.Value != "/admin/account/login")
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    context.Response.Redirect("/admin/account/login");
                    return;
                }
            }
            else if(context.Request.Path.StartsWithSegments("/checkout"))
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    context.Response.Redirect("/account/login");
                    return;
                }
            }
            await next(context);
        }
    }
}
