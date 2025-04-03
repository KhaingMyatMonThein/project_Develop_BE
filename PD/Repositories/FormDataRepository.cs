// Repositories/FormDataRepository.cs
using Microsoft.EntityFrameworkCore;
using PD.Data;
using PD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PD.Repositories
{
    public class FormDataRepository : IFormDataRepository
    {
        private readonly ApplicationDbContext _context;

        public FormDataRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FormData>> GetFormDataAsync()
        {
            return await _context.FormData.ToListAsync();
        }

        public async Task<FormData> AddFormDataAsync(FormData formData)
        {
            _context.FormData.Add(formData);
            await _context.SaveChangesAsync();
            return formData;
        }

        public async Task<FormData> GetFormDataByIdAsync(int id)
        {
            return await _context.FormData.FindAsync(id);
        }

        public async Task UpdateFormDataAsync(FormData formData)
        {
            _context.Entry(formData).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }

}
