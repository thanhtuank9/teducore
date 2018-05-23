﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TeduCore.Application.Systems.Roles;
using TeduCore.Application.Systems.Roles.Dtos;

namespace TeduCore.WebApi.Controllers
{
    public class RoleController : ApiController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AppRoleViewModel appRoleViewModel)
        {
            if (appRoleViewModel == null || !ModelState.IsValid)
                return new BadRequestResult();

            await _roleService.AddAsync(appRoleViewModel);
            return new OkResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AppRoleViewModel item)
        {
            if (item == null || item.Id != id)
            {
                return new BadRequestResult();
            }
            var role = await _roleService.GetById(id);
            if (role == null)
            {
                return new NotFoundResult();
            }

            await _roleService.UpdateAsync(item);

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _roleService.GetById(id);

            if (role == null)
            {
                return NotFound();
            }
            await _roleService.DeleteAsync(id);

            return new NoContentResult();
        }

        [HttpGet("{id}", Name = "GetRole")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _roleService.GetById(id);
            if (role == null)
                return new NotFoundResult();

            return new OkObjectResult(role);
        }
        [HttpGet]
        [Route("getAllPaging")]
        public async Task<IActionResult> GetAllPaging(int page, int pageSize, string filter = null)
        {
            var model = await _roleService.GetAllPagingAsync(filter, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _roleService.GetAllAsync();
            return new OkObjectResult(model);
        }
    }
}