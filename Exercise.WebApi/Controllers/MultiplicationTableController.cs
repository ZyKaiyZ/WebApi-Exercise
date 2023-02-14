using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Exercise.ClassLibrary;

namespace Exercise.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultiplicatissonTableController : ControllerBase
    {
        //[HttpPost("{number:int:range(2,9)}")]
        [HttpPost]
        [SwaggerOperation(Summary = "取得Multiplicandr的99乘法表",
            Description = "透過Multiplicand取得99乘法表，Multiplicand限制為整數2-9，若Multiplicand為0或空取得2-9的99乘法表")]
        public ReturnData<List<GetMultiplicationTableResult>> GetMultiplicationTable([FromBody] GetMultiplicationTableParameter parameter)
        {   
            int num = parameter.Multiplicand;
            ReturnData<List<GetMultiplicationTableResult>> result = new ReturnData<List<GetMultiplicationTableResult>>();
            result.Data = new List<GetMultiplicationTableResult>();
            Boolean isSuccessful = (num >= 2 && num <= 9) || num == 0;
            result.Code = isSuccessful ? 200 : 400;
            result.Status = isSuccessful ? "Success." : "Failure.";
            if (isSuccessful)
            {
                if (num == 0)
                {
                    for (int i = 2; i < 10; i++)
                    {
                        result.Data.Add(new GetMultiplicationTableResult(i));
                    }
                }
                else
                {
                    result.Data.Add(new GetMultiplicationTableResult(num));
                }
            }
            return result;
        }
    }
}