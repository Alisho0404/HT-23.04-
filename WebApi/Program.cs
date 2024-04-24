using Infrastructure.Automapper;
using Infrastructure.Data;
using Infrastructure.Services.Bonus;
using Infrastructure.Services.CourseService;
using Infrastructure.Services.GroupService;
using Infrastructure.Services.MentorGroupService;
using Infrastructure.Services.MentorService;
using Infrastructure.Services.StudentGroupService;
using Infrastructure.Services.StudentService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IMentorService, MentorService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IMentorGroupService, MentorGroupService>();
builder.Services.AddScoped<IStudentGroupService, StudentGroupService>();

builder.Services.AddAutoMapper(typeof(MapperProfile));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();


