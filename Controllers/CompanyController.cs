using AutoMapper;
using FastFill_API.Data;
using FastFill_API.Extentions;
using FastFill_API.Web.Dto;
using FastFill_API.Web.Model;
using FastFill_API.Web.Services;
using FastFill_API.Web.Utils;
using FastFill_API.Web.Utils.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FastFill_API.Controllers
{
    [Route("/api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserServices _userServices;
        private readonly SecurityServices _securityServices;
        private readonly CompanyServices _companyServices;

        public CompanyController(IMapper mapper, UserServices userServices, SecurityServices securityServices, CompanyServices companyServices)
        {
            _mapper = mapper;
            _userServices = userServices;
            _securityServices = securityServices;
            _companyServices = companyServices;
        }




        // GET: api/company/CompanyByCode
        [HttpGet("CompanyByCode/{code}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyByCode(string code)
        {
            if (code.Trim() == "")
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            CompanyWithFavorite company = await _companyServices.GetCompanyByCode(code, userId);

            if (company == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            return Ok(_mapper.Map<Company>(company));

        }

        // GET: api/company/AllCompanies
        [HttpGet("CompaniesByName/{name}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompaniesByName(int page = 1, int pageSize = 10, string name = "")
        {
            if (name.Trim() == "")
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyWithFavorite> companies = await _companyServices.SearchCompaniesByName(page, pageSize, paginationInfo, name, userId);
            IEnumerable<CompanyWithFavorite> company = _mapper.Map<IEnumerable<CompanyWithFavorite>>(companies);

            var response = new
            {
                Companies = company,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // GET: api/company/AllCompanies
        [HttpGet("CompaniesBranchesByName/{name}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompaniesBranchesByName(int page = 1, int pageSize = 10, string name = "")
        {
            if (name.Trim() == "")
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyBranchWithFavorite> companiesBranches = await _companyServices.SearchCompaniesBranchesByName(page, pageSize, paginationInfo, name, userId);
            IEnumerable<CompanyBranchWithFavorite> companyBranch = _mapper.Map<IEnumerable<CompanyBranchWithFavorite>>(companiesBranches);

            var response = new
            {
                CompaniesBranches = companyBranch,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // GET: api/company/AllCompanies
        [HttpGet("CompaniesBranchesByText/{text}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompaniesBranchesByText(int page = 1, int pageSize = 10, string text = "")
        {
            if (text.Trim() == "")
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyBranchWithFavorite> companiesBranches = await _companyServices.SearchCompaniesBranchesByText(page, pageSize, paginationInfo, text, userId);
            IEnumerable<CompanyBranchWithFavorite> companyBranch = _mapper.Map<IEnumerable<CompanyBranchWithFavorite>>(companiesBranches);

            var response = new
            {
                CompaniesBranches = companyBranch,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // GET: api/company/AllCompanies
        [HttpGet("CompaniesByText/{text}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompaniesByText(int page = 1, int pageSize = 10, string text = "")
        {
            if (text.Trim() == "")
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyWithFavorite> companies = await _companyServices.SearchCompaniesByText(page, pageSize, paginationInfo, text, userId);
            IEnumerable<CompanyWithFavorite> company = _mapper.Map<IEnumerable<CompanyWithFavorite>>(companies);

            var response = new
            {
                Companies = company,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // GET: api/company/AllCompaniesBranches
        [HttpGet("AllCompaniesBranches")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCompaniesBranches(int page = 1, int pageSize = 10)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyBranch> companiesBranches = await _companyServices.GetAllCompaniesBranches(page, pageSize, paginationInfo, userId);
            IEnumerable<CompanyBranch> companyBranch = _mapper.Map<IEnumerable<CompanyBranch>>(companiesBranches);

            var response = new
            {
                CompaniesBranches = companyBranch,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // GET: api/company/CompanyBranches
        [HttpGet("CompanyBranches/{companyId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyBranches(int page = 1, int pageSize = 10, int companyId = 0)
        {

            if (companyId == 0)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyBranch> companiesBranches = await _companyServices.GetCompanyBranches(page, pageSize, paginationInfo, companyId, userId);
            IEnumerable<CompanyBranch> companyBranch = _mapper.Map<IEnumerable<CompanyBranch>>(companiesBranches);

            var response = new
            {
                CompaniesBranches = companyBranch,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // GET: api/company/CompanyBranchByCode
        [HttpGet("CompanyBranchByCode/{code}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyBranchByCode(string code)
        {            

            if (code.Trim() == "")
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            CompanyBranchWithFavorite companyBranch = await _companyServices.GetCompanyBranchByCode(code, userId);          

            if (companyBranch == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            return Ok(_mapper.Map<CompanyBranch>(companyBranch));

        }

        // GET: api/company/CompanyBranchesByAddress
        [HttpGet("CompanyBranchesByAddress/{address}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyBranchesByAddress(int page = 1, int pageSize = 10, string address = "")
        {
            if (address.Trim() == "")
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyBranch> companiesBranches = await _companyServices.GetCompanyBranchesByAddress(page, pageSize, paginationInfo, address, userId);
            IEnumerable<CompanyBranch> companyBranch = _mapper.Map<IEnumerable<CompanyBranch>>(companiesBranches);

            var response = new
            {
                CompaniesBranches = companyBranch,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // PUT: api/company/AddToFavorite
        [HttpPut("AddToFavorite")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddCompanyToFavorite([FromBody] AddRemoveCompanyFavoriteDto addRemoveCompanyFavorite)
        {
            if (addRemoveCompanyFavorite == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (addRemoveCompanyFavorite.companyId <= 0)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }
            
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userId <= 0)
            {
                return BadRequest(ResponseMessages.NOT_LOGGEDIN);
            }

            return Ok(await _companyServices.AddCompanyToFavorite(userId, addRemoveCompanyFavorite.companyId));
        }

        // PUT: api/company/RemoveFromFavorite
        [HttpPut("RemoveFromFavorite")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveCompanyFromFavorite([FromBody] AddRemoveCompanyFavoriteDto addRemoveCompanyFavorite)
        {
            if (addRemoveCompanyFavorite == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (addRemoveCompanyFavorite.companyId <= 0)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userId <= 0)
            {
                return BadRequest(ResponseMessages.NOT_LOGGEDIN);
            }

            return Ok(await _companyServices.RemoveCompanyFromFavorite(userId, addRemoveCompanyFavorite.companyId));
        }

        // PUT: api/company/AddBranchToFavorite
        [HttpPut("AddBranchToFavorite")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddCompanyBranchToFavorite([FromBody] AddRemoveCompanyBranchFavoriteDto addRemoveCompanyBranchFavorite)
        {
            if (addRemoveCompanyBranchFavorite == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (addRemoveCompanyBranchFavorite.companyBranchId <= 0)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userId <= 0)
            {
                return BadRequest(ResponseMessages.NOT_LOGGEDIN);
            }

            return Ok(await _companyServices.AddCompanyBranchToFavorite(userId, addRemoveCompanyBranchFavorite.companyBranchId));
        }

        // PUT: api/company/RemoveBranchFromFavorite
        [HttpPut("RemoveBranchFromFavorite")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveCompanyBranchFromFavorite([FromBody] AddRemoveCompanyBranchFavoriteDto addRemoveCompanyBranchFavorite)
        {
            if (addRemoveCompanyBranchFavorite == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (addRemoveCompanyBranchFavorite.companyBranchId <= 0)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userId <= 0)
            {
                return BadRequest(ResponseMessages.NOT_LOGGEDIN);
            }

            return Ok(await _companyServices.RemoveCompanyBranchFromFavorite(userId, addRemoveCompanyBranchFavorite.companyBranchId));
        }

        // GET: api/company/FavoriteCompanies
        [HttpGet("FavoriteCompanies")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFavoriteCompanies(int page = 1, int pageSize = 10)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyWithFavorite> companies = await _companyServices.GetFavoriteCompanies(page, pageSize, paginationInfo, userId);
            IEnumerable<CompanyWithFavorite> company = _mapper.Map<IEnumerable<CompanyWithFavorite>>(companies);

            var response = new
            {
                Companies = company,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // GET: api/company/FavoriteCompaniesBranches
        [HttpGet("FavoriteCompaniesBranches")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFavoriteCompaniesBranches(int page = 1, int pageSize = 10)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyBranchWithFavorite> companiesBranches = await _companyServices.GetFavoriteCompaniesBranches(page, pageSize, paginationInfo, userId);
            IEnumerable<CompanyBranchWithFavorite> companyBranches = _mapper.Map<IEnumerable<CompanyBranchWithFavorite>>(companiesBranches);

            var response = new
            {
                CompaniesBranches = companyBranches,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // GET: api/company/FrequentlyVisitedCompaniesBranches
        [HttpGet("FrequentlyVisitedCompaniesBranches")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFrequentlyVisitedCompaniesBranches(int page = 1, int pageSize = 10)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyBranchWithFavorite> companiesBranches = await _companyServices.GetFrequentlyVisitedCompaniesBranches(page, pageSize, paginationInfo, userId);
            IEnumerable<CompanyBranchWithFavorite> companyBranches = _mapper.Map<IEnumerable<CompanyBranchWithFavorite>>(companiesBranches);

            var response = new
            {
                CompaniesBranches = companyBranches,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // GET: api/company/FrequentlyVisitedCompanies
        [HttpGet("FrequentlyVisitedCompanies")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFrequentlyVisitedCompanies(int page = 1, int pageSize = 10)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyWithFavorite> companies = await _companyServices.GetFrequentlyVisitedCompanies(page, pageSize, paginationInfo, userId);
            IEnumerable<CompanyWithFavorite> company = _mapper.Map<IEnumerable<CompanyWithFavorite>>(companies);

            var response = new
            {
                Companies = company,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // POST: api/company/InsertCompanyBranch
        [HttpPost("InsertCompanyBranch")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertCompanyBranch([FromBody] UpdateCompanyBranchDto updateCompanyBranchDto)
        {
            if (updateCompanyBranchDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            CompanyBranch mappedCompanyBranch = new CompanyBranch();
            mappedCompanyBranch.ArabicName = updateCompanyBranchDto.ArabicName;
            mappedCompanyBranch.EnglishName = updateCompanyBranchDto.EnglishName;
            mappedCompanyBranch.ArabicAddress = updateCompanyBranchDto.ArabicAddress;
            mappedCompanyBranch.EnglishAddress = updateCompanyBranchDto.EnglishAddress;
            mappedCompanyBranch.Code = updateCompanyBranchDto.Code;
            mappedCompanyBranch.CompanyId = updateCompanyBranchDto.CompanyId;

            bool res = await _companyServices.AddCompanyBranch(mappedCompanyBranch);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.ADDED_SUCCESSFULLY);
        }


        // Put: api/company/UpdateCompanyBranch
        [HttpPut("UpdateCompanyBranch")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCompanyBranch([FromBody] UpdateCompanyBranchDto updateCompanyBranchDto)
        {
            if (updateCompanyBranchDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            CompanyBranch mappedCompanyBranch = new CompanyBranch();
            mappedCompanyBranch.Id = updateCompanyBranchDto.Id;
            mappedCompanyBranch.ArabicName = updateCompanyBranchDto.ArabicName;
            mappedCompanyBranch.EnglishName = updateCompanyBranchDto.EnglishName;
            mappedCompanyBranch.ArabicAddress = updateCompanyBranchDto.ArabicAddress;
            mappedCompanyBranch.EnglishAddress = updateCompanyBranchDto.EnglishAddress;
            mappedCompanyBranch.Code = updateCompanyBranchDto.Code;
            mappedCompanyBranch.CompanyId = updateCompanyBranchDto.CompanyId;

            bool res = await _companyServices.UpdateCompanyBranch(mappedCompanyBranch);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

        // Delete: api/company/DeleteCompanyBranch
        [HttpDelete("DeleteCompanyBranch/{id}")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCompanyBranch(int id)
        {
            CompanyBranch companyBranch = await _companyServices.GetCompanyBranchById(id);

            if (companyBranch == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            bool res = await _companyServices.DeleteCompanyBranch(companyBranch);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.DELETED_SUCCESSFULLY);
        }


        // POST: api/company/InsertCompany
        [HttpPost("InsertCompany")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertCompany([FromBody] UpdateCompanyDto updateCompanyDto)
        {
            if (updateCompanyDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            Company mappedCompany = new Company();
            mappedCompany.ArabicName = updateCompanyDto.ArabicName;
            mappedCompany.EnglishName = updateCompanyDto.EnglishName;
            mappedCompany.ArabicAddress = updateCompanyDto.ArabicAddress;
            mappedCompany.EnglishAddress = updateCompanyDto.EnglishAddress;
            mappedCompany.Code = updateCompanyDto.Code;
            mappedCompany.GroupId = updateCompanyDto.GroupId;
            mappedCompany.Disabled = updateCompanyDto.Disabled;
            mappedCompany.AutoAddToFavorite = updateCompanyDto.AutoAddToFavorite;

            bool res = await _companyServices.AddCompany(mappedCompany);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.ADDED_SUCCESSFULLY);
        }


        // Put: api/company/UpdateCompany
        [HttpPut("UpdateCompany")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyDto updateCompanyDto)
        {
            if (updateCompanyDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            Company mappedCompany = new Company();
            mappedCompany.Id = updateCompanyDto.Id;
            mappedCompany.ArabicName = updateCompanyDto.ArabicName;
            mappedCompany.EnglishName = updateCompanyDto.EnglishName;
            mappedCompany.ArabicAddress = updateCompanyDto.ArabicAddress;
            mappedCompany.EnglishAddress = updateCompanyDto.EnglishAddress;
            mappedCompany.Code = updateCompanyDto.Code;
            mappedCompany.GroupId = updateCompanyDto.GroupId;
            mappedCompany.Disabled = updateCompanyDto.Disabled;
            mappedCompany.AutoAddToFavorite = updateCompanyDto.AutoAddToFavorite;

            bool res = await _companyServices.UpdateCompany(mappedCompany);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

        // Delete: api/company/DeleteCompany
        [HttpDelete("DeleteCompany/{id}")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            Company company = await _companyServices.GetCompanyById(id);

            if (company == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            bool res = await _companyServices.DeleteCompany(company);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.DELETED_SUCCESSFULLY);
        }

        // GET: api/company/FrequentlyVisitedCompanies
        [HttpGet("Users/{companyId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyUsers(int companyId, int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<User> companyUsers = await _companyServices.GetCompanyUsers(page, pageSize, paginationInfo, companyId);
            IEnumerable<User> companyUser = _mapper.Map<IEnumerable<User>>(companyUsers);

            var response = new
            {
                CompanyUsers = companyUser,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // Put: api/company/AddUser
        [HttpPost("AddUser")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddUserToCompany([FromBody] AddUserToCompanyDto addUserToCompanyDto)
        {
            if (addUserToCompanyDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            bool res = await _companyServices.AddUserToCompany(addUserToCompanyDto.userId, addUserToCompanyDto.companyId);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.ADDED_SUCCESSFULLY);
        }

        // Put: api/company/AddUser
        [HttpDelete("RemoveUser")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveUserFromCompany([FromBody] AddUserToCompanyDto addUserToCompanyDto)
        {
            if (addUserToCompanyDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            bool res = await _companyServices.RemoveUserFromCompany(addUserToCompanyDto.userId, addUserToCompanyDto.companyId);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.DELETED_SUCCESSFULLY);
        }


        // GET: api/company/FrequentlyVisitedCompaniesBranches
        [HttpGet("GetStationPaymentTransactions")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyPaymentTransactions(bool filterByDate, DateTime filterFromDate, DateTime filterToDate,  int page = 1, int pageSize = 10)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<PaymentTransaction> companyPaymentTransactions = await _companyServices.GetCompanyPaymentTransactions(page, pageSize, paginationInfo, userId, filterByDate, filterFromDate, filterToDate);

            var response = new
            {
                StationPaymentTransactions = companyPaymentTransactions,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        [HttpGet("GetMonthlyStationPaymentTransactions")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMonthlyStationPaymentTransactions(int page = 1, int pageSize = 10)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<MonthlyPaymentTransaction> monthlyCompanyPaymentTransactions = await _companyServices.GetMonthlyCompanyPaymentTransactions(page, pageSize, paginationInfo, userId);

            var response = new
            {
                MonthlyStationPaymentTransactions = monthlyCompanyPaymentTransactions,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }


        [HttpGet("GetTotalStationPaymentTransactions")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalCompanyPaymentTransactions(bool filterByDate, DateTime filterFromDate, DateTime filterToDate)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            TotalPaymentTransaction companyTotalPaymentTransactions = await _companyServices.GetTotalCompanyPaymentTransactions(userId, filterByDate, filterFromDate, filterToDate);

            return Ok(companyTotalPaymentTransactions);
        }

        // GET: api/company/FrequentlyVisitedCompaniesBranches
        [HttpGet("GetStationPaymentTransactionsPDF")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyPaymentTransactionsPDF(bool filterByDate, DateTime filterFromDate, DateTime filterToDate)

        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            User user = await _userServices.GetUserById(userId, 0);

            DateTime fromDate = filterFromDate;
            DateTime toDate = filterToDate;

            var paymentTransactions = await _companyServices.GetCompanyPaymentTransactionsPDF(userId, true, fromDate, toDate);

            string title = "Transactions Details Report";
            string dateTitle = "Date";
            string mobileNumberTitle = "Customer NO.";
            string userTitle = "Name";
            string amountTitle = "Amount";
            string currencyCode = "SDG";
            string fromDateTitle = "From: ";
            string toDateTitle = "To: ";

            if (user.Language == 2)
            {
                title = " تقرير تفصيلي بالعمليات ".ArabicWithFontGlyphsToPfd();
                dateTitle = " التاريخ ".ArabicWithFontGlyphsToPfd(); ;
                mobileNumberTitle = " رقم العميل ".ArabicWithFontGlyphsToPfd(); ;
                userTitle = " اسم العميل ".ArabicWithFontGlyphsToPfd(); ;
                amountTitle = " القيمة ".ArabicWithFontGlyphsToPfd(); ;
                currencyCode = " ج.س ".ArabicWithFontGlyphsToPfd(); ;
                fromDateTitle = "من : ".ArabicWithFontGlyphsToPfd(); ;
                toDateTitle = "إلى : ".ArabicWithFontGlyphsToPfd(); ;
            }

            PdfDocument document = new PdfDocument();
            // Page Options
            PdfPage pdfPage = document.AddPage();
            pdfPage.Height = 3508;//842
            pdfPage.Width = 2480;

            // Get an XGraphics object for drawing
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);

            // Text format
            XStringFormat format = new XStringFormat();
            format.LineAlignment = XLineAlignment.Near;
            format.Alignment = XStringAlignment.Near;
            var tf = new XTextFormatter(graph);

            XFont fontParagraph = new XFont("Arial", 40, XFontStyle.Regular);

            // Row elements
            int el1_width = 150;
            int el2_width = 516;
            int el3_width = 316;
            int el4_width = 682;
            int el5_width = 416;


            // page structure options
            double lineHeight = 100;
            int marginLeft = 200;
            int marginTop = 200;

            int el_height = 100;
            int rect_height = 100;

            int interLine_X_1 = 2;
            int interLine_X_2 = 2 * interLine_X_1;
            int interLine_X_3 = 3 * interLine_X_1;
            int interLine_X_4 = 4 * interLine_X_1;
            int interLine_X_5 = 5 * interLine_X_1;


            int offSetX_1 = el1_width;
            int offSetX_2 = el1_width + el2_width;
            int offSetX_3 = el1_width + el2_width + el3_width;
            int offSetX_4 = el1_width + el2_width + el3_width + el4_width;
            int offSetX_5 = el1_width + el2_width + el3_width + el4_width + el5_width;

            XSolidBrush rect_style1 = new XSolidBrush(XColors.LightGray);

            XSolidBrush rect_style2 = new XSolidBrush(XColor.FromArgb(255, 220, 189));

            XPen rect_style2_pen = new XPen(XColors.Black);

            XSolidBrush rect_style3 = new XSolidBrush(XColor.FromArgb(209, 207, 142));
            int i = -1;
            int x = -1;

            var logoPath = Directory.GetCurrentDirectory() + @"\attachments\logo.png";
            XImage image = XImage.FromFile(logoPath);

            graph.DrawImage(image, 1140, 10, 200, 200);


            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

            tf.DrawString(title, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20, marginTop + 10, pdfPage.Width - 2 * marginLeft, el_height), format);

            x = x + 1;
            i = i + 1;
            double dist_Y = lineHeight * (x + 1);
            double dist_Y2 = dist_Y - 2;
            int pageNo = 1;
            int line = 0;
            //graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, (marginLeft + 20 + offSetX_4 + 2 * interLine_X_4) + el5_width, rect_height);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el1_width, rect_height);
            tf.DrawString("#", fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
            tf.DrawString(dateTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
            tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
            tf.DrawString(userTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
            tf.DrawString(amountTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);

            double totalMonthlyAmount = 0;

                line = 0;
                i = i + 1;
                x = x + 1;

                dist_Y = lineHeight * (x + 1);
                dist_Y2 = dist_Y - 2;

                if ((marginTop + dist_Y) > 3250)
                {
                    tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

                    pageNo = pageNo + 1;

                    PdfPage newPage = document.AddPage();
                    newPage.Height = 3508;//842
                    newPage.Width = 2480;
                    graph = XGraphics.FromPdfPage(newPage);
                    tf = new XTextFormatter(graph);
                    x = -1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                    logoPath = Directory.GetCurrentDirectory() + @"\attachments\logo.png";
                    image = XImage.FromFile(logoPath);

                    graph.DrawImage(image, 1090, 10, 300, 300);


                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                    tf.DrawString(title, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft, marginTop + 10, pdfPage.Width - 2 * marginLeft, el_height), format);

                    x = x + 1;
                    i = i + 1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el5_width, rect_height);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el1_width, rect_height);
                    tf.DrawString("#", fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
                    tf.DrawString(dateTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
                    tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
                    tf.DrawString(userTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
                    tf.DrawString(amountTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);


                    x = x + 1;
                    i = i + 1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;


                }


                graph.DrawRectangle(rect_style2_pen, rect_style3, marginLeft, marginTop + dist_Y, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                tf.DrawString(fromDateTitle+" "+fromDate.ToString("dd-MM-yyyy") + " " + toDateTitle + " " + toDate.ToString("dd-MM-yyyy"), fontParagraph, XBrushes.Black,
                              new XRect(marginLeft + 20, marginTop + dist_Y + 10, pdfPage.Width - 2 * marginLeft, el_height), format);


                totalMonthlyAmount = 0;
                foreach (var item in paymentTransactions)
                {
                    line = line + 1;
                    totalMonthlyAmount = totalMonthlyAmount + (item.Amount - item.Fastfill);

                    i = i + 1;
                    x = x + 1;

                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                    if ((marginTop + dist_Y) > 3250)
                    {

                        tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
                          new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

                        pageNo = pageNo + 1;

                        PdfPage newPage = document.AddPage();
                        newPage.Height = 3508;//842
                        newPage.Width = 2480;
                        graph = XGraphics.FromPdfPage(newPage);
                        tf = new XTextFormatter(graph);
                        x = -1;
                        dist_Y = lineHeight * (x + 1);
                        dist_Y2 = dist_Y - 2;

                        logoPath = Directory.GetCurrentDirectory() + @"\attachments\logo.png";
                        image = XImage.FromFile(logoPath);

                        graph.DrawImage(image, 1090, 10, 300, 300);


                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                        tf.DrawString(title, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20, marginTop + 10, pdfPage.Width - 2 * marginLeft, el_height), format);

                        x = x + 1;
                        i = i + 1;
                        dist_Y = lineHeight * (x + 1);
                        dist_Y2 = dist_Y - 2;

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el5_width, rect_height);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el1_width, rect_height);
                        tf.DrawString("#", fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height), format);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
                        tf.DrawString(dateTitle, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height), format);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
                        tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height), format);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
                        tf.DrawString(userTitle, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height), format);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
                        tf.DrawString(amountTitle, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);


                        x = x + 1;
                        i = i + 1;
                        dist_Y = lineHeight * (x + 1);
                        dist_Y2 = dist_Y - 2;

                    }
                    //dist_Y = lineHeight * (x);
                    //dist_Y2 = dist_Y - 2;


                    // header della G
                    /*if (i == 2)
                    {
                        graph.DrawRectangle(rect_style2, marginLeft, marginTop, pdfPage.Width - 2 * marginLeft, rect_height);

                        tf.DrawString("#", fontParagraph, XBrushes.White,
                                      new XRect(marginLeft, marginTop, el1_width, el_height), format);

                        tf.DrawString(dateTitle, fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_1 + interLine_X_1, marginTop, el2_width, el_height), format);

                        tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_2 + 2 * interLine_X_2, marginTop, el1_width, el_height), format);

                        tf.DrawString(userTitle, fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_3 + 2 * interLine_X_3, marginTop, el1_width, el_height), format);

                        tf.DrawString(amountTitle, fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_4 + 2 * interLine_X_4, marginTop, el1_width, el_height), format);


                    }
                    else */
                    {

                        //if (i % 2 == 1)
                        //{
                        //  graph.DrawRectangle(TextBackgroundBrush, marginLeft, lineY - 2 + marginTop, pdfPage.Width - marginLeft - marginRight, lineHeight - 2);
                        //}

                        //ELEMENT 1 - SMALL 80
                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft, marginTop + dist_Y, el1_width, rect_height);
                        tf.DrawString(

                            line.ToString(),
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height),
                            format);

                        //ELEMENT 2 - BIG 380
                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
                        tf.DrawString(
                            item.Date.ToString("yyyy-MM-dd hh:mm tt"),
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height),
                            format);


                        //ELEMENT 3 - SMALL 80

                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
                        tf.DrawString(
                            item.User.Id.ToString(),
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height),
                            format);

                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
                        tf.DrawString(
                            item.User.FirstName,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height),
                            format);


                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
                        tf.DrawString(
                            String.Format("{0:n0}", item.Amount-item.Fastfill) + " " + currencyCode,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height),
                            format);

                    }

                }


                i = i + 1;
                x = x + 1;

                dist_Y = lineHeight * (x + 1);
                dist_Y2 = dist_Y - 2;


                if ((marginTop + dist_Y) > 3250)
                {
                    tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

                    pageNo = pageNo + 1;

                    PdfPage newPage = document.AddPage();
                    newPage.Height = 3508;//842
                    newPage.Width = 2480;
                    graph = XGraphics.FromPdfPage(newPage);
                    tf = new XTextFormatter(graph);
                    x = -1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                }

                graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                tf.DrawString(String.Format("{0:n0}", totalMonthlyAmount) + " " + currencyCode, fontParagraph, XBrushes.Black,
                              new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);


            

            tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
            new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

            var fileName = "attachment_" + System.Guid.NewGuid().ToString() + ".pdf";

            var path = Directory.GetCurrentDirectory() + @"\attachments\" + fileName;

            string fileURL = "https://fastfillpro.developitech.com/attachments/" + fileName;

            document.Save(path);

            var response = new
            {
                URL = fileURL,
            };

            return Ok(response);
        }

        [HttpPost("GetMonthlyCompanyPaymentTransactionsPDF")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMonthlyCompanyPaymentTransactionsPDF([FromBody] List<MonthlyPaymentTransactionDto> MonthlyPaymentTransactions)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            User user = await _userServices.GetUserById(userId, 0);

            List<MonthlyPaymentTransactionListDto> monthlyPaymentTransactionList = new List<MonthlyPaymentTransactionListDto>();

            foreach (var item in MonthlyPaymentTransactions)
            {
                MonthlyPaymentTransactionListDto monthlyPaymentTransactionListDto = new MonthlyPaymentTransactionListDto();
                monthlyPaymentTransactionListDto.monthlyPaymentTransactionDto = item;

                int mDays = DateTime.DaysInMonth(item.Year, item.Month);
                DateTime fromDate = new DateTime(item.Year, item.Month, 1);
                DateTime toDate = new DateTime(item.Year, item.Month, mDays);

                monthlyPaymentTransactionListDto.paymentTransactions = await _companyServices.GetCompanyPaymentTransactionsPDF(userId, true, fromDate, toDate);
                if (monthlyPaymentTransactionListDto.paymentTransactions.Count() > 0)
                    monthlyPaymentTransactionList.Add(monthlyPaymentTransactionListDto);                
            }
           
            string title = "Transactions Details Report";
            string dateTitle = "Date";
            string mobileNumberTitle = "Customer NO.";
            string userTitle = "Name";
            string amountTitle = "Amount";
            string currencyCode = "SDG";

            if (user.Language == 2)
            {
                title = " تقرير تفصيلي بالعمليات ".ArabicWithFontGlyphsToPfd();
                dateTitle = " التاريخ ".ArabicWithFontGlyphsToPfd();
                mobileNumberTitle = " رقم العميل ".ArabicWithFontGlyphsToPfd();
                userTitle = " اسم العميل ".ArabicWithFontGlyphsToPfd();
                amountTitle = " القيمة ".ArabicWithFontGlyphsToPfd();
                currencyCode = " ج.س ".ArabicWithFontGlyphsToPfd();
            }

            PdfDocument document = new PdfDocument();
            // Page Options
            PdfPage pdfPage = document.AddPage();
            pdfPage.Height = 3508;//842
            pdfPage.Width = 2480;
            
            // Get an XGraphics object for drawing
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);

            // Text format
            XStringFormat format = new XStringFormat();
            format.LineAlignment = XLineAlignment.Near;
            format.Alignment = XStringAlignment.Near;
            var tf = new XTextFormatter(graph);

            XFont fontParagraph = new XFont("Arial", 40, XFontStyle.Regular);

            // Row elements
            int el1_width = 150;
            int el2_width = 516;
            int el3_width = 316;
            int el4_width = 682;
            int el5_width = 416;


            // page structure options
            double lineHeight = 100;
            int marginLeft = 200;
            int marginTop = 200;

            int el_height = 100;
            int rect_height = 100;

            int interLine_X_1 = 2;
            int interLine_X_2 = 2 * interLine_X_1;
            int interLine_X_3 = 3 * interLine_X_1;
            int interLine_X_4 = 4 * interLine_X_1;
            int interLine_X_5 = 5 * interLine_X_1;


            int offSetX_1 = el1_width;
            int offSetX_2 = el1_width + el2_width;
            int offSetX_3 = el1_width + el2_width + el3_width;
            int offSetX_4 = el1_width + el2_width + el3_width + el4_width;
            int offSetX_5 = el1_width + el2_width + el3_width + el4_width + el5_width;

            XSolidBrush rect_style1 = new XSolidBrush(XColors.LightGray);
            
            XSolidBrush rect_style2 = new XSolidBrush(XColor.FromArgb(255, 220, 189));

            XPen rect_style2_pen = new XPen(XColors.Black);

            XSolidBrush rect_style3 = new XSolidBrush(XColor.FromArgb(209, 207, 142));
            int i = -1;
            int x = -1;

            var logoPath = Directory.GetCurrentDirectory() + @"\attachments\logo.png";
            XImage image = XImage.FromFile(logoPath);

            graph.DrawImage(image, 1140, 10, 200, 200);


            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

            tf.DrawString(title, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20, marginTop + 10, pdfPage.Width - 2 * marginLeft, el_height), format);

            x = x + 1;
            i = i + 1;
            double dist_Y = lineHeight * (x + 1);
            double dist_Y2 = dist_Y - 2;
            int pageNo = 1;
            int line = 0;
            //graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, (marginLeft + 20 + offSetX_4 + 2 * interLine_X_4) + el5_width, rect_height);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el1_width, rect_height);
            tf.DrawString("#", fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
            tf.DrawString(dateTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
            tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
            tf.DrawString(userTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
            tf.DrawString(amountTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);

            double totalMonthlyAmount = 0;

            foreach (var month in monthlyPaymentTransactionList)
            {

                line = 0;
                i = i + 1;
                x = x + 1;

                dist_Y = lineHeight * (x + 1);
                dist_Y2 = dist_Y - 2;

                if ((marginTop + dist_Y) > 3250)
                {
                    tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

                    pageNo = pageNo + 1;

                    PdfPage newPage = document.AddPage();
                    newPage.Height = 3508;//842
                    newPage.Width = 2480;
                    graph = XGraphics.FromPdfPage(newPage);
                    tf = new XTextFormatter(graph);
                    x = -1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                    logoPath = Directory.GetCurrentDirectory() + @"\attachments\logo.png";
                    image = XImage.FromFile(logoPath);

                    graph.DrawImage(image, 1140, 10, 200, 200);


                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                    tf.DrawString(title, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft, marginTop + 10, pdfPage.Width - 2 * marginLeft, el_height), format);

                    x = x + 1;
                    i = i + 1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el5_width, rect_height);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el1_width, rect_height);
                    tf.DrawString("#", fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
                    tf.DrawString(dateTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
                    tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
                    tf.DrawString(userTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
                    tf.DrawString(amountTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);


                    x = x + 1;
                    i = i + 1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;


                }


                graph.DrawRectangle(rect_style2_pen, rect_style3, marginLeft, marginTop + dist_Y, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                tf.DrawString(month.monthlyPaymentTransactionDto.Month.ToString()+'/'+ month.monthlyPaymentTransactionDto.Year.ToString(), fontParagraph, XBrushes.Black,
                              new XRect(marginLeft + 20, marginTop + dist_Y + 10, pdfPage.Width - 2 * marginLeft, el_height), format);


                totalMonthlyAmount = 0;
                foreach (var item in month.paymentTransactions)
                {
                    line = line + 1;
                    totalMonthlyAmount = totalMonthlyAmount + (item.Amount - item.Fastfill);

                    i = i + 1;
                    x = x + 1;

                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                    if ((marginTop + dist_Y) > 3250)
                    {
            
                        tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
                          new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

                        pageNo = pageNo + 1;

                        PdfPage newPage = document.AddPage();
                        newPage.Height = 3508;//842
                        newPage.Width = 2480;
                        graph = XGraphics.FromPdfPage(newPage);
                        tf = new XTextFormatter(graph);
                        x = -1;
                        dist_Y = lineHeight * (x + 1);
                        dist_Y2 = dist_Y - 2;

                        logoPath = Directory.GetCurrentDirectory() + @"\attachments\logo.png";
                        image = XImage.FromFile(logoPath);

                        graph.DrawImage(image, 1140, 10, 200, 200);


                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                        tf.DrawString(title, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20, marginTop + 10, pdfPage.Width - 2 * marginLeft, el_height), format);

                        x = x + 1;
                        i = i + 1;
                        dist_Y = lineHeight * (x + 1);
                        dist_Y2 = dist_Y - 2;

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el5_width, rect_height);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el1_width, rect_height);
                        tf.DrawString("#", fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height), format);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
                        tf.DrawString(dateTitle, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height), format);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
                        tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height), format);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
                        tf.DrawString(userTitle, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height), format);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
                        tf.DrawString(amountTitle, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);


                        x = x + 1;
                        i = i + 1;
                        dist_Y = lineHeight * (x + 1);
                        dist_Y2 = dist_Y - 2;

                    }
                    //dist_Y = lineHeight * (x);
                    //dist_Y2 = dist_Y - 2;


                    // header della G
                    /*if (i == 2)
                    {
                        graph.DrawRectangle(rect_style2, marginLeft, marginTop, pdfPage.Width - 2 * marginLeft, rect_height);

                        tf.DrawString("#", fontParagraph, XBrushes.White,
                                      new XRect(marginLeft, marginTop, el1_width, el_height), format);

                        tf.DrawString(dateTitle, fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_1 + interLine_X_1, marginTop, el2_width, el_height), format);

                        tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_2 + 2 * interLine_X_2, marginTop, el1_width, el_height), format);

                        tf.DrawString(userTitle, fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_3 + 2 * interLine_X_3, marginTop, el1_width, el_height), format);

                        tf.DrawString(amountTitle, fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_4 + 2 * interLine_X_4, marginTop, el1_width, el_height), format);


                    }
                    else */
                    {

                        //if (i % 2 == 1)
                        //{
                        //  graph.DrawRectangle(TextBackgroundBrush, marginLeft, lineY - 2 + marginTop, pdfPage.Width - marginLeft - marginRight, lineHeight - 2);
                        //}

                        //ELEMENT 1 - SMALL 80
                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft, marginTop + dist_Y, el1_width, rect_height);
                        tf.DrawString(

                            line.ToString(),
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height),
                            format);

                        //ELEMENT 2 - BIG 380
                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
                        tf.DrawString(
                            item.Date.ToString("yyyy-MM-dd hh:mm tt"),
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height),
                            format);


                        //ELEMENT 3 - SMALL 80

                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
                        tf.DrawString(
                            item.User.Id.ToString(),
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height),
                            format);

                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
                        tf.DrawString(
                            item.User.FirstName,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height),
                            format);


                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
                        tf.DrawString(
                            String.Format("{0:n0}", item.Amount - item.Fastfill)+ " " + currencyCode,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height),
                            format);

                    }

                }


                i = i + 1;
                x = x + 1;

                dist_Y = lineHeight * (x + 1);
                dist_Y2 = dist_Y - 2;


                if ((marginTop + dist_Y) > 3250)
                {
                    tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

                    pageNo = pageNo + 1;

                    PdfPage newPage = document.AddPage();
                    newPage.Height = 3508;//842
                    newPage.Width = 2480;
                    graph = XGraphics.FromPdfPage(newPage);
                    tf = new XTextFormatter(graph);
                    x = -1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                }

                graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                tf.DrawString(String.Format("{0:n0}", totalMonthlyAmount) + " " + currencyCode, fontParagraph, XBrushes.Black,
                              new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);


            }

            tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
            new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

            var fileName = "attachment_" + System.Guid.NewGuid().ToString() + ".pdf";

            var path = Directory.GetCurrentDirectory() + @"\attachments\" + fileName;

            string fileURL = "https://fastfillpro.developitech.com/attachments/" + fileName;

            document.Save(path);

            var response = new
            {
                URL = fileURL,
            };

            return Ok(response);
        }

        // POST: api/company/InsertCompany
        [HttpPost("CompanyPump")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertCompanyPump([FromBody] InsertCompanyPumpDto insertCompanyPumpDto)
        {
            if (insertCompanyPumpDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            CompanyPump mappedCompanyPump = new CompanyPump();
            mappedCompanyPump.CompanyId = insertCompanyPumpDto.CompanyId;
            mappedCompanyPump.Code = insertCompanyPumpDto.Code;

            bool res = await _companyServices.AddCompanyPump(mappedCompanyPump);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.ADDED_SUCCESSFULLY);
        }

        [HttpPut("CompanyPump")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCompanyPump([FromBody] InsertCompanyPumpDto insertCompanyPumpDto)
        {
            if (insertCompanyPumpDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            CompanyPump mappedCompanyPump = new CompanyPump();
            mappedCompanyPump.CompanyId = insertCompanyPumpDto.CompanyId;
            mappedCompanyPump.Code = insertCompanyPumpDto.Code;
            mappedCompanyPump.Id = insertCompanyPumpDto.Id;

            bool res = await _companyServices.UpdateCompanyPump(mappedCompanyPump);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }


        [HttpDelete("CompanyPump")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCompanyPump([FromBody] InsertCompanyPumpDto insertCompanyPumpDto)
        {
            if (insertCompanyPumpDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            CompanyPump mappedCompanyPump = new CompanyPump();
            mappedCompanyPump.CompanyId = insertCompanyPumpDto.CompanyId;
            mappedCompanyPump.Code = insertCompanyPumpDto.Code;
            mappedCompanyPump.Id = insertCompanyPumpDto.Id;

            bool res = await _companyServices.RemoveCompanyPump(mappedCompanyPump);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.DELETED_SUCCESSFULLY);
        }


        // GET: api/company/FrequentlyVisitedCompanies
        [HttpGet("GetFastFillFees")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFastFillFees()
        {                     
            return Ok(100.0);
        }


        // GET: api/company/AllCompanies
        [HttpGet("AllCompanies")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCompanies(int page = 1, int pageSize = 10)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<Company> companies = await _companyServices.GetAllCompanies(page, pageSize, paginationInfo, userId);
            IEnumerable<Company> company = _mapper.Map<IEnumerable<Company>>(companies);

            var response = new
            {
                Companies = company,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }


        // POST: api/company/InsertCompany
        [HttpPost("InsertGroup")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertGroup([FromBody] UpdateGroupDto updateGroupDto)
        {
            if (updateGroupDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            Group mappedGroup = new Group();
            mappedGroup.ArabicName = updateGroupDto.ArabicName;
            mappedGroup.EnglishName = updateGroupDto.EnglishName;

            bool res = await _companyServices.AddGroup(mappedGroup);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.ADDED_SUCCESSFULLY);
        }


        // Put: api/company/UpdateCompany
        [HttpPut("UpdateGroup")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateGroup([FromBody] UpdateGroupDto updateGroupDto)
        {
            if (updateGroupDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            Group mappedGroup = new Group();
            mappedGroup.Id = updateGroupDto.Id;
            mappedGroup.ArabicName = updateGroupDto.ArabicName;
            mappedGroup.EnglishName = updateGroupDto.EnglishName;

            bool res = await _companyServices.UpdateGroup(mappedGroup);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

        // Delete: api/company/DeleteCompany
        [HttpDelete("DeleteGroup/{id}")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            Group group = await _companyServices.GetGroupById(id);

            if (group == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            bool res = await _companyServices.DeleteGroup(group);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.DELETED_SUCCESSFULLY);
        }

        // GET: api/company/AllCompanies
        [HttpGet("AllGroups")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllGroups(int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<Group> groups = await _companyServices.GetAllGroups(page, pageSize, paginationInfo);
            IEnumerable<Group> group = _mapper.Map<IEnumerable<Group>>(groups);

            var response = new
            {
                Groups = group,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // GET: api/company/GroupsByName
        [HttpGet("GroupsByName/{name}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGroupsByName(int page = 1, int pageSize = 10, string name = "")
        {
            if (name.Trim() == "")
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<Group> groups = await _companyServices.SearchGroupsByName(page, pageSize, paginationInfo, name);
            IEnumerable<Group> group = _mapper.Map<IEnumerable<Group>>(groups);

            var response = new
            {
                Groups = group,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }


        // GET: api/company/AllCompanies
        [HttpGet("AllCompaniesByGroup")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCompaniesByGroup(bool filterByDate, DateTime filterFromDate, DateTime filterToDate, int page = 1, int pageSize = 10)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            PaginationInfo paginationInfo = new PaginationInfo();
            Totals totals = new Totals();
            IEnumerable<Company> companies = await _companyServices.GetAllCompaniesByGroup(page, pageSize, paginationInfo, userId, totals, filterByDate, filterFromDate, filterToDate);
            IEnumerable<Company> company = _mapper.Map<IEnumerable<Company>>(companies);

            List<CompanyWithTransactionsTotal> companiesWithTransactionsTotal = new List<CompanyWithTransactionsTotal>();
            foreach (var c in company)
            {
                TotalPaymentTransaction companyTotalPaymentTransactions = await _companyServices.GetTotalCompanyPaymentTransactionsByCompanyId(userId, filterByDate, filterFromDate, filterToDate, c.Id);
                CompanyWithTransactionsTotal companyWithTransactionsTotal = new CompanyWithTransactionsTotal();
                
                var s = JsonConvert.SerializeObject(c);
                CompanyWithTransactionsTotal cwtt = JsonConvert.DeserializeObject<CompanyWithTransactionsTotal>(s);
                cwtt.Count = companyTotalPaymentTransactions.count;
                cwtt.Amount = companyTotalPaymentTransactions.amount;
                companiesWithTransactionsTotal.Add(cwtt);              
            }


            var response = new
            {
                Companies = companiesWithTransactionsTotal,
                PaginationInfo = paginationInfo,
                TotalCount = totals.Count,
                TotalAmount = totals.Amount
            };

            return Ok(response);
        }


        // GET: api/company/FrequentlyVisitedCompaniesBranches
        [HttpGet("GetStationPaymentTransactionsByCompanyId")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyPaymentTransactionsByCompanyId(bool filterByDate, DateTime filterFromDate, DateTime filterToDate, int companyId, int page = 1, int pageSize = 10)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<PaymentTransaction> companyPaymentTransactions = await _companyServices.GetCompanyPaymentTransactionsByCompanyId(page, pageSize, paginationInfo, companyId, filterByDate, filterFromDate, filterToDate);

            var response = new
            {
                StationPaymentTransactions = companyPaymentTransactions,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        [HttpGet("GetMonthlyStationPaymentTransactionsByCompanyId")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMonthlyStationPaymentTransactionsByCompanyId(int companyId, int page = 1, int pageSize = 10)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<MonthlyPaymentTransaction> monthlyCompanyPaymentTransactions = await _companyServices.GetMonthlyCompanyPaymentTransactionsByCompanyId(page, pageSize, paginationInfo, userId, companyId);

            var response = new
            {
                MonthlyStationPaymentTransactions = monthlyCompanyPaymentTransactions,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }


        [HttpGet("GetTotalStationPaymentTransactionsByCompanyId")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalCompanyPaymentTransactionsByCompanyId(bool filterByDate, DateTime filterFromDate, DateTime filterToDate, int companyId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            TotalPaymentTransaction companyTotalPaymentTransactions = await _companyServices.GetTotalCompanyPaymentTransactionsByCompanyId(userId, filterByDate, filterFromDate, filterToDate, companyId);

            return Ok(companyTotalPaymentTransactions);
        }

        // GET: api/company/FrequentlyVisitedCompaniesBranches
        [HttpGet("GetStationPaymentTransactionsPDFByCompanyId")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyPaymentTransactionsPDFByCompanyId(bool filterByDate, DateTime filterFromDate, DateTime filterToDate, int companyId)

        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            User user = await _userServices.GetUserById(userId, 0);

            DateTime fromDate = filterFromDate;
            DateTime toDate = filterToDate;

            var paymentTransactions = await _companyServices.GetCompanyPaymentTransactionsPDFByCompanyId(userId, true, fromDate, toDate, companyId);

            string title = "Transactions Details Report";
            string dateTitle = "Date";
            string mobileNumberTitle = "Customer NO.";
            string userTitle = "Name";
            string amountTitle = "Amount";
            string currencyCode = "SDG";
            string fromDateTitle = "From: ";
            string toDateTitle = "To: ";

            if (user.Language == 2)
            {
                title = " تقرير تفصيلي بالعمليات ".ArabicWithFontGlyphsToPfd();
                dateTitle = " التاريخ ".ArabicWithFontGlyphsToPfd(); ;
                mobileNumberTitle = " رقم العميل ".ArabicWithFontGlyphsToPfd(); ;
                userTitle = " اسم العميل ".ArabicWithFontGlyphsToPfd(); ;
                amountTitle = " القيمة ".ArabicWithFontGlyphsToPfd(); ;
                currencyCode = " ج.س ".ArabicWithFontGlyphsToPfd(); ;
                fromDateTitle = "من : ".ArabicWithFontGlyphsToPfd(); ;
                toDateTitle = "إلى : ".ArabicWithFontGlyphsToPfd(); ;
            }

            PdfDocument document = new PdfDocument();
            // Page Options
            PdfPage pdfPage = document.AddPage();
            pdfPage.Height = 3508;//842
            pdfPage.Width = 2480;

            // Get an XGraphics object for drawing
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);

            // Text format
            XStringFormat format = new XStringFormat();
            format.LineAlignment = XLineAlignment.Near;
            format.Alignment = XStringAlignment.Near;
            var tf = new XTextFormatter(graph);

            XFont fontParagraph = new XFont("Arial", 40, XFontStyle.Regular);

            // Row elements
            int el1_width = 150;
            int el2_width = 516;
            int el3_width = 316;
            int el4_width = 682;
            int el5_width = 416;


            // page structure options
            double lineHeight = 100;
            int marginLeft = 200;
            int marginTop = 200;

            int el_height = 100;
            int rect_height = 100;

            int interLine_X_1 = 2;
            int interLine_X_2 = 2 * interLine_X_1;
            int interLine_X_3 = 3 * interLine_X_1;
            int interLine_X_4 = 4 * interLine_X_1;
            int interLine_X_5 = 5 * interLine_X_1;


            int offSetX_1 = el1_width;
            int offSetX_2 = el1_width + el2_width;
            int offSetX_3 = el1_width + el2_width + el3_width;
            int offSetX_4 = el1_width + el2_width + el3_width + el4_width;
            int offSetX_5 = el1_width + el2_width + el3_width + el4_width + el5_width;

            XSolidBrush rect_style1 = new XSolidBrush(XColors.LightGray);

            XSolidBrush rect_style2 = new XSolidBrush(XColor.FromArgb(255, 220, 189));

            XPen rect_style2_pen = new XPen(XColors.Black);

            XSolidBrush rect_style3 = new XSolidBrush(XColor.FromArgb(209, 207, 142));
            int i = -1;
            int x = -1;

            var logoPath = Directory.GetCurrentDirectory() + @"\attachments\logo.png";
            XImage image = XImage.FromFile(logoPath);

            graph.DrawImage(image, 1140, 10, 200, 200);


            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

            tf.DrawString(title, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20, marginTop + 10, pdfPage.Width - 2 * marginLeft, el_height), format);

            x = x + 1;
            i = i + 1;
            double dist_Y = lineHeight * (x + 1);
            double dist_Y2 = dist_Y - 2;
            int pageNo = 1;
            int line = 0;
            //graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, (marginLeft + 20 + offSetX_4 + 2 * interLine_X_4) + el5_width, rect_height);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el1_width, rect_height);
            tf.DrawString("#", fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
            tf.DrawString(dateTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
            tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
            tf.DrawString(userTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
            tf.DrawString(amountTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);

            double totalMonthlyAmount = 0;

            line = 0;
            i = i + 1;
            x = x + 1;

            dist_Y = lineHeight * (x + 1);
            dist_Y2 = dist_Y - 2;

            if ((marginTop + dist_Y) > 3250)
            {
                tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
                              new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

                pageNo = pageNo + 1;

                PdfPage newPage = document.AddPage();
                newPage.Height = 3508;//842
                newPage.Width = 2480;
                graph = XGraphics.FromPdfPage(newPage);
                tf = new XTextFormatter(graph);
                x = -1;
                dist_Y = lineHeight * (x + 1);
                dist_Y2 = dist_Y - 2;

                logoPath = Directory.GetCurrentDirectory() + @"\attachments\logo.png";
                image = XImage.FromFile(logoPath);

                graph.DrawImage(image, 1090, 10, 300, 300);


                graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                tf.DrawString(title, fontParagraph, XBrushes.Black,
                              new XRect(marginLeft, marginTop + 10, pdfPage.Width - 2 * marginLeft, el_height), format);

                x = x + 1;
                i = i + 1;
                dist_Y = lineHeight * (x + 1);
                dist_Y2 = dist_Y - 2;

                graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el5_width, rect_height);

                graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el1_width, rect_height);
                tf.DrawString("#", fontParagraph, XBrushes.Black,
                              new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height), format);

                graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
                tf.DrawString(dateTitle, fontParagraph, XBrushes.Black,
                              new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height), format);

                graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
                tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.Black,
                              new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height), format);

                graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
                tf.DrawString(userTitle, fontParagraph, XBrushes.Black,
                              new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height), format);

                graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
                tf.DrawString(amountTitle, fontParagraph, XBrushes.Black,
                              new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);


                x = x + 1;
                i = i + 1;
                dist_Y = lineHeight * (x + 1);
                dist_Y2 = dist_Y - 2;


            }


            graph.DrawRectangle(rect_style2_pen, rect_style3, marginLeft, marginTop + dist_Y, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

            tf.DrawString(fromDateTitle + " " + fromDate.ToString("dd-MM-yyyy") + " " + toDateTitle + " " + toDate.ToString("dd-MM-yyyy"), fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20, marginTop + dist_Y + 10, pdfPage.Width - 2 * marginLeft, el_height), format);


            totalMonthlyAmount = 0;
            foreach (var item in paymentTransactions)
            {
                line = line + 1;
                totalMonthlyAmount = totalMonthlyAmount + (item.Amount - item.Fastfill);

                i = i + 1;
                x = x + 1;

                dist_Y = lineHeight * (x + 1);
                dist_Y2 = dist_Y - 2;

                if ((marginTop + dist_Y) > 3250)
                {

                    tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
                      new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

                    pageNo = pageNo + 1;

                    PdfPage newPage = document.AddPage();
                    newPage.Height = 3508;//842
                    newPage.Width = 2480;
                    graph = XGraphics.FromPdfPage(newPage);
                    tf = new XTextFormatter(graph);
                    x = -1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                    logoPath = Directory.GetCurrentDirectory() + @"\attachments\logo.png";
                    image = XImage.FromFile(logoPath);

                    graph.DrawImage(image, 1090, 10, 300, 300);


                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                    tf.DrawString(title, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20, marginTop + 10, pdfPage.Width - 2 * marginLeft, el_height), format);

                    x = x + 1;
                    i = i + 1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el5_width, rect_height);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el1_width, rect_height);
                    tf.DrawString("#", fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
                    tf.DrawString(dateTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
                    tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
                    tf.DrawString(userTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
                    tf.DrawString(amountTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);


                    x = x + 1;
                    i = i + 1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                }
                //dist_Y = lineHeight * (x);
                //dist_Y2 = dist_Y - 2;


                // header della G
                /*if (i == 2)
                {
                    graph.DrawRectangle(rect_style2, marginLeft, marginTop, pdfPage.Width - 2 * marginLeft, rect_height);

                    tf.DrawString("#", fontParagraph, XBrushes.White,
                                  new XRect(marginLeft, marginTop, el1_width, el_height), format);

                    tf.DrawString(dateTitle, fontParagraph, XBrushes.White,
                                  new XRect(marginLeft + offSetX_1 + interLine_X_1, marginTop, el2_width, el_height), format);

                    tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.White,
                                  new XRect(marginLeft + offSetX_2 + 2 * interLine_X_2, marginTop, el1_width, el_height), format);

                    tf.DrawString(userTitle, fontParagraph, XBrushes.White,
                                  new XRect(marginLeft + offSetX_3 + 2 * interLine_X_3, marginTop, el1_width, el_height), format);

                    tf.DrawString(amountTitle, fontParagraph, XBrushes.White,
                                  new XRect(marginLeft + offSetX_4 + 2 * interLine_X_4, marginTop, el1_width, el_height), format);


                }
                else */
                {

                    //if (i % 2 == 1)
                    //{
                    //  graph.DrawRectangle(TextBackgroundBrush, marginLeft, lineY - 2 + marginTop, pdfPage.Width - marginLeft - marginRight, lineHeight - 2);
                    //}

                    //ELEMENT 1 - SMALL 80
                    graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft, marginTop + dist_Y, el1_width, rect_height);
                    tf.DrawString(

                        line.ToString(),
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height),
                        format);

                    //ELEMENT 2 - BIG 380
                    graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
                    tf.DrawString(
                        item.Date.ToString("yyyy-MM-dd hh:mm tt"),
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height),
                        format);


                    //ELEMENT 3 - SMALL 80

                    graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
                    tf.DrawString(
                        item.User.Id.ToString(),
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height),
                        format);

                    graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
                    tf.DrawString(
                        item.User.FirstName,
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height),
                        format);


                    graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
                    tf.DrawString(
                        String.Format("{0:n0}", item.Amount - item.Fastfill) + " " + currencyCode,
                        fontParagraph,
                        XBrushes.Black,
                        new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height),
                        format);

                }

            }


            i = i + 1;
            x = x + 1;

            dist_Y = lineHeight * (x + 1);
            dist_Y2 = dist_Y - 2;


            if ((marginTop + dist_Y) > 3250)
            {
                tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
                              new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

                pageNo = pageNo + 1;

                PdfPage newPage = document.AddPage();
                newPage.Height = 3508;//842
                newPage.Width = 2480;
                graph = XGraphics.FromPdfPage(newPage);
                tf = new XTextFormatter(graph);
                x = -1;
                dist_Y = lineHeight * (x + 1);
                dist_Y2 = dist_Y - 2;

            }

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

            tf.DrawString(String.Format("{0:n0}", totalMonthlyAmount) + " " + currencyCode, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);




            tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
            new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

            var fileName = "attachment_" + System.Guid.NewGuid().ToString() + ".pdf";

            var path = Directory.GetCurrentDirectory() + @"\attachments\" + fileName;

            string fileURL = "https://fastfillpro.developitech.com/attachments/" + fileName;

            document.Save(path);

            var response = new
            {
                URL = fileURL,
            };

            return Ok(response);
        }

        [HttpPost("GetMonthlyCompanyPaymentTransactionsPDFByCompanyId")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMonthlyCompanyPaymentTransactionsPDFByCompanyId([FromBody] List<MonthlyPaymentTransactionDto> MonthlyPaymentTransactions, int companyId)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            User user = await _userServices.GetUserById(userId, 0);

            List<MonthlyPaymentTransactionListDto> monthlyPaymentTransactionList = new List<MonthlyPaymentTransactionListDto>();

            foreach (var item in MonthlyPaymentTransactions)
            {
                MonthlyPaymentTransactionListDto monthlyPaymentTransactionListDto = new MonthlyPaymentTransactionListDto();
                monthlyPaymentTransactionListDto.monthlyPaymentTransactionDto = item;

                int mDays = DateTime.DaysInMonth(item.Year, item.Month);
                DateTime fromDate = new DateTime(item.Year, item.Month, 1);
                DateTime toDate = new DateTime(item.Year, item.Month, mDays);

                monthlyPaymentTransactionListDto.paymentTransactions = await _companyServices.GetCompanyPaymentTransactionsPDFByCompanyId(userId, true, fromDate, toDate, companyId);
                if (monthlyPaymentTransactionListDto.paymentTransactions.Count() > 0)
                    monthlyPaymentTransactionList.Add(monthlyPaymentTransactionListDto);
            }

            string title = "Transactions Details Report";
            string dateTitle = "Date";
            string mobileNumberTitle = "Customer NO.";
            string userTitle = "Name";
            string amountTitle = "Amount";
            string currencyCode = "SDG";

            if (user.Language == 2)
            {
                title = " تقرير تفصيلي بالعمليات ".ArabicWithFontGlyphsToPfd();
                dateTitle = " التاريخ ".ArabicWithFontGlyphsToPfd();
                mobileNumberTitle = " رقم العميل ".ArabicWithFontGlyphsToPfd();
                userTitle = " اسم العميل ".ArabicWithFontGlyphsToPfd();
                amountTitle = " القيمة ".ArabicWithFontGlyphsToPfd();
                currencyCode = " ج.س ".ArabicWithFontGlyphsToPfd();
            }

            PdfDocument document = new PdfDocument();
            // Page Options
            PdfPage pdfPage = document.AddPage();
            pdfPage.Height = 3508;//842
            pdfPage.Width = 2480;

            // Get an XGraphics object for drawing
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);

            // Text format
            XStringFormat format = new XStringFormat();
            format.LineAlignment = XLineAlignment.Near;
            format.Alignment = XStringAlignment.Near;
            var tf = new XTextFormatter(graph);

            XFont fontParagraph = new XFont("Arial", 40, XFontStyle.Regular);

            // Row elements
            int el1_width = 150;
            int el2_width = 516;
            int el3_width = 316;
            int el4_width = 682;
            int el5_width = 416;


            // page structure options
            double lineHeight = 100;
            int marginLeft = 200;
            int marginTop = 200;

            int el_height = 100;
            int rect_height = 100;

            int interLine_X_1 = 2;
            int interLine_X_2 = 2 * interLine_X_1;
            int interLine_X_3 = 3 * interLine_X_1;
            int interLine_X_4 = 4 * interLine_X_1;
            int interLine_X_5 = 5 * interLine_X_1;


            int offSetX_1 = el1_width;
            int offSetX_2 = el1_width + el2_width;
            int offSetX_3 = el1_width + el2_width + el3_width;
            int offSetX_4 = el1_width + el2_width + el3_width + el4_width;
            int offSetX_5 = el1_width + el2_width + el3_width + el4_width + el5_width;

            XSolidBrush rect_style1 = new XSolidBrush(XColors.LightGray);

            XSolidBrush rect_style2 = new XSolidBrush(XColor.FromArgb(255, 220, 189));

            XPen rect_style2_pen = new XPen(XColors.Black);

            XSolidBrush rect_style3 = new XSolidBrush(XColor.FromArgb(209, 207, 142));
            int i = -1;
            int x = -1;

            var logoPath = Directory.GetCurrentDirectory() + @"\attachments\logo.png";
            XImage image = XImage.FromFile(logoPath);

            graph.DrawImage(image, 1140, 10, 200, 200);


            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

            tf.DrawString(title, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20, marginTop + 10, pdfPage.Width - 2 * marginLeft, el_height), format);

            x = x + 1;
            i = i + 1;
            double dist_Y = lineHeight * (x + 1);
            double dist_Y2 = dist_Y - 2;
            int pageNo = 1;
            int line = 0;
            //graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, (marginLeft + 20 + offSetX_4 + 2 * interLine_X_4) + el5_width, rect_height);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el1_width, rect_height);
            tf.DrawString("#", fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
            tf.DrawString(dateTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
            tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
            tf.DrawString(userTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height), format);

            graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
            tf.DrawString(amountTitle, fontParagraph, XBrushes.Black,
                          new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);

            double totalMonthlyAmount = 0;

            foreach (var month in monthlyPaymentTransactionList)
            {

                line = 0;
                i = i + 1;
                x = x + 1;

                dist_Y = lineHeight * (x + 1);
                dist_Y2 = dist_Y - 2;

                if ((marginTop + dist_Y) > 3250)
                {
                    tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

                    pageNo = pageNo + 1;

                    PdfPage newPage = document.AddPage();
                    newPage.Height = 3508;//842
                    newPage.Width = 2480;
                    graph = XGraphics.FromPdfPage(newPage);
                    tf = new XTextFormatter(graph);
                    x = -1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                    logoPath = Directory.GetCurrentDirectory() + @"\attachments\logo.png";
                    image = XImage.FromFile(logoPath);

                    graph.DrawImage(image, 1140, 10, 200, 200);


                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                    tf.DrawString(title, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft, marginTop + 10, pdfPage.Width - 2 * marginLeft, el_height), format);

                    x = x + 1;
                    i = i + 1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el5_width, rect_height);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el1_width, rect_height);
                    tf.DrawString("#", fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
                    tf.DrawString(dateTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
                    tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
                    tf.DrawString(userTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height), format);

                    graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
                    tf.DrawString(amountTitle, fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);


                    x = x + 1;
                    i = i + 1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;


                }


                graph.DrawRectangle(rect_style2_pen, rect_style3, marginLeft, marginTop + dist_Y, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                tf.DrawString(month.monthlyPaymentTransactionDto.Month.ToString() + '/' + month.monthlyPaymentTransactionDto.Year.ToString(), fontParagraph, XBrushes.Black,
                              new XRect(marginLeft + 20, marginTop + dist_Y + 10, pdfPage.Width - 2 * marginLeft, el_height), format);


                totalMonthlyAmount = 0;
                foreach (var item in month.paymentTransactions)
                {
                    line = line + 1;
                    totalMonthlyAmount = totalMonthlyAmount + (item.Amount - item.Fastfill);

                    i = i + 1;
                    x = x + 1;

                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                    if ((marginTop + dist_Y) > 3250)
                    {

                        tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
                          new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

                        pageNo = pageNo + 1;

                        PdfPage newPage = document.AddPage();
                        newPage.Height = 3508;//842
                        newPage.Width = 2480;
                        graph = XGraphics.FromPdfPage(newPage);
                        tf = new XTextFormatter(graph);
                        x = -1;
                        dist_Y = lineHeight * (x + 1);
                        dist_Y2 = dist_Y - 2;

                        logoPath = Directory.GetCurrentDirectory() + @"\attachments\logo.png";
                        image = XImage.FromFile(logoPath);

                        graph.DrawImage(image, 1140, 10, 200, 200);


                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                        tf.DrawString(title, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20, marginTop + 10, pdfPage.Width - 2 * marginLeft, el_height), format);

                        x = x + 1;
                        i = i + 1;
                        dist_Y = lineHeight * (x + 1);
                        dist_Y2 = dist_Y - 2;

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el5_width, rect_height);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, el1_width, rect_height);
                        tf.DrawString("#", fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height), format);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
                        tf.DrawString(dateTitle, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height), format);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
                        tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height), format);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
                        tf.DrawString(userTitle, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height), format);

                        graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
                        tf.DrawString(amountTitle, fontParagraph, XBrushes.Black,
                                      new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);


                        x = x + 1;
                        i = i + 1;
                        dist_Y = lineHeight * (x + 1);
                        dist_Y2 = dist_Y - 2;

                    }
                    //dist_Y = lineHeight * (x);
                    //dist_Y2 = dist_Y - 2;


                    // header della G
                    /*if (i == 2)
                    {
                        graph.DrawRectangle(rect_style2, marginLeft, marginTop, pdfPage.Width - 2 * marginLeft, rect_height);

                        tf.DrawString("#", fontParagraph, XBrushes.White,
                                      new XRect(marginLeft, marginTop, el1_width, el_height), format);

                        tf.DrawString(dateTitle, fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_1 + interLine_X_1, marginTop, el2_width, el_height), format);

                        tf.DrawString(mobileNumberTitle, fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_2 + 2 * interLine_X_2, marginTop, el1_width, el_height), format);

                        tf.DrawString(userTitle, fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_3 + 2 * interLine_X_3, marginTop, el1_width, el_height), format);

                        tf.DrawString(amountTitle, fontParagraph, XBrushes.White,
                                      new XRect(marginLeft + offSetX_4 + 2 * interLine_X_4, marginTop, el1_width, el_height), format);


                    }
                    else */
                    {

                        //if (i % 2 == 1)
                        //{
                        //  graph.DrawRectangle(TextBackgroundBrush, marginLeft, lineY - 2 + marginTop, pdfPage.Width - marginLeft - marginRight, lineHeight - 2);
                        //}

                        //ELEMENT 1 - SMALL 80
                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft, marginTop + dist_Y, el1_width, rect_height);
                        tf.DrawString(

                            line.ToString(),
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20, marginTop + dist_Y + 10, el1_width, el_height),
                            format);

                        //ELEMENT 2 - BIG 380
                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_1 + interLine_X_1, dist_Y + marginTop, el2_width, rect_height);
                        tf.DrawString(
                            item.Date.ToString("yyyy-MM-dd hh:mm tt"),
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20 + offSetX_1 + interLine_X_1, marginTop + dist_Y + 10, el2_width, el_height),
                            format);


                        //ELEMENT 3 - SMALL 80

                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_2 + interLine_X_2, dist_Y + marginTop, el3_width, rect_height);
                        tf.DrawString(
                            item.User.Id.ToString(),
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20 + offSetX_2 + 2 * interLine_X_2, marginTop + dist_Y + 10, el3_width, el_height),
                            format);

                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_3 + interLine_X_3, dist_Y + marginTop, el4_width, rect_height);
                        tf.DrawString(
                            item.User.FirstName,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20 + offSetX_3 + 2 * interLine_X_3, marginTop + dist_Y + 10, el4_width, el_height),
                            format);


                        graph.DrawRectangle(rect_style2_pen, rect_style1, marginLeft + offSetX_4 + interLine_X_4, dist_Y + marginTop, el5_width, rect_height);
                        tf.DrawString(
                            String.Format("{0:n0}", item.Amount - item.Fastfill) + " " + currencyCode,
                            fontParagraph,
                            XBrushes.Black,
                            new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height),
                            format);

                    }

                }


                i = i + 1;
                x = x + 1;

                dist_Y = lineHeight * (x + 1);
                dist_Y2 = dist_Y - 2;


                if ((marginTop + dist_Y) > 3250)
                {
                    tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
                                  new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

                    pageNo = pageNo + 1;

                    PdfPage newPage = document.AddPage();
                    newPage.Height = 3508;//842
                    newPage.Width = 2480;
                    graph = XGraphics.FromPdfPage(newPage);
                    tf = new XTextFormatter(graph);
                    x = -1;
                    dist_Y = lineHeight * (x + 1);
                    dist_Y2 = dist_Y - 2;

                }

                graph.DrawRectangle(rect_style2_pen, rect_style2, marginLeft, marginTop + dist_Y, (pdfPage.Width - 2 * marginLeft) + 8, rect_height);

                tf.DrawString(String.Format("{0:n0}", totalMonthlyAmount) + " " + currencyCode, fontParagraph, XBrushes.Black,
                              new XRect(marginLeft + 20 + offSetX_4 + 2 * interLine_X_4, marginTop + dist_Y + 10, el5_width, el_height), format);


            }

            tf.DrawString(pageNo.ToString(), fontParagraph, XBrushes.Black,
            new XRect(marginLeft, pdfPage.Height - 100, pdfPage.Width - 2 * marginLeft, el_height), format);

            var fileName = "attachment_" + System.Guid.NewGuid().ToString() + ".pdf";

            var path = Directory.GetCurrentDirectory() + @"\attachments\" + fileName;

            string fileURL = "https://fastfillpro.developitech.com/attachments/" + fileName;

            document.Save(path);

            var response = new
            {
                URL = fileURL,
            };

            return Ok(response);
        }

    }
}
