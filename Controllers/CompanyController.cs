using AutoMapper;
using FastFill_API.Data;
using FastFill_API.Web.Dto;
using FastFill_API.Web.Model;
using FastFill_API.Web.Services;
using FastFill_API.Web.Utils;
using FastFill_API.Web.Utils.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> GetCompanyPaymentTransactions(int page = 1, int pageSize = 10)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<PaymentTransaction> companyPaymentTransactions = await _companyServices.GetCompanyPaymentTransactions(page, pageSize, paginationInfo, userId);

            var response = new
            {
                StationPaymentTransactions = companyPaymentTransactions,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }
    }
}
