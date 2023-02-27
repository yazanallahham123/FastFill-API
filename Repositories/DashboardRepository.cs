using FastFill_API.Interfaces;
using FastFill_API.Web.Dto;
using FastFill_API.Web.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly FastFillDBContext _context;

        public DashboardRepository(FastFillDBContext context)
        {
            _context = context;
        }
        public async Task<int> GetTotalPaymentTransactionsCount(bool filterByDate, DateTime fromDate, DateTime toDate)
        {
            if (!filterByDate)
                return await _context.PaymentTransactions.CountAsync();
            else
            {
                return await _context.PaymentTransactions.Where(t => t.Date.Date >= fromDate.Date && t.Date.Date <= toDate.Date).CountAsync();
            }
        }

        public async Task<double> GetTotalPaymentTransactionsAmount(bool filterByDate, DateTime fromDate, DateTime toDate)
        {

            if (!filterByDate)
                return await _context.PaymentTransactions.Where(t => t.Status == true).SumAsync(i => (i.Amount - i.Fastfill));
            else
            {
                return await _context.PaymentTransactions.Where(t => t.Status == true && t.Date.Date >= fromDate.Date && t.Date.Date <= toDate.Date).SumAsync(i => (i.Amount - i.Fastfill));
            }
        }

        public async Task<double> GetTotalFastFill(bool filterByDate, DateTime fromDate, DateTime toDate)
        {
            if (!filterByDate)
                return await _context.PaymentTransactions.Where(t => t.Status == true).SumAsync(i => (i.Fastfill));
            else
            {
                return await _context.PaymentTransactions.Where(t => t.Status == true && t.Date.Date >= fromDate.Date && t.Date.Date <= toDate.Date).SumAsync(i => (i.Fastfill));
            }
        }

        public async Task<List<PaymentTransaction>> GetLatestPaymentTransactions()
        {
            return await _context.PaymentTransactions.OrderByDescending(u => u.Date).Take(20).ToListAsync();
        }

        public async Task<WeekTotalPaymentTransactions> GetWeekTotalPaymentTransactionsAmount()
        {

            WeekTotalPaymentTransactions weekTotalPaymentTransactions = new WeekTotalPaymentTransactions();

            weekTotalPaymentTransactions.SixDaysBefore = await _context.PaymentTransactions.Where(t => t.Status == true && t.Date.Date == (DateTime.Now.Date.AddDays(-6))).SumAsync(i => (i.Amount - i.Fastfill));
            weekTotalPaymentTransactions.FiveDaysBefore = await _context.PaymentTransactions.Where(t => t.Status == true && t.Date.Date == DateTime.Now.Date.AddDays(-7)).SumAsync(i => (i.Amount - i.Fastfill));
            weekTotalPaymentTransactions.FourDaysBefore = await _context.PaymentTransactions.Where(t => t.Status == true && t.Date.Date == DateTime.Now.Date.AddDays(-4)).SumAsync(i => (i.Amount - i.Fastfill));
            weekTotalPaymentTransactions.ThreeDaysBefore = await _context.PaymentTransactions.Where(t => t.Status == true && t.Date.Date == DateTime.Now.Date.AddDays(-3)).SumAsync(i => (i.Amount - i.Fastfill));
            weekTotalPaymentTransactions.TwoDaysBefore = await _context.PaymentTransactions.Where(t => t.Status == true && t.Date.Date == DateTime.Now.Date.AddDays(-2)).SumAsync(i => (i.Amount - i.Fastfill));
            weekTotalPaymentTransactions.OneDayBefore = await _context.PaymentTransactions.Where(t => t.Status == true && t.Date.Date == DateTime.Now.Date.AddDays(-1)).SumAsync(i => (i.Amount - i.Fastfill));
            weekTotalPaymentTransactions.Today = await _context.PaymentTransactions.Where(t => t.Status == true && t.Date.Date == DateTime.Now.Date).SumAsync(i => (i.Amount - i.Fastfill));

            return weekTotalPaymentTransactions;
        }

        public async Task<List<CompanySales>> GetTopCompanies()
        {

            List<CompanySales> companySales = new List<CompanySales>();

            companySales = await _context.PaymentTransactions.Where(x => (x.Status == true)).Include(x => x.Company).GroupBy(x => new {x.CompanyId, x.Company.ArabicName, x.Company.EnglishName}).Select(g => new
             CompanySales
                        {
                            Count = g.Count(),
                            Amount = g.Sum(c => c.Amount),
                            ArabicName = g.Key.ArabicName,
                            EnglishName = g.Key.EnglishName,
            }
            ).OrderByDescending(g => g.Amount).Take(10).ToListAsync();

            return companySales;
        }
    }
}
