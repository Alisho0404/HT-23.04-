using AutoMapper;
using Domain.DTO_s.StudentDTO;
using Domain.Enteties;
using Domain.Response;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Services.StudentService;
using System.Net;
using Microsoft.EntityFrameworkCore;



namespace Infrastructure.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public StudentService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
        }

        public async Task<Response<string>> AddStudentAsync(AddStudentDto studentDto)
        {
            try
            {
                var existing = await _context.Students.FirstOrDefaultAsync(x => x.Email == studentDto.Email);
                if (existing != null)
                {
                    return new Response<string>(HttpStatusCode.BadRequest, "Error, already exist");
                }
                var mapped = _mapper.Map<Student>(studentDto);

                await _context.Students.AddAsync(mapped);
                await _context.SaveChangesAsync();

                return new Response<string>("Succesfully added");

            }
            catch (Exception e)
            {
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<Response<bool>> DeleteStudentAsync(int studentId)
        {
            var delete = await _context.Students.Where(x => x.Id == studentId).ExecuteDeleteAsync();
            if (delete == 0)
            {
                return new Response<bool>(HttpStatusCode.BadRequest, "Not found");
            }
            return new Response<bool>(true);
        }

        public async Task<Response<List<GetStudentDto>>> GetStudentAsync()
        {
            try
            {
                var create = await _context.Students.ToListAsync();
                var mapped = _mapper.Map<List<GetStudentDto>>(create);
                return new Response<List<GetStudentDto>>(mapped);
            }
            catch (Exception e)
            {

                return new Response<List<GetStudentDto>>(HttpStatusCode.InternalServerError, e.Message);    
            }
        }

        public async Task<Response<GetStudentDto>> GetStudentByIdAsync(int id)
        {
            var create = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (create==null)
            {
                return new Response<GetStudentDto>(HttpStatusCode.BadRequest, "Not found");
            }
            var mapped=_mapper.Map<GetStudentDto>(create);
            return new Response<GetStudentDto>(mapped);
        }

        public async Task<Response<string>> UpdateStudentAsync(UpdateStudentDto studentDto)
        {
            try
            {
                var mapped = _mapper.Map<Student>(studentDto);
                _context.Students.Update(mapped);
                var update = await _context.SaveChangesAsync();
                if (update==0)
                {
                    return new Response<string>(HttpStatusCode.BadRequest, "Not found"); 
                }
                return new Response<string>("Succesfully updated");
            }
            catch (Exception e)
            {

                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
