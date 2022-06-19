
using APIWeapon.Data;
using APIWeapon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWeapon.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<SuggestionClassModel> SuggestionClassModels { get; }
        IGenericRepository<SuggestionGameModel> SuggestionGameModels { get; }
        IGenericRepository<SuggestionWeaponModel> SuggestionWeaponModels { get; }
        Task Save();
    }
}