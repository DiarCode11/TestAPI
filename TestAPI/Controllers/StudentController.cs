using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using TestAPI.Data;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly DataContext _context;

        public StudentController(DataContext context)
        {
            _context = context;
        }


        //Simpan Data Ke Database (CREATE)
        [HttpPost]
        public async Task<IActionResult> StoreData(RequestData request)
        {
            var data = new Student()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Semester = request.Semester,
                Birth = request.Birth
            };

            await _context.AddAsync(data);
            await _context.SaveChangesAsync();

            return Ok(data);
        }

        //Baca Semua Data dari Database (READ)
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            return Ok(await _context.Students.ToListAsync());
        }

        //Baca Data berdasarkan ID
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetDetail([FromRoute] Guid id)
        {
            var data = await _context.Students.FindAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        //Mengupdate Data berdasarkan ID (UPDATE)
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, RequestData request)
        {
            var data = await _context.Students.FindAsync(id);

            if (data != null)
            {
                data.Name = request.Name;
                data.Semester = request.Semester;
                data.Birth = request.Birth;

                await _context.SaveChangesAsync();
                return Ok(data);
            }

            return NotFound();
        }

        //Menghapus Data Berdasarkan ID (DELETE)
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var data = await _context.Students.FindAsync(id);

            if (data != null)
            {
                _context.Students.Remove(data);
                await _context.SaveChangesAsync();
                return Ok(data);
            }

            return NotFound();
        }
    }
}
