using APIWeapon.Data;
using APIWeapon.Models;
using APIWeapon.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWeapon.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<SuggestionClassModel> _scm;
        private IGenericRepository<SuggestionGameModel> _sgm;
        private IGenericRepository<SuggestionWeaponModel> _swm;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IGenericRepository<SuggestionClassModel> SuggestionClassModels => _scm ??= new GenericRepository<SuggestionClassModel>(_context);
        public IGenericRepository<SuggestionGameModel> SuggestionGameModels => _sgm ??= new GenericRepository<SuggestionGameModel>(_context);
        public IGenericRepository<SuggestionWeaponModel> SuggestionWeaponModels => _swm ??= new GenericRepository<SuggestionWeaponModel>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}