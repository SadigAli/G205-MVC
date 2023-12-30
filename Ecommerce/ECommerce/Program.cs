using Ecommerce.Data.Entities;
using Ecommerce.Data.Mappers;
using Ecommerce.Services.Contracts;
using Ecommerce.Services.Implementations;
using ECommerce.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            }).AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            builder.Services.AddAutoMapper(typeof(MapProfile));

            builder.Services.AddScoped<IColorRepository,ColorRepository>();
            builder.Services.AddScoped<ISizeRepository, SizeRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IFileService,FileService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IEmailRepository, EmailRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<LayoutService>();
            builder.Services.AddScoped<PermissionMiddleware>();
            builder.Services.AddHttpContextAccessor();
            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/login";
            //});
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<PermissionMiddleware>();

            app.MapControllerRoute(
                  name: "admin",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name : "contact",
                pattern : "contact-us", // /contact-us
                new { Controller = "Home",Action="Contact" }
                );

            //app.MapControllerRoute(
            //    name : "add-to-cart",
            //    pattern: "add-to-cart/{*productId}/{*sizeId}",
            //    new {Controller = "Cart", Action = "AddToCart"}
            //);

            app.MapControllerRoute(
                name : "product-detail",
                pattern : "product/{*slug}",
                new {Controller = "Product",Action="Detail"}
                );

            app.Run();
        }
    }
}
