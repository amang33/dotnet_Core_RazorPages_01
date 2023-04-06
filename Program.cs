using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RazorPagesMovie.Data;
using RezorPagesMovie.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Razor Pages용 서비스를 앱에 추가(AddRazorPages())
builder.Services.AddRazorPages();

// if (builder.Environment.IsDevelopment()){
//     builder.Services.AddDbContext<RazorPagesMovieContext>(options =>
//         options.UseSqlite(builder.Configuration.GetConnectionString("RazorPagesMovieContext"))
//     );
// } else{
//     builder.Services.AddDbContext<RazorPagesMovieContext>(options =>
//         options.UseSqlServer(builder.Configuration.GetConnectionString("ProductionMovieContext"))
//     );
// }

builder.Services.AddDbContext<RazorPagesMovieContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RazorPagesMovieContext") ?? throw new InvalidOperationException("Connection string 'RazorPagesMovieContext' not found.")));

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Razor Pages용 엔드포인트를 IEndpointRouteBuilder(interface)에 추가
app.MapRazorPages();
// 엔드포인트 : HTTP 요청을 수신하고 처리하는 핸들러. (클라이언트가 서버로 보낸 HTTP 요청이 도달하는 곳이라고 생각하면 됨.)
// IEndpointRouteBuilder(interface) : 엔드포인트를 정의하고 라우팅을 구성하기 위한 빌더 클래스.
// IEndpointRouteBuilder 인터페이스는 Configure 메서드에서 사용
// IEndpointRouteBuilder : 1. HTTP 요청에 대한 경로 및 HTTP 메서드를 정의 2. 경로 및 HTTP 메서드에 대한 처리기(handler)를 등록 3. 엔드포인트에 필요한 데이터 및 제약 조건을 추가


app.Run();
