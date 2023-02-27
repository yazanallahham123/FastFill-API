using FastFill_API.Web.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Interfaces
{
    public interface IDashboardRepository
    {
        public Task<int> GetTotalPaymentTransactionsCount(bool filterByDate, DateTime fromDate, DateTime toDate);

        public Task<double> GetTotalPaymentTransactionsAmount(bool filterByDate, DateTime fromDate, DateTime toDate);

        public Task<double> GetTotalFastFill(bool filterByDate, DateTime fromDate, DateTime toDate);

        public Task<List<PaymentTransaction>> GetLatestPaymentTransactions();

        public Task<WeekTotalPaymentTransactions> GetWeekTotalPaymentTransactionsAmount();

        public Task<List<CompanySales>> GetTopCompanies();
    }
}
