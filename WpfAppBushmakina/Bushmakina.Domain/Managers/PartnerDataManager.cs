using Microsoft.EntityFrameworkCore;
using Bushmakina.Domain.Infrastructure;
using Bushmakina.Domain.Entities;
using Bushmakina.Domain.Calculators;

namespace Bushmakina.Domain.Managers
{
    public class PartnerDataManager : IPartnerManager
    {
        private readonly PartnersDbContext _context;

        public PartnerDataManager(PartnersDbContext context)
        {
            _context = context;
        }

        public async Task<List<PartnerEntity>> LoadAllPartnersAsync()
        {
            return await _context.Partners
                .Include(p => p.Type)
                .ToListAsync();
        }

        public async Task<PartnerEntity?> GetPartnerAsync(int id)
        {
            return await _context.Partners
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task StorePartnerAsync(PartnerEntity partner)
        {
            if (partner.Id == 0)
                _context.Partners.Add(partner);
            else
                _context.Partners.Update(partner);

            await _context.SaveChangesAsync();
        }

        public async Task RemovePartnerAsync(int id)
        {
            var partner = await _context.Partners.FindAsync(id);
            if (partner != null)
            {
                _context.Partners.Remove(partner);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<PartnerTypeEntity>> LoadPartnerTypesAsync()
        {
            return await _context.PartnerTypes.ToListAsync();
        }

        public async Task<List<SalesRecordEntity>> LoadSalesHistoryAsync(int partnerId)
        {
            return await _context.SalesRecords
                .Where(s => s.PartnerId == partnerId)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<int> CalculateTotalSalesAsync(int partnerId)
        {
            return await _context.SalesRecords
                .Where(s => s.PartnerId == partnerId)
                .SumAsync(s => s.Quantity);
        }

        public async Task<int> GetBonusPercentAsync(int partnerId)
        {
            var total = await CalculateTotalSalesAsync(partnerId);
            return BonusCalculator.CalculateBonusPercent(total);
        }

        public async Task RecordSaleAsync(SalesRecordEntity sale)
        {
            _context.SalesRecords.Add(sale);
            await _context.SaveChangesAsync();
        }
    }
}