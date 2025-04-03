// Repositories/IFormDataRepository.cs
using Microsoft.AspNetCore.Mvc;
using PD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PD.Repositories
{
    public interface IFormDataRepository
    {
        Task<IEnumerable<FormData>> GetFormDataAsync();
        Task<FormData> AddFormDataAsync(FormData formData);
        Task<FormData> GetFormDataByIdAsync(int id);
        Task UpdateFormDataAsync(FormData formData);
    }

}
