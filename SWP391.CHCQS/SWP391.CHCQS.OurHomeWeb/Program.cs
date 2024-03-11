﻿using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using SWP391.CHCQS.DataAccess.Data;
using SWP391.CHCQS.DataAccess.Repository;
using SWP391.CHCQS.DataAccess.Repository.IRepository;
using SWP391.CHCQS.Services.NotificationHub;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using SWP391.CHCQS.Utility;
using SWP391.CHCQS.Services;
using SWP391.CHCQS.Services.NotificationHub;
using NotificationHub = SWP391.CHCQS.Services.NotificationHub.NotificationHub;
using SWP391.CHCQS.Model;

namespace SWP391.CHCQS.OurHomeWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<SWP391DBContext>(
				options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<SWP391DBContext>().AddDefaultTokenProviders();
			builder.Services.ConfigureApplicationCookie(options => {
				options.LoginPath = $"/Identity/Account/Login";
				options.LogoutPath = $"/Identity/Account/Logout";
				options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
			});


			builder.Services.AddRazorPages();

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IEmailSender, EmailSender>();

            //Thêm service SignalR
            builder.Services.AddSignalR();

            //Câu lệnh đăng ký một dịch vụ bộ nhớ phân phối (distributed memory cache) trong container dịch vụ của ứng dụng.
            builder.Services.AddDistributedMemoryCache();

            
            //đăng ký dịch vụ phiên (session) trong ứng dụng
            //đăng ký dịch vụ phiên trong container dịch vụ của ứng dụng.
            builder.Services.AddSession(options =>
            {

                //được sử dụng để thiết lập thời gian chờ phiên. 
                options.IdleTimeout = TimeSpan.FromMinutes(30);

                //cho cookie chỉ có thể được truy cập thông qua HTTP và không thể được truy cập từ mã JavaScript
                options.Cookie.HttpOnly = true;

                //cookie này là bắt buộc và không thể được loại bỏ.
                //Quan trọng khi cần tới Identity
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("BasePolicy", policy =>
                {
                    policy.RequireRole("Manager", "Seller", "Engineer");
                });
            });

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

			app.UseRouting();
            app.UseAuthentication();//before authorization

			app.UseAuthorization();
			app.MapRazorPages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            

            //Khai báo app có sử dụng signalR
            app.MapHub<NotificationHub>("/notificationServer");

            //Khai báo app có sử dụng Session thì truy cập HttpContext.Session mới được
            //Khai báo trước app.UseRouting(); app.UseAuthorization(); sau app.UseEndpoints app.Run();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
                );

            });
			
			app.Run();
		}
	}
}