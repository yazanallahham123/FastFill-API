using FastFill_API.Interfaces;
using FastFill_API.Web.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Services
{
    public class DashboardServices
    {
        private readonly IPublicRepository _repository;
        private readonly SecurityServices _securityServices;

        public DashboardServices(IPublicRepository repository, SecurityServices securityServices)
        {
            _repository = repository;
            _securityServices = securityServices;
        }

        public async Task<int> GetTotalPaymentTransactionsCount(bool filterByDate, DateTime fromDate, DateTime toDate)
        {
            return await _repository.GetDashboardRepository.GetTotalPaymentTransactionsCount(filterByDate, fromDate, toDate);
        }
        public async Task<double> GetTotalPaymentTransactionsAmount(bool filterByDate, DateTime fromDate, DateTime toDate)
        {
            return await _repository.GetDashboardRepository.GetTotalPaymentTransactionsAmount(filterByDate, fromDate, toDate);
        }

        public async Task<double> GetTotalFastFill(bool filterByDate, DateTime fromDate, DateTime toDate)
        {
            return await _repository.GetDashboardRepository.GetTotalFastFill(filterByDate, fromDate, toDate);
        }

        public async Task<List<PaymentTransaction>> GetLatestPaymentTransactions()
        {
            return await _repository.GetDashboardRepository.GetLatestPaymentTransactions();
        }

        public async Task<WeekTotalPaymentTransactions> GetWeekTotalPaymentTransactionsAmount()
        {
            return await _repository.GetDashboardRepository.GetWeekTotalPaymentTransactionsAmount();
        }

        public async Task<List<CompanySales>> GetTopCompanies()
        {
            return await _repository.GetDashboardRepository.GetTopCompanies();
        }
    }
}
