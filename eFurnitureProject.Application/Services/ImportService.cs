using AutoMapper;
using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.ImportDetailViewModels;
using eFurnitureProject.Application.ViewModels.ImportViewModels;
using eFurnitureProject.Domain.Entities;
using FluentValidation;
using System.Data.Common;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace eFurnitureProject.Application.Services
{
    public class ImportService : IImportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateImportDTO> _validatorCreateImport;
        private readonly IValidator<CreateImportDetailDTO> _validatorCreateImportDetail;
        private readonly IValidator<UpdateImportDTO> _validatorUpdateImport;

        public ImportService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateImportDTO> validatorCreateImport, IValidator<CreateImportDetailDTO> validatorCreateImportDetail, IValidator<UpdateImportDTO> validatorUpdateImport)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validatorCreateImport = validatorCreateImport;
            _validatorCreateImportDetail = validatorCreateImportDetail;
            _validatorUpdateImport = validatorUpdateImport;
        }

        public async Task<ApiResponse<ImportViewDTO>> CreateImportWithDetailAsync(CreateImportDTO import)
        {
            var response = new ApiResponse<ImportViewDTO>();
            try
            {
                var importObj = _mapper.Map<Import>(import);
                ValidationResult validationCreateImportResult = await _validatorCreateImport.ValidateAsync(import);
                ValidationResult validationCreateImportDetailResult = null;
                foreach (var importdetail in import.ImportDetail)
                {
                    validationCreateImportDetailResult = await _validatorCreateImportDetail.ValidateAsync(importdetail);
                }
                if (!validationCreateImportResult.IsValid || !validationCreateImportResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationCreateImportResult.Errors.Select(error => error.ErrorMessage)
                                                    , ", ", validationCreateImportDetailResult.Errors.Select((error => error.ErrorMessage)));
                    return response;
                }
                else
                {
                    await _unitOfWork.ImportRepository.AddWithDetailAsync(importObj);
                    var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSuccess is false)
                    {
                        throw new Exception("Create import with details is fail!");
                    }
                    response.Data = _mapper.Map<ImportViewDTO>(importObj);
                    response.Message = "Create import with details is successful!";
                }
            }
            catch (DbException ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse<Pagination<ImportViewDTO>>> GetImportPagingAsync(int pageIndex, int pageSize)
        {
            var response = new ApiResponse<Pagination<ImportViewDTO>>();
            var imports = await _unitOfWork.ImportRepository.ToPaginationIsNotDelete(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ImportViewDTO>>(imports);
            response.Data = result;
            return response;
        }

        public async Task<ApiResponse<ImportViewFullDTO>> GetImportDetailAsync(string importId)
        {
            var response = new ApiResponse<ImportViewFullDTO>();
            try
            {
                var import = await _unitOfWork.ImportRepository.GetImportWithDetail(Guid.Parse(importId));
                var result = _mapper.Map<ImportViewFullDTO>(import);
                response.Data = result;
                response.Message = "Get import successful!";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponse<string>> UpdateStatusImportAsync(UpdateImportDTO updateImport)
        {
            var response = new ApiResponse<string>();
            try
            {
                var existingImport = await _unitOfWork.ImportRepository.GetByIdAsync(Guid.Parse(updateImport.ImportId));
                var importObj = _mapper.Map<Import>(updateImport);
                ValidationResult validationResult = await _validatorUpdateImport.ValidateAsync(updateImport);
                if (!validationResult.IsValid)
                {
                    response.isSuccess = false;
                    response.Message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }
                if (existingImport.Status == importObj.Status || existingImport.Status > importObj.Status)
                {
                    throw new Exception($"Can not update status value {existingImport.Status} again");
                }
                existingImport.Status = importObj.Status;
                bool isSuccess;
                if (existingImport.Status == 2)
                {
                    var importDetail = await _unitOfWork.ImportDetailRepository.GetImportDetailsByIdAsync(existingImport.Id);
                    _unitOfWork.ProductRepository.IncreaseQuantityProductFromImport(importDetail);
                    if (await _unitOfWork.SaveChangeAsync() <= 0)
                    {
                        throw new Exception("Update status import and product quantity are fail!");
                    }
                    _unitOfWork.ImportRepository.Update(existingImport);
                    isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSuccess)
                    {
                        response.Message = "Update status import and product quantity are successful!";
                    }
                }
                else
                {
                    _unitOfWork.ImportRepository.Update(existingImport);
                    isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSuccess)
                    {
                        response.Message = "Update status import is successful!";
                    }
                }
                if (isSuccess is false)
                {
                    throw new Exception("Update status import fail");
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
