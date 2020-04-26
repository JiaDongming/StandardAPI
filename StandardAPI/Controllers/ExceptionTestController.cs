using StandardAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StandardAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [WebApiExceptionFilter]
    public class ExceptionTestController : ApiController
    {
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>返回</returns>
        ///  GET: api/ExceptionTest
        public IEnumerable<string> Get()
        {
            throw new NotImplementedException("方法不被支持");
        }
        /// <summary>
        /// 获取指定的id数据
        /// </summary>
        /// <param name="id">参数id</param>
        /// <returns></returns>
        public string Get(int id)
        {
            return "value" + id.ToString();
        }

        /// <summary>
        /// 更新指定id的数据
        /// </summary>
        /// <param name="id">指定id删除</param>
        /// <param name="value">参数value</param>
        /// <returns></returns>
        public int Put(int id, [FromBody] string value)
        {
            return id;
        }

        /// <summary>
        /// 删除指定id的条目
        /// </summary>
        /// <param name="id">指定id删除</param>
        /// <returns>返回删除的id</returns>
        public int Delete(int id)
        {
            return id;
        }
    }
}
