using AvitoRuParser.Data;
using AvitoRuParser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvitoRuParser.Services
{
    public class BlacklistService
    {
        private readonly HashSet<string> _blacklistedItems = new(StringComparer.OrdinalIgnoreCase);
        private readonly AppDbContext _dbContext;
        private readonly string _blacklistPath;

        public BlacklistService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _blacklistPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blacklist.txt");
            LoadBlacklist();
            SyncWithDatabase();
        }

        private void LoadBlacklist()
        {
            if (File.Exists(_blacklistPath))
            {
                var lines = File.ReadAllLines(_blacklistPath)
                             .Where(line => !string.IsNullOrWhiteSpace(line))
                             .Select(line => line.Trim());
                _blacklistedItems.UnionWith(lines);
            }
        }

        private void SyncWithDatabase()
        {
            // Добавляем в БД новые компании из черного списка
            var existingCompanies = _dbContext.Vacancies
                .Where(v => _blacklistedItems.Contains(v.Company))
                .Select(v => v.Company)
                .Distinct()
                .ToHashSet();

            var newCompanies = _blacklistedItems.Except(existingCompanies);
            foreach (var company in newCompanies)
            {
                _dbContext.BlacklistedCompanies.Add(new BlacklistedCompany { Name = company });
            }
            _dbContext.SaveChanges();
        }

        public bool IsBlacklisted(Vacancy vacancy)
        {
            return _blacklistedItems.Contains(vacancy.Company) ||
                   (!string.IsNullOrEmpty(vacancy.SiteId) && _blacklistedItems.Contains(vacancy.SiteId));
        }
    }
}
