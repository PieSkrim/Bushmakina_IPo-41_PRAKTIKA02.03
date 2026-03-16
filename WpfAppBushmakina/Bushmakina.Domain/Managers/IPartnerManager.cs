using Bushmakina.Domain.Entities;

namespace Bushmakina.Domain.Managers
{
    public interface IPartnerManager
    {
        Task<List<PartnerEntity>> LoadAllPartnersAsync();
        Task<PartnerEntity?> GetPartnerAsync(int id);
        Task StorePartnerAsync(PartnerEntity partner);
        Task RemovePartnerAsync(int id);
        Task<List<PartnerTypeEntity>> LoadPartnerTypesAsync();
        Task<List<SalesRecordEntity>> LoadSalesHistoryAsync(int partnerId);
        Task<int> CalculateTotalSalesAsync(int partnerId);
        Task<int> GetBonusPercentAsync(int partnerId);
        Task RecordSaleAsync(SalesRecordEntity sale);
    }
}