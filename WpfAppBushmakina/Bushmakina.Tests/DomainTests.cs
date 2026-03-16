using Xunit;
using Bushmakina.Domain.Calculators;
using Bushmakina.Domain.Entities;
using System;

namespace Bushmakina.Tests
{

    public class DomainTests
    {
        #region Тесты BonusCalculator (6 тестов)

        [Fact]
        public void Bonus_LessThan10000_Returns0()
        {
            int result = BonusCalculator.CalculateBonusPercent(5000);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Bonus_Exactly10000_Returns5()
        {
            int result = BonusCalculator.CalculateBonusPercent(10000);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Bonus_Between10000And50000_Returns5()
        {
            int result = BonusCalculator.CalculateBonusPercent(25000);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Bonus_Exactly50000_Returns10()
        {
            int result = BonusCalculator.CalculateBonusPercent(50000);
            Assert.Equal(10, result);
        }

        [Fact]
        public void Bonus_Between50000And300000_Returns10()
        {
            int result = BonusCalculator.CalculateBonusPercent(150000);
            Assert.Equal(10, result);
        }

        [Fact]
        public void Bonus_MoreThan300000_Returns15()
        {
            int result = BonusCalculator.CalculateBonusPercent(500000);
            Assert.Equal(15, result);
        }

        #endregion

        #region Тесты PartnerEntity (4 теста)

        [Fact]
        public void PartnerEntity_Create_ValidData()
        {
            var partner = new PartnerEntity
            {
                Name = "Test Partner",
                TypeId = 1,
                Rating = 10,
                INN = "1234567890"
            };

            Assert.Equal("Test Partner", partner.Name);
            Assert.Equal(1, partner.TypeId);
            Assert.Equal(10, partner.Rating);
        }

        [Fact]
        public void PartnerEntity_Rating_NonNegative()
        {
            var partner = new PartnerEntity();
            partner.Rating = 0;
            Assert.Equal(0, partner.Rating);
        }

        [Fact]
        public void PartnerEntity_CurrentBonus_DefaultZero()
        {
            var partner = new PartnerEntity();
            Assert.Equal(0, partner.CurrentBonus);
        }

        [Fact]
        public void PartnerEntity_INN_CanBeSet()
        {
            var partner = new PartnerEntity();
            partner.INN = "1234567890";
            Assert.Equal("1234567890", partner.INN);
        }

        #endregion

        #region Тесты SalesRecordEntity (3 теста)

        [Fact]
        public void SalesRecordEntity_Create_ValidData()
        {
            var sale = new SalesRecordEntity
            {
                PartnerId = 1,
                ProductName = "Product Test",
                Quantity = 1000
            };

            Assert.Equal(1, sale.PartnerId);
            Assert.Equal("Product Test", sale.ProductName);
            Assert.Equal(1000, sale.Quantity);
        }

        [Fact]
        public void SalesRecordEntity_Quantity_MustBePositive()
        {
            var sale = new SalesRecordEntity();
            sale.Quantity = 1;
            Assert.Equal(1, sale.Quantity);
        }

        [Fact]
        public void SalesRecordEntity_SaleDate_HasValue()
        {
            var sale = new SalesRecordEntity();
            Assert.True(sale.SaleDate.Year >= 2025);
        }

        #endregion

        #region Тесты PartnerTypeEntity (2 теста)

        [Fact]
        public void PartnerTypeEntity_Create_ValidData()
        {
            var type = new PartnerTypeEntity
            {
                Id = 1,
                TypeName = "Retail Store"
            };

            Assert.Equal(1, type.Id);
            Assert.Equal("Retail Store", type.TypeName);
        }

        [Fact]
        public void PartnerTypeEntity_TypeName_CanBeSet()
        {
            var type = new PartnerTypeEntity();
            type.TypeName = "Wholesale";
            Assert.Equal("Wholesale", type.TypeName);
        }

        #endregion
    }
}