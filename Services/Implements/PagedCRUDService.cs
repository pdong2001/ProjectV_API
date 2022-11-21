using AutoMapper;
using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Contracts;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Utils.Exceptions;

namespace Services.Implements
{
    public abstract class PagedCRUDService<TKey, TEntity, TDto, TUpSert, TLookUp> : IPagedCRUDService<TKey, TEntity, TDto, TUpSert, TLookUp>
        where TEntity : Entity<TKey>
        where TLookUp : PageLookUpDto
    {
        private IRepository<TKey, TEntity> _repos;
        protected IMapper _mapper;

        public PagedCRUDService(IRepository<TKey, TEntity> repos, IMapper mapper)
        {
            _repos = repos;
            _mapper = mapper;
        }

        public virtual async Task<ServiceResponse<TDto>> CreateAsync(TUpSert input)
        {
            var response = new ServiceResponse<TDto>();
            TEntity entity = _mapper.Map<TUpSert, TEntity>(input);
            if (await _repos.AddAsync(entity) != null)
            {
                response.SetValue(_mapper.Map<TEntity, TDto>(entity));
            }
            else
            {
                response.SetFailed();
            }
            return response;
        }

        public virtual async Task<ServiceResponse> DeleteAsync(TKey id)
        {
            if (await _repos.DeleteAsync(id))
            {
                return ServiceResponse.CreateSuccess();
            }
            return ServiceResponse.CreateFailed();
        }

        public virtual async Task<ServiceResponse<TDto>> GetAsync(TKey id)
        {
            var response = new ServiceResponse<TDto>();
            var query = _repos.GetQueryable();
            query = BeforeGet(id, query);
            var entity = await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
            if (entity != null)
            {
                var dto = _mapper.Map<TEntity, TDto>(entity);
                response.SetValue(dto);
            }
            else
            {
                response.SetFailed();
            }
            return response;
        }

        public virtual IQueryable<TEntity> BeforeGet(TKey id, IQueryable<TEntity> query) { return query; }

        public virtual async Task<ServiceResponse<List<TDto>>> GetListAsync()
        {
            var response = new ServiceResponse<List<TDto>>();
            var data = await _repos.ToListAsync();
            if (data != null && data.Any())
            {
                var dtos = data.Select(e => _mapper.Map<TEntity, TDto>(e)).ToList();
                response.SetValue(dtos);
            }
            else
            {
                response.SetFailed();
            }
            return response;
        }

        public virtual async Task<ServiceResponse<PageResultDto<TDto>>> SearchAsync(TLookUp request)
        {
            var response = new ServiceResponse<PageResultDto<TDto>>();
            var query = _repos.GetQueryable();
            query = BeforeSearch(query, request);
            if (!string.IsNullOrWhiteSpace(request.Columns))
                query = query.OrderBy(request.Columns + " " + request.Sort);
            long count = await query.LongCountAsync();
            var queryResult = await query.Skip((request.PageIndex - 1) & request.PageSize).Take(request.PageSize).ToListAsync();
            var dtos = queryResult.Select(e => _mapper.Map<TEntity, TDto>(e)).ToList();
            var result = new PageResultDto<TDto>
            {
                Items = dtos,
                Total = count,
            };
            response.SetValue(result);
            return response;
        }

        protected virtual IQueryable<TEntity> BeforeSearch(IQueryable<TEntity> query, TLookUp request) => query;

        public virtual async Task<ServiceResponse<TDto>> UpdateAsync(TKey id, TUpSert input)
        {
            var response = new ServiceResponse<TDto>();
            var entity = await _repos.GetAsync(id);
            if (entity == null) throw new UserFriendlyException("Not found");
            else
            {
                _mapper.Map(input, entity);
                if (await _repos.UpdateAsync(entity) != null)
                {
                    response.SetValue(_mapper.Map<TEntity, TDto>(entity));
                }
            }
            return response;
        }
    }
}
