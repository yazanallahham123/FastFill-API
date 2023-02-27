using AutoMapper;
using FastFill_API.Data;
using FastFill_API.Interfaces;
using FastFill_API.Web.Dto;
using FastFill_API.Web.Model;
using FastFill_API.Web.Services;
using FastFill_API.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Controllers
{
    [Route("/api/[controller]")]
    public class DashboardController : Controller
    {
        private readonly UserServices _userServices;
        private readonly CompanyServices _companyServices;
        private readonly SecurityServices _securityServices;
        private readonly DashboardServices _dashboardServices;

        public DashboardController(SecurityServices securityServices, 
            UserServices userServices, 
            CompanyServices companyServices, 
            DashboardServices dashboardServices)
        {
            _securityServices = securityServices;
            _companyServices = companyServices;
            _userServices = userServices;
            _dashboardServices = dashboardServices;
        }

        // GET: api/user/admins
        [HttpGet("DashboardData")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDashboardData()
        {
            int totalUsers = 0;
            int totalCompanies = 0;
            int totalTransactionsCount = 0;
            double totalTransactionsAmount = 0;
            double totalFastFill = 0;
            WeekTotalPaymentTransactions weekTotalPaymentTransactions = new WeekTotalPaymentTransactions();
            List<CompanySales> topCompanies = new List<CompanySales>();

            List<PaymentTransaction> latestPaymentTransactions = new List<PaymentTransaction>();
            totalUsers = await _userServices.GetUsersCount();
            totalCompanies = await _companyServices.GetCompaniesCount();
            totalTransactionsCount = await _dashboardServices.GetTotalPaymentTransactionsCount(false, DateTime.Now, DateTime.Now);
            totalTransactionsAmount = await _dashboardServices.GetTotalPaymentTransactionsAmount(false, DateTime.Now, DateTime.Now);
            totalFastFill = await _dashboardServices.GetTotalFastFill(false, DateTime.Now, DateTime.Now);

            latestPaymentTransactions = await _dashboardServices.GetLatestPaymentTransactions();
            weekTotalPaymentTransactions = await _dashboardServices.GetWeekTotalPaymentTransactionsAmount();
            topCompanies = await _dashboardServices.GetTopCompanies();
            var response = new
            {
                TotalUsers = totalUsers,
                TotalCompanies = totalCompanies,
                TotalTransactionsCount = totalTransactionsCount,
                TotalTransactionsAmount = totalTransactionsAmount,
                TotalFastFill = totalFastFill,
                LatestPaymentTransactions = latestPaymentTransactions,
                WeekTotalPaymentTransactions = weekTotalPaymentTransactions,
                TopCompanies = topCompanies
            };
            
            return Ok(response);

        }

        // GET: api/user/admins
        [HttpPost("GetRefills")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRefills([FromBody] UserRefillFilter userRefillFilter)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            RefillsTotals refillsTotals = new RefillsTotals();

            IEnumerable<UserRefill> refills = await _userServices.GetRefills(userRefillFilter.mobileNumber,
                userRefillFilter.fromDate,
                userRefillFilter.toDate,
                userRefillFilter.status,
                userRefillFilter.transactionId,
                userRefillFilter.refillSources,
                userRefillFilter.page,
                userRefillFilter.pageSize,
                paginationInfo, refillsTotals
                );



            var response = new
            {
                Refills = refills,
                TotalAmount = refillsTotals.TotalAmount,
                TotalCount = refillsTotals.TotalCount,
                TotalSuccessAmount = refillsTotals.TotalSuccessAmount,
                TotalSuccessCount = refillsTotals.TotalSuccessCount,
                TotalFailAmount = refillsTotals.TotalFailAmount,
                TotalFailCount = refillsTotals.TotalFailCount,
                PaginationInfo = paginationInfo,
                RefillSourceAmounts = refillsTotals.RefillSourceAmounts
            };

            return Ok(response);
        }

        // GET: api/user/admins
        [HttpPost("GetPaymentTransactionResults")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaymentTransactionResults([FromBody] PaymentTransactionResultsFilter paymentTransactionResultFilter)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            PaymentTransactionResultTotals paymentTransactionResultTotals = new PaymentTransactionResultTotals();

            IEnumerable<PaymentTransaction> paymentTransactionResults = await _userServices.GetPaymentTransactionResults(paymentTransactionResultFilter.mobileNumber,
                paymentTransactionResultFilter.fromDate,
                paymentTransactionResultFilter.toDate,
                paymentTransactionResultFilter.companies,
                paymentTransactionResultFilter.page,
                paymentTransactionResultFilter.pageSize,
                paginationInfo, paymentTransactionResultTotals
                );



            var response = new
            {
                PaymentTransactionResults = paymentTransactionResults,
                TotalAmount = paymentTransactionResultTotals.TotalAmount,
                TotalCount = paymentTransactionResultTotals.TotalCount,
                TotalFastfill = paymentTransactionResultTotals.TotalFastfill,
                PaginationInfo = paginationInfo,
            };

            return Ok(response);
        }

    }
}
