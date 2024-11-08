using BAL.Services.EmpRegister;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DAL.Model.EmpRegister.EmpRegister;

namespace EMP_WEB_API_PROJECT.Controllers.EmpRegister
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpRegisterController : ControllerBase
    {
        private IEmpRegisterService _empRegisterService;
        public EmpRegisterController(IEmpRegisterService empRegisterService)
        {
            _empRegisterService = empRegisterService;
        }

        [AllowAnonymous]

        [HttpGet]
        [Route("getAllEmpData")]
        public async Task<IActionResult> getAllEmpData()
        {


            EmpRegisterResponse response = new EmpRegisterResponse();
            try
            {
                response = await _empRegisterService.getAllEmpData("fetchData");
                return Ok(response);
            }
            catch (Exception ex)
            {

                response.isSuccess = false;
                response.message = ex.Message.ToString();
                response.data = new List<Data>();
               
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("postVisitor")]

        public async Task<IActionResult> PostEmp(EmpRegisterRequest request)
        {
            EmpRegisterResponse response = new EmpRegisterResponse();
            try
            {
                await _empRegisterService.EmpAsync(request, response, "saveData");
                return Ok(response);
            }
            catch (Exception ex)
            {

                response.isSuccess = false;
                response.message = ex.Message.ToString();
                response.data = new List<Data>();

            }
            return Ok(response);
        }

        [HttpPut]
        [Route("putVisitor")]
        public async Task<IActionResult> PutEmp(EmpRegisterRequest request, int Id)
        {
            request.Id = Id;
            EmpRegisterResponse response = new EmpRegisterResponse();
            try
            {
                await _empRegisterService.EmpAsync(request, response, "updateData");
                return Ok(response);
            }
            catch (Exception ex)
            {

                response.isSuccess = false;
                response.message = ex.Message.ToString();
                response.data = new List<Data>();

            }
            return Ok(response);
        }


        [HttpDelete]
        [Route("deleteEmpData/{Id}")]
        public async Task<IActionResult> deleteEmpData(int Id)
        {
            EmpRegisterResponse response = new EmpRegisterResponse();
            try
            {
                await _empRegisterService.deleteEmpData(response, "deleteData", Id);
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = ex.Message.ToString();
                response.data = new List<Data>();
            }
            return Ok(response);
        }

    }
}
